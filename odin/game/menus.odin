package game

// The menu-style screens of the original (AOS.UI BasePickerState, SplashState, AboutState and MHOSContext's splash and about content).

import "core:fmt"

GAME_TITLE :: "Murder Hobo of SPLORR!!"
GAME_SUBTITLE :: "Itinerance and slaughter!"
CONTINUE_TEXT :: "Cont"

// "Spc/(A) - Sel | Esc/(B) - Cancel": either part may be empty and is then left out.
controls_text :: proc(a_text, b_text: string, allocator := context.temp_allocator) -> string {
	switch {
	case a_text != "" && b_text != "": return fmt.aprintf("Spc/(A) - %s | Esc/(B) - %s", a_text, b_text, allocator = allocator)
	case a_text != "":                 return fmt.aprintf("Spc/(A) - %s", a_text, allocator = allocator)
	case b_text != "":                 return fmt.aprintf("Esc/(B) - %s", b_text, allocator = allocator)
	}
	return ""
}

// ShowHeader: a black row with the text centred in orange. ShowStatusBar: a light gray row with black text.
draw_header :: proc(f: ^Hue_Frame, text: string, foreground, background: Hue) {
	fill_rect(f, 0, 0, FRAME_WIDTH, FONT_HEIGHT, background)
	draw_text(f, FRAME_WIDTH / 2 - text_width(text) / 2, 0, text, foreground)
}
draw_status_bar :: proc(f: ^Hue_Frame, text: string, foreground, background: Hue) {
	fill_rect(f, 0, FRAME_HEIGHT - FONT_HEIGHT, FRAME_WIDTH, FONT_HEIGHT, background)
	draw_text(f, FRAME_WIDTH / 2 - text_width(text) / 2, FRAME_HEIGHT - FONT_HEIGHT, text, foreground)
}

// BasePickerState.Render: a blue bar across the middle holds the selected item (black text); the others scroll past it in blue.
render_menu :: proc(f: ^Hue_Frame, header, status: string, items: []string, index: int) {
	fill_all(f, .Black)
	middle := FRAME_HEIGHT / 2 - FONT_HEIGHT / 2
	fill_rect(f, 0, middle, FRAME_WIDTH, FONT_HEIGHT, .Blue)
	y := middle - index * FONT_HEIGHT
	for item, i in items {
		x := FRAME_WIDTH / 2 - text_width(item) / 2
		draw_text(f, x, y, item, i == index ? .Black : .Blue)
		y += FONT_HEIGHT
	}
	draw_header(f, header, .Orange, .Black)
	draw_status_bar(f, status, .Black, .Light_Gray)
}

// MHOSContext.ShowSplashContent: the title in brown with a tan outline, the subtitle below it, and the status bar.
render_splash :: proc(f: ^Hue_Frame) {
	fill_all(f, .Black)
	x := FRAME_WIDTH / 2 - text_width(GAME_TITLE) / 2
	y := FRAME_HEIGHT / 2 - FONT_HEIGHT * 3 / 2
	for dy in -1 ..= 1 {
		for dx in -1 ..= 1 {
			if dx == 0 && dy == 0 { continue }
			draw_text(f, x + dx, y + dy, GAME_TITLE, .Tan)
		}
	}
	draw_text(f, x, y, GAME_TITLE, .Brown)
	draw_text(f, FRAME_WIDTH / 2 - text_width(GAME_SUBTITLE) / 2, FRAME_HEIGHT / 2 + FONT_HEIGHT * 3 / 2, GAME_SUBTITLE, .Dark_Gray)
	draw_status_bar(f, controls_text(CONTINUE_TEXT, ""), .Black, .Light_Gray)
}

// The original had two lines. `with_credit` adds the font credit (m5x7 is CC0 and asks for attribution); the reference
// test draws it without, to compare with the VB screen.
render_about :: proc(f: ^Hue_Frame, with_credit := true) {
	fill_all(f, .Black)
	draw_text(f, 0, 0, "About Murder Hobo of SPLORR!!", .Orange)
	draw_text(f, 0, FONT_HEIGHT, "A Production of TheGrumpyGameDev", .White)
	if with_credit { draw_text(f, 0, FONT_HEIGHT * 3, "Font: m5x7 by Daniel Linssen (managore)", .Light_Gray) }
}
