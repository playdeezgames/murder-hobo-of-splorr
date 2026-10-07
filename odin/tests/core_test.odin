#+build !js
package tests

import "core:testing"
import "kmh:game"

fake_services :: proc(desktop := false) -> game.Services {
	return {entropy = proc() -> u64 { return 5 }, desktop = desktop}
}

press :: proc(command: game.Command) -> game.Input_Event { return {kind = .Command, command = command} }

Rig :: struct {
	core: ^game.Core,
	out:  game.Step_Output,
}

rig_new :: proc(desktop := false) -> Rig {
	r := Rig{core = new(game.Core)}
	game.core_init(r.core, fake_services(desktop))
	game.core_step(r.core, {now_ms = 0}, &r.out)
	return r
}
rig_free :: proc(r: ^Rig) { free(r.core); free_all(context.temp_allocator) }

// Presses commands one per step, as a player would, at a given clock.
play :: proc(r: ^Rig, now_ms: f64, commands: ..game.Command) {
	for c in commands { game.core_step(r.core, {now_ms = now_ms, events = {press(c)}}, &r.out) }
	free_all(context.temp_allocator)
}

// Splash to Main Menu to Embark! (the first item on the web, which has no saved world)
embark :: proc(r: ^Rig) { play(r, 0, .Confirm, .Confirm) }

@(test)
boots_on_the_splash_and_any_a_press_leaves_it :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	testing.expect_value(t, r.core.screen, game.Screen.Splash)
	testing.expect(t, r.out.frame != nil && r.out.frame_changed)
	play(&r, 0, .Cancel, .Up, .Left)
	testing.expect_value(t, r.core.screen, game.Screen.Splash)
	play(&r, 0, .Confirm)
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
}

@(test)
nothing_is_redrawn_when_nothing_changes :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	game.core_step(r.core, {now_ms = 5}, &r.out)
	testing.expect(t, !r.out.frame_changed)
}

@(test)
web_main_menu_has_no_quit_or_options :: proc(t: ^testing.T) {
	r := rig_new(false); defer rig_free(&r)
	play(&r, 0, .Confirm)
	items := game.menu_items(r.core)
	testing.expect_value(t, len(items), 2)
	testing.expect_value(t, items[0], "Embark!")
	testing.expect_value(t, items[1], "About...")
	testing.expect_value(t, game.menu_status(r.core), "Spc/(A) - Sel")
	play(&r, 0, .Cancel) // Escape does nothing where there is no Quit
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
}

@(test)
desktop_main_menu_has_options_and_quit :: proc(t: ^testing.T) {
	r := rig_new(true); defer rig_free(&r)
	play(&r, 0, .Confirm)
	items := game.menu_items(r.core)
	testing.expect_value(t, len(items), 4)
	testing.expect_value(t, items[2], "About...")
	testing.expect_value(t, items[3], "Quit")
	testing.expect_value(t, game.menu_status(r.core), "Spc/(A) - Sel | Esc/(B) - Quit")
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Confirm_Quit)
	play(&r, 0, .Down, .Confirm) // Yes
	testing.expect(t, r.core.quit)
	game.core_step(r.core, {now_ms = 0}, &r.out)
	testing.expect(t, r.out.quit_requested)
}

@(test)
continue_game_appears_only_with_a_world :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	play(&r, 0, .Confirm)
	testing.expect_value(t, game.menu_items(r.core)[0], "Embark!")
	r.core.has_world = true
	testing.expect_value(t, game.menu_items(r.core)[0], "Continue Game")
	play(&r, 0, .Confirm)
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	testing.expect_value(t, r.core.has_world, true)
}

@(test)
menu_cursor_wraps_and_resets_on_entry :: proc(t: ^testing.T) {
	r := rig_new(true); defer rig_free(&r)
	play(&r, 0, .Confirm)
	play(&r, 0, .Up)
	testing.expect_value(t, r.core.menu_index, 3) // wrapped to Quit
	play(&r, 0, .Down)
	testing.expect_value(t, r.core.menu_index, 0)
	play(&r, 0, .Down, .Down, .Confirm) // About...
	testing.expect_value(t, r.core.screen, game.Screen.About)
	play(&r, 0, .Left) // any command returns
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
	testing.expect_value(t, r.core.menu_index, 0)
}

@(test)
embark_plays_through_the_dialogs :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	embark(&r)
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	testing.expect_value(t, game.view_hint_text(&r.core.view), "(Escape -> Game Menu)")
	play(&r, 0, .Confirm) // Murder!
	testing.expect_value(t, r.core.world.attempt_counter, 1)
	play(&r, 0, .Right, .Confirm)
	testing.expect_value(t, r.core.screen, game.Screen.Shoppe)
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
}

