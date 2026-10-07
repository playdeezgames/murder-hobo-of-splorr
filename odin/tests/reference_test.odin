#+build !js
package tests

// The renderer against the real VB game. tools/vb-oracle drives the original headlessly and records what it draws
// (docs/reference/vb/*.txt: 216 lines of 384 hue digits in hex). These tests build the same world and require every pixel to match.

import "core:fmt"
import "core:testing"
import "kmh:game"

reference :: proc(t: ^testing.T, name: string, data: string, f: ^game.Hue_Frame, loc := #caller_location) {
	pos := 0
	bad := 0
	first_x, first_y := -1, -1
	for y in 0 ..< game.FRAME_HEIGHT {
		for x in 0 ..< game.FRAME_WIDTH {
			ch := data[pos]; pos += 1
			want := int(ch >= 'a' ? ch - 'a' + 10 : ch - '0')
			if int(f[y * game.FRAME_WIDTH + x]) != want {
				if bad == 0 { first_x, first_y = x, y }
				bad += 1
			}
		}
		pos += 1 // newline
	}
	testing.expectf(t, bad == 0, "%s: %d pixels differ from the VB frame, first at (%d, %d)", name, bad, first_x, first_y, loc = loc)
}

msg :: proc(w: ^game.World, mood: game.Mood, format: string, args: ..any) { game.message_add(w, mood, format, ..args) }

draw_neutral :: proc(w: ^game.World, cursor: int) -> ^game.Hue_Frame {
	f := new(game.Hue_Frame)
	v := new(game.View); defer free(v)
	game.neutral_view(w, 0, v)
	game.render_dialog(f, v, cursor)
	return f
}

draw_shoppe :: proc(w: ^game.World, cursor: int) -> ^game.Hue_Frame {
	f := new(game.Hue_Frame)
	v := new(game.View); defer free(v)
	game.shoppe_view(w, v)
	if cursor >= v.choice_count { return draw_shoppe_at(f, v, 0) } // the original resets an out-of-range cursor to the first choice
	return draw_shoppe_at(f, v, cursor)
}
draw_shoppe_at :: proc(f: ^game.Hue_Frame, v: ^game.View, cursor: int) -> ^game.Hue_Frame { game.render_dialog(f, v, cursor); return f }

@(test)
ref_neutral_fresh :: proc(t: ^testing.T) {
	w := game.world_new()
	f := draw_neutral(&w, 0); defer free(f)
	reference(t, "neutral_fresh", #load("../../docs/reference/vb/neutral_fresh.txt", string), f)
}

@(test)
ref_neutral_cursor_on_shoppe :: proc(t: ^testing.T) {
	w := game.world_new()
	f := draw_neutral(&w, 1); defer free(f)
	reference(t, "neutral_cursor_shoppe", #load("../../docs/reference/vb/neutral_cursor_shoppe.txt", string), f)
}

@(test)
ref_neutral_success :: proc(t: ^testing.T) {
	w := game.world_new()
	w.attempt_counter, w.murder_counter, w.success_streak, w.record_streak, w.experience = 1, 1, 1, 1, 1
	msg(&w, .Success, "Success!"); msg(&w, .Success, "New Record Success Streak!"); msg(&w, .Success, "You get 1 XP")
	f := draw_neutral(&w, 0); defer free(f)
	reference(t, "neutral_success", #load("../../docs/reference/vb/neutral_success.txt", string), f)
}

@(test)
ref_neutral_failure :: proc(t: ^testing.T) {
	w := game.world_new()
	w.attempt_counter, w.murder_counter, w.success_streak, w.record_streak, w.experience = 3, 2, 0, 2, 17
	w.skill, w.difficulty = 2, 3
	msg(&w, .Failure, "Failure!"); msg(&w, .Success, "You get 6 XP")
	f := draw_neutral(&w, 0); defer free(f)
	reference(t, "neutral_failure", #load("../../docs/reference/vb/neutral_failure.txt", string), f)
}

@(test)
ref_neutral_streak :: proc(t: ^testing.T) {
	w := game.world_new()
	w.attempt_counter, w.murder_counter, w.success_streak, w.record_streak, w.experience = 9, 7, 5, 5, 123
	msg(&w, .Success, "Success!"); msg(&w, .Success, "Streak bonus 4 XP!"); msg(&w, .Success, "New Record Success Streak!"); msg(&w, .Success, "You get 5 XP")
	f := draw_neutral(&w, 0); defer free(f)
	reference(t, "neutral_streak", #load("../../docs/reference/vb/neutral_streak.txt", string), f)
}

@(test)
ref_neutral_big_numbers :: proc(t: ^testing.T) {
	w := game.world_new()
	w.attempt_counter, w.murder_counter, w.success_streak, w.experience = 12345678, 9876543, 1234567, 2000000000
	w.skill, w.difficulty = 99999, 88888
	f := draw_neutral(&w, 0); defer free(f)
	reference(t, "neutral_big", #load("../../docs/reference/vb/neutral_big.txt", string), f)
}

@(test)
ref_shoppe_poor :: proc(t: ^testing.T) {
	w := game.world_new()
	f := draw_shoppe(&w, 1); defer free(f) // the original's cursor (1) is out of range for the single choice and resets to 0
	reference(t, "shoppe_poor", #load("../../docs/reference/vb/shoppe_poor.txt", string), f)
}

@(test)
ref_shoppe_rich :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = 5000
	f := draw_shoppe(&w, 1); defer free(f)
	reference(t, "shoppe_rich", #load("../../docs/reference/vb/shoppe_rich.txt", string), f)
}

@(test)
ref_shoppe_some :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = 30
	f := draw_shoppe(&w, 1); defer free(f)
	reference(t, "shoppe_some", #load("../../docs/reference/vb/shoppe_some.txt", string), f)
}

@(test)
ref_shoppe_cursor_last_and_second_row :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = 5000
	f := draw_shoppe(&w, 3); defer free(f)
	reference(t, "shoppe_cursor_last", #load("../../docs/reference/vb/shoppe_cursor_last.txt", string), f)
	g := draw_shoppe(&w, 3); defer free(g)
	reference(t, "shoppe_cursor_row2", #load("../../docs/reference/vb/shoppe_cursor_row2.txt", string), g)
}

@(test)
font_data_matches_the_original_json_shape :: proc(t: ^testing.T) {
	testing.expect_value(t, game.FONT_COUNT, 96)
	testing.expect_value(t, game.FONT_HEIGHT, 8)
	// 'A' (index 33): the top row is a single pixel in column 2 (CyFont5x7.json: line 0 is [2])
	rows := game.FONT_ROWS
	testing.expect_value(t, rows['A' - game.FONT_FIRST][0], u8(1 << 2))
	testing.expect_value(t, game.text_width("Hello"), 30)
	_ = fmt.tprintf // keep the import used by -vet
}
