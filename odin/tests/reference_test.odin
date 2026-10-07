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
font_is_the_proportional_m5x7 :: proc(t: ^testing.T) {
	testing.expect_value(t, game.FONT_COUNT, 96)
	testing.expect_value(t, game.FONT_HEIGHT, 9)
	// m5x7 advances: i is 3 wide, H 6, M 8 (ink plus one pixel of spacing)
	testing.expect_value(t, game.text_width("i"), 3)
	testing.expect_value(t, game.text_width("H"), 6)
	testing.expect_value(t, game.text_width("M"), 8)
	testing.expect_value(t, game.text_width("iHM"), 17)
	testing.expect_value(t, game.text_width(""), 0)
	// 'H' (index 40): its top row is the two stems, columns 0 and 4 (m5x7 draws a capital 5 wide)
	rows := game.FONT_ROWS
	testing.expect_value(t, rows['H' - game.FONT_FIRST][0], u8(1 << 0 | 1 << 4))
	// characters without a glyph take the width of a space and draw nothing
	testing.expect_value(t, game.text_width("\x01"), game.text_width(" "))
	_ = fmt.tprintf // keep the import used by -vet
}

@(test)
about_screen_credits_the_font :: proc(t: ^testing.T) {
	f := new(game.Hue_Frame); defer free(f)
	g := new(game.Hue_Frame); defer free(g)
	game.render_about(f, with_credit = false)
	game.render_about(g)
	differs := false
	for i in 0 ..< len(f) { if f[i] != g[i] { differs = true; break } }
	testing.expect(t, differs, "the credit line is drawn")
	// and it is the only thing that differs: the first two lines are the same
	for y in 0 ..< game.FONT_HEIGHT * 2 { for x in 0 ..< game.FRAME_WIDTH { testing.expect_value(t, g[y * game.FRAME_WIDTH + x], f[y * game.FRAME_WIDTH + x]) } }
}

// ---- the menu screens ------------------------------------------------------------------------------------
// The VB game's own item lists (with Scum Load, Load..., the volume screens), so the picker itself is compared.
VB_MAIN_MENU :: []string{"Embark!", "Scum Load", "Load...", "Options...", "About...", "Quit"}
VB_GAME_MENU :: []string{"Continue Game", "Scum Save", "Save...", "Scum Load", "Options...", "Abandon Game"}
VB_OPTIONS :: []string{"Toggle Full Screen", "Window Size...", "Sfx Volume...", "Mux Volume..."}
VB_SIZES := []string{"1152x648", "1536x864", "1920x1080", "3456x1944", "3840x2160", "5376x3024", "5760x3240", "7296x4104", "7680x4320"}
VB_CONTROLS :: "Spc/(A) - Sel | Esc/(B) - Cancel"

draw_menu :: proc(header, status: string, items: []string, index: int) -> ^game.Hue_Frame {
	f := new(game.Hue_Frame)
	game.render_menu(f, header, status, items, index)
	return f
}

@(test)
ref_splash :: proc(t: ^testing.T) {
	f := new(game.Hue_Frame); defer free(f)
	game.render_splash(f)
	reference(t, "splash", #load("../../docs/reference/vb/splash.txt", string), f)
}

@(test)
ref_about :: proc(t: ^testing.T) {
	f := new(game.Hue_Frame); defer free(f)
	game.render_about(f, with_credit = false)
	reference(t, "about", #load("../../docs/reference/vb/about.txt", string), f)
}

@(test)
ref_main_menu :: proc(t: ^testing.T) {
	status := game.controls_text("Sel", "Quit")
	f0 := draw_menu("Main Menu", status, VB_MAIN_MENU, 0); defer free(f0)
	reference(t, "main_menu", #load("../../docs/reference/vb/main_menu.txt", string), f0)
	f3 := draw_menu("Main Menu", status, VB_MAIN_MENU, 3); defer free(f3)
	reference(t, "main_menu_item3", #load("../../docs/reference/vb/main_menu_item3.txt", string), f3)
	f5 := draw_menu("Main Menu", status, VB_MAIN_MENU, 5); defer free(f5)
	reference(t, "main_menu_wrapped", #load("../../docs/reference/vb/main_menu_wrapped.txt", string), f5)
	free_all(context.temp_allocator)
}

@(test)
ref_game_menu_and_confirmations :: proc(t: ^testing.T) {
	status := game.controls_text("Sel", "Cancel")
	a := draw_menu("Menu...", status, VB_GAME_MENU, 0); defer free(a)
	reference(t, "game_menu", #load("../../docs/reference/vb/game_menu.txt", string), a)
	b := draw_menu("Menu...", status, VB_GAME_MENU, 5); defer free(b)
	reference(t, "game_menu_wrapped", #load("../../docs/reference/vb/game_menu_wrapped.txt", string), b)
	c := draw_menu("Are you sure you want to abandon?", status, {"No", "Yes"}, 0); defer free(c)
	reference(t, "confirm_abandon", #load("../../docs/reference/vb/confirm_abandon.txt", string), c)
	d := draw_menu("Are you sure you want to abandon?", status, {"No", "Yes"}, 1); defer free(d)
	reference(t, "confirm_abandon_yes", #load("../../docs/reference/vb/confirm_abandon_yes.txt", string), d)
	e := draw_menu("Are you sure you want to quit?", status, {"No", "Yes"}, 0); defer free(e)
	reference(t, "confirm_quit", #load("../../docs/reference/vb/confirm_quit.txt", string), e)
	free_all(context.temp_allocator)
}

@(test)
ref_options_and_window_size :: proc(t: ^testing.T) {
	status := game.controls_text("Sel", "Cancel")
	a := draw_menu("Options", status, VB_OPTIONS, 0); defer free(a)
	reference(t, "options", #load("../../docs/reference/vb/options.txt", string), a)
	b := draw_menu("Current Size: 1152x648", status, VB_SIZES, 0); defer free(b)
	reference(t, "window_size", #load("../../docs/reference/vb/window_size.txt", string), b)
	c := draw_menu("Current Size: 1152x648", status, VB_SIZES, 3); defer free(c)
	reference(t, "window_size_item4", #load("../../docs/reference/vb/window_size_item4.txt", string), c)
	free_all(context.temp_allocator)
}

// The port's own screens produce the same pixels as the reference frames where the item lists agree.
@(test)
window_size_screen_is_the_original_list :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	game.core_init(core, game.Services{desktop = true})
	game.enter(core, .Window_Size)
	items := game.menu_items(core)
	testing.expect_value(t, len(items), len(VB_SIZES))
	for item, i in items { testing.expect_value(t, item, VB_SIZES[i]) }
	free_all(context.temp_allocator)
}
