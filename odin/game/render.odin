package game

// The software rasterizer: a port of the original's pixel sink, Font and the NeutralState layout.
// The picture is kept as hue indexes (like the original's OffscreenBuffer) and converted to RGBA at the end,
// so tests can compare it with the frames the VB game drew (docs/reference/vb).

Hue_Frame :: [FRAME_WIDTH * FRAME_HEIGHT]Hue

font_rows := FONT_ROWS // a constant array cannot be indexed at run time
font_advance := FONT_ADVANCE

// Pixels outside the view are ignored (as in the original's DisplayBuffer).
set_pixel :: proc(f: ^Hue_Frame, x, y: int, hue: Hue) {
	if x < 0 || y < 0 || x >= FRAME_WIDTH || y >= FRAME_HEIGHT { return }
	f[y * FRAME_WIDTH + x] = hue
}

fill_rect :: proc(f: ^Hue_Frame, x, y, w, h: int, hue: Hue) {
	for yy in max(y, 0) ..< min(y + h, FRAME_HEIGHT) {
		for xx in max(x, 0) ..< min(x + w, FRAME_WIDTH) { f[yy * FRAME_WIDTH + xx] = hue }
	}
}

fill_all :: proc(f: ^Hue_Frame, hue: Hue) { for i in 0 ..< len(f) { f[i] = hue } }

// The font is proportional: each glyph advances by its own width. Characters without a glyph (the original would throw)
// are drawn as a blank the width of a space.
glyph_index :: proc(ch: u8) -> int {
	if int(ch) >= FONT_FIRST && int(ch) < FONT_FIRST + FONT_COUNT { return int(ch) - FONT_FIRST }
	return 0
}

draw_text :: proc(f: ^Hue_Frame, x, y: int, text: string, hue: Hue) -> (end_x: int) {
	x := x
	for i in 0 ..< len(text) {
		g := glyph_index(text[i])
		if text[i] >= FONT_FIRST && int(text[i]) < FONT_FIRST + FONT_COUNT {
			rows := font_rows[g]
			for row in 0 ..< FONT_HEIGHT {
				bits := rows[row]
				for col in 0 ..< int(font_advance[g]) {
					if bits & (1 << uint(col)) != 0 { set_pixel(f, x + col, y + row, hue) }
				}
			}
		}
		x += int(font_advance[g])
	}
	return x
}

text_width :: proc(text: string) -> int {
	width := 0
	for i in 0 ..< len(text) { width += int(font_advance[glyph_index(text[i])]) }
	return width
}

draw_text_centered :: proc(f: ^Hue_Frame, y: int, text: string, hue: Hue) {
	draw_text(f, (FRAME_WIDTH - text_width(text)) / 2, y, text, hue)
}

// ---- the dialog screen (NeutralState.Render) ------------------------------------------------------------
CHOICE_COLUMNS :: 3
MAX_VIEW_LINES :: 24
MAX_VIEW_CHOICES :: 9
VIEW_TEXT_CAP :: 96

View_Line :: struct {
	text:   [VIEW_TEXT_CAP]u8,
	length: u8,
	mood:   Mood,
}

View_Choice :: struct {
	text:   [32]u8,
	length: u8,
}

// What a dialog shows: a hint line, description lines in moods, and the choices.
View :: struct {
	hint:         [32]u8,
	hint_length:  u8,
	lines:        [MAX_VIEW_LINES]View_Line,
	line_count:   int,
	choices:      [MAX_VIEW_CHOICES]View_Choice,
	choice_count: int,
}

mood_hue :: proc(mood: Mood) -> Hue {
	switch mood {
	case .Normal:  return .Light_Gray
	case .Success: return .Green
	case .Failure: return .Red
	case .Heading: return .Orange
	}
	return .Light_Gray
}

view_hint_text :: proc(v: ^View) -> string { return string(v.hint[:v.hint_length]) }
view_line_text :: proc(l: ^View_Line) -> string { return string(l.text[:l.length]) }
view_choice_text :: proc(c: ^View_Choice) -> string { return string(c.text[:c.length]) }

// Draws the dialog exactly as NeutralState.Render does: black, the hint, the lines, then the choices in three columns
// at the bottom, the selected one inverted (white box, black text).
render_dialog :: proc(f: ^Hue_Frame, v: ^View, cursor: int) {
	fill_all(f, .Black)
	y := 0
	draw_text(f, 0, y, view_hint_text(v), mood_hue(.Normal)); y += FONT_HEIGHT
	for i in 0 ..< v.line_count {
		line := &v.lines[i]
		draw_text(f, 0, y, view_line_text(line), mood_hue(line.mood)); y += FONT_HEIGHT
	}
	rows := (v.choice_count + CHOICE_COLUMNS - 1) / CHOICE_COLUMNS
	y = FRAME_HEIGHT - FONT_HEIGHT * rows
	column_width := FRAME_WIDTH / CHOICE_COLUMNS
	index := 0
	for _ in 0 ..< rows {
		for column in 0 ..< CHOICE_COLUMNS {
			if index < v.choice_count {
				text := view_choice_text(&v.choices[index])
				if index == cursor {
					fill_rect(f, column * column_width, y, column_width, FONT_HEIGHT, .White)
					draw_text(f, column * column_width, y, text, .Black)
				} else {
					fill_rect(f, column * column_width, y, column_width, FONT_HEIGHT, .Black)
					draw_text(f, column * column_width, y, text, .White)
				}
			}
			index += 1
		}
		y += FONT_HEIGHT
	}
}

// Hue indexes to the RGBA bytes the platforms present.
frame_to_rgba :: proc(f: ^Hue_Frame, rgba: ^[FRAME_WIDTH * FRAME_HEIGHT]u32) {
	for i in 0 ..< len(f) { rgba[i] = PALETTE[f[i]] }
}
