#+build !js
package tests

import "core:testing"
import "kmh:game"

fake_services :: proc() -> game.Services { return {} }

step_with :: proc(core: ^game.Core, out: ^game.Step_Output, events: ..game.Input_Event) {
	game.core_step(core, {dt = 0.016, now_ms = 0, events = events}, out)
}

@(test)
frame_is_presented_in_full :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	step_with(core, &out)
	testing.expect(t, out.frame != nil)
	testing.expect(t, out.frame_changed)
	// first swatch is Black, last is White; the frame's bottom-right pixel is the outline
	testing.expect_value(t, out.frame[0], game.PALETTE[.Black])
	testing.expect_value(t, out.frame[game.SWATCH_W * 15 + 1], game.PALETTE[.White])
	testing.expect_value(t, out.frame[game.FRAME_WIDTH * game.FRAME_HEIGHT - 1], game.PALETTE[.White])
	// a second step with no input changes nothing
	step_with(core, &out)
	testing.expect(t, !out.frame_changed)
}

@(test)
commands_move_and_clamp_the_marker :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	step_with(core, &out)
	x0, y0 := core.marker_x, core.marker_y
	step_with(core, &out, {kind = .Command, command = .Right}, {kind = .Command, command = .Down})
	testing.expect_value(t, core.marker_x, x0 + game.MARKER_STEP)
	testing.expect_value(t, core.marker_y, y0 + game.MARKER_STEP)
	testing.expect(t, out.frame_changed)
	for _ in 0 ..< 100 { step_with(core, &out, {kind = .Command, command = .Left}, {kind = .Command, command = .Up}) }
	testing.expect_value(t, core.marker_x, 0)
	testing.expect_value(t, core.marker_y, 0)
	step_with(core, &out, {kind = .Command, command = .Confirm})
	testing.expect_value(t, core.marker_hue, game.Hue.Black) // White wraps to Black
	step_with(core, &out, {kind = .Command, command = .Cancel})
	testing.expect_value(t, core.marker_x, x0)
	testing.expect_value(t, core.marker_hue, game.Hue.White)
}

@(test)
tap_puts_the_marker_under_the_pointer_and_stays_inside :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	out: game.Step_Output
	game.core_init(core, fake_services())
	step_with(core, &out, {kind = .Tap, x = 100, y = 100})
	testing.expect_value(t, core.marker_x, 100 - game.MARKER_SIZE / 2)
	step_with(core, &out, {kind = .Tap, x = 10_000, y = -50})
	testing.expect_value(t, core.marker_x, game.FRAME_WIDTH - game.MARKER_SIZE)
	testing.expect_value(t, core.marker_y, 0)
}

@(test)
palette_matches_the_original_hue_json :: proc(t: ^testing.T) {
	// hue.json: Blue 2A4BD7, Orange FF9233, White FFFFFF (stored 0xAABBGGRR)
	testing.expect_value(t, game.PALETTE[.Blue], u32(0xFFD74B2A))
	testing.expect_value(t, game.PALETTE[.Orange], u32(0xFF3392FF))
	testing.expect_value(t, game.PALETTE[.White], u32(0xFFFFFFFF))
	testing.expect_value(t, len(game.Hue), 16)
}
