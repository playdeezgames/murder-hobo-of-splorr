#+build !js
package tests

import "core:testing"
import "kmh:game"

fake_services :: proc() -> game.Services { return {entropy = proc() -> u64 { return 5 }} }

step_with :: proc(core: ^game.Core, out: ^game.Step_Output, now_ms: f64, events: ..game.Input_Event) {
	game.core_step(core, {dt = 0.016, now_ms = now_ms, events = events}, out)
}

press :: proc(command: game.Command) -> game.Input_Event { return {kind = .Command, command = command} }

@(test)
the_first_frame_shows_the_neutral_dialog :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	step_with(core, &out, 0)
	testing.expect(t, out.frame != nil && out.frame_changed)
	testing.expect_value(t, game.view_hint_text(&core.view), "(Escape -> Game Menu)")
	// the first choice is selected: a white box at the bottom left
	testing.expect_value(t, out.frame[(game.FRAME_HEIGHT - 1) * game.FRAME_WIDTH], game.PALETTE[.White])
	step_with(core, &out, 0)
	testing.expect(t, !out.frame_changed, "nothing happened, so nothing is redrawn")
}

@(test)
cursor_moves_in_three_columns_and_clamps :: proc(t: ^testing.T) {
	testing.expect_value(t, game.cursor_move(0, 4, .Right), 1)
	testing.expect_value(t, game.cursor_move(3, 4, .Right), 3)
	testing.expect_value(t, game.cursor_move(0, 4, .Left), 0)
	testing.expect_value(t, game.cursor_move(1, 4, .Down), 3) // 1 + 3 = 4, clamped to the last
	testing.expect_value(t, game.cursor_move(3, 4, .Up), 0)
	testing.expect_value(t, game.cursor_move(0, 2, .Down), 1)
}

@(test)
playing_through_the_dialogs :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	step_with(core, &out, 0, press(.Confirm)) // Murder!
	testing.expect_value(t, core.world.attempt_counter, 1)
	step_with(core, &out, 0, press(.Right), press(.Confirm)) // Shoppe
	testing.expect_value(t, core.screen, game.Screen.Shoppe)
	testing.expect_value(t, game.view_hint_text(&core.view), "(Escape -> Go Back)")
	step_with(core, &out, 0, press(.Cancel))
	testing.expect_value(t, core.screen, game.Screen.Neutral)
	step_with(core, &out, 0, press(.Cancel)) // nothing to go back to yet (the game menu comes in phase 4)
	testing.expect_value(t, core.screen, game.Screen.Neutral)
}

@(test)
each_screen_remembers_its_own_cursor :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	core.world.experience = 5000
	step_with(core, &out, 0, press(.Right), press(.Confirm)) // into the Shoppe
	testing.expect_value(t, core.cursor[.Shoppe], 0) // not the neutral screen's 1: no accidental purchase
	step_with(core, &out, 0, press(.Right), press(.Confirm)) // buy a skill increase
	testing.expect_value(t, core.world.skill, 2)
	testing.expect_value(t, core.cursor[.Shoppe], 1)
	step_with(core, &out, 0, press(.Cancel))
	testing.expect_value(t, core.cursor[.Neutral], 1) // back on Shoppe
}

@(test)
auto_murder_runs_from_the_frame_step_and_redraws :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	core.world.experience = 1000
	step_with(core, &out, 1000)
	game.buy_auto(&core.world, 1000)
	step_with(core, &out, 1000 + 30_000)
	testing.expect_value(t, core.world.attempt_counter, 0)
	step_with(core, &out, 1000 + 60_000)
	testing.expect_value(t, core.world.attempt_counter, 1)
	testing.expect(t, out.frame_changed)
}