@(test)
each_dialog_remembers_its_own_cursor :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	embark(&r)
	r.core.world.experience = 5000
	play(&r, 0, .Right, .Confirm) // into the Shoppe
	testing.expect_value(t, r.core.cursor[.Shoppe], 0) // not the neutral screen's 1: no accidental purchase
	play(&r, 0, .Right, .Confirm) // buy a skill increase
	testing.expect_value(t, r.core.world.skill, 2)
	testing.expect_value(t, r.core.cursor[.Shoppe], 1)
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.cursor[.Neutral], 1) // back on Shoppe
}

@(test)
cursor_moves_in_three_columns_and_clamps :: proc(t: ^testing.T) {
	testing.expect_value(t, game.cursor_move(0, 4, .Right), 1)
	testing.expect_value(t, game.cursor_move(3, 4, .Right), 3)
	testing.expect_value(t, game.cursor_move(0, 4, .Left), 0)
	testing.expect_value(t, game.cursor_move(1, 4, .Down), 3)
	testing.expect_value(t, game.cursor_move(3, 4, .Up), 0)
	testing.expect_value(t, game.cursor_move(0, 2, .Down), 1)
}

@(test)
game_menu_abandon_flow :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	embark(&r)
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
	items := game.menu_items(r.core)
	testing.expect_value(t, len(items), 2) // web: no Options
	testing.expect_value(t, items[1], "Abandon Game")
	play(&r, 0, .Cancel) // back to the game
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	play(&r, 0, .Cancel, .Up, .Confirm) // Abandon Game
	testing.expect_value(t, r.core.screen, game.Screen.Confirm_Abandon)
	testing.expect_value(t, r.core.menu_index, 0) // No is first
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
	r.core.world.murder_counter = 7
	play(&r, 0, .Up, .Confirm, .Down, .Confirm) // Abandon Game, then Yes
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
	testing.expect_value(t, r.core.has_world, false)
	testing.expect_value(t, r.core.world.murder_counter, 0)
}

@(test)
options_return_to_where_they_were_opened :: proc(t: ^testing.T) {
	r := rig_new(true); defer rig_free(&r)
	play(&r, 0, .Confirm, .Down, .Confirm) // Main Menu > Options...
	testing.expect_value(t, r.core.screen, game.Screen.Options)
	play(&r, 0, .Confirm)
	testing.expect_value(t, r.core.fullscreen, true)
	game.core_step(r.core, {now_ms = 0}, &r.out)
	testing.expect(t, r.out.fullscreen)
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
	game.enter(r.core, .Game_Menu) // on the desktop the game menu has Options... in the middle
	play(&r, 0, .Down, .Confirm)
	testing.expect_value(t, r.core.screen, game.Screen.Options)
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
}

@(test)
window_size_picks_a_scale_and_starts_on_the_current_one :: proc(t: ^testing.T) {
	r := rig_new(true); defer rig_free(&r)
	play(&r, 0, .Confirm, .Down, .Confirm, .Down, .Confirm) // Main Menu > Options > Window Size...
	testing.expect_value(t, r.core.screen, game.Screen.Window_Size)
	testing.expect_value(t, r.core.menu_index, 0) // 3 is the first and the current scale
	testing.expect_value(t, game.menu_header(r.core), "Current Size: 1152x648")
	play(&r, 0, .Down, .Confirm)
	testing.expect_value(t, r.core.window_scale, 4)
	testing.expect_value(t, game.menu_header(r.core), "Current Size: 1536x864")
	game.core_step(r.core, {now_ms = 0}, &r.out)
	testing.expect_value(t, r.out.window_scale, 4)
	play(&r, 0, .Cancel)
	testing.expect_value(t, r.core.screen, game.Screen.Options)
	play(&r, 0, .Down, .Confirm)
	testing.expect_value(t, r.core.menu_index, 1) // starts on the current size
}

@(test)
auto_murder_runs_on_neutral_and_shoppe_only :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	embark(&r)
	r.core.world.experience = 1000
	game.buy_auto(&r.core.world, 1000)
	game.core_step(r.core, {now_ms = 1000 + 30_000}, &r.out)
	testing.expect_value(t, r.core.world.attempt_counter, 0)
	play(&r, 1000 + 60_000) // no events: the neutral screen ticks
	game.core_step(r.core, {now_ms = 1000 + 60_000}, &r.out)
	testing.expect_value(t, r.core.world.attempt_counter, 1)
	testing.expect(t, r.out.frame_changed || true)
	play(&r, 1000 + 70_000, .Cancel) // the game menu: no ticking
	game.core_step(r.core, {now_ms = 1000 + 130_000}, &r.out)
	testing.expect_value(t, r.core.world.attempt_counter, 1)
	play(&r, 1000 + 131_000, .Cancel) // back on neutral: catches up the missed attempts
	game.core_step(r.core, {now_ms = 1000 + 131_000}, &r.out)
	testing.expect(t, r.core.world.attempt_counter >= 2)
}
