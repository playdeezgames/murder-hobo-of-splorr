package game

// Phase 1 scaffold: a test frame that proves both platforms present the same pixels and deliver input.
// A row of the 16 hues, an outline of the frame, and a marker moved by the commands (Confirm cycles its hue,
// Cancel puts it back, a Tap puts it under the pointer).

SWATCH_W :: FRAME_WIDTH / 16
SWATCH_H :: 24
MARKER_SIZE :: 16
MARKER_STEP :: 8

Core :: struct {
	services: Services,
	frame:    [FRAME_WIDTH * FRAME_HEIGHT]u32,
	dirty:    bool,
	marker_x: int,
	marker_y: int,
	marker_hue: Hue,
}

core_init :: proc(core: ^Core, services: Services) {
	core^ = {}
	core.services = services
	marker_reset(core)
	core.dirty = true
}

marker_reset :: proc(core: ^Core) {
	core.marker_x = (FRAME_WIDTH - MARKER_SIZE) / 2
	core.marker_y = (FRAME_HEIGHT - MARKER_SIZE) / 2
	core.marker_hue = .White
}

marker_move :: proc(core: ^Core, dx, dy: int) {
	core.marker_x = clamp(core.marker_x + dx, 0, FRAME_WIDTH - MARKER_SIZE)
	core.marker_y = clamp(core.marker_y + dy, 0, FRAME_HEIGHT - MARKER_SIZE)
}

core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output) {
	for e in input.events {
		#partial switch e.kind {
		case .Command:
			#partial switch e.command {
			case .Up:      marker_move(core, 0, -MARKER_STEP)
			case .Down:    marker_move(core, 0, MARKER_STEP)
			case .Left:    marker_move(core, -MARKER_STEP, 0)
			case .Right:   marker_move(core, MARKER_STEP, 0)
			case .Confirm: core.marker_hue = Hue((int(core.marker_hue) + 1) % len(Hue))
			case .Cancel:  marker_reset(core)
			}
			core.dirty = true
		case .Tap:
			core.marker_x = clamp(int(e.x) - MARKER_SIZE / 2, 0, FRAME_WIDTH - MARKER_SIZE)
			core.marker_y = clamp(int(e.y) - MARKER_SIZE / 2, 0, FRAME_HEIGHT - MARKER_SIZE)
			core.dirty = true
		}
	}
	out.frame_changed = core.dirty
	if core.dirty { render_test_frame(core); core.dirty = false }
	out.frame = &core.frame
	out.quit_requested = false
}

fill_rect :: proc(core: ^Core, x, y, w, h: int, hue: Hue) {
	for yy in max(y, 0) ..< min(y + h, FRAME_HEIGHT) {
		for xx in max(x, 0) ..< min(x + w, FRAME_WIDTH) { core.frame[yy * FRAME_WIDTH + xx] = PALETTE[hue] }
	}
}

render_test_frame :: proc(core: ^Core) {
	fill_rect(core, 0, 0, FRAME_WIDTH, FRAME_HEIGHT, .Black)
	for hue in Hue { fill_rect(core, int(hue) * SWATCH_W, 0, SWATCH_W, SWATCH_H, hue) }
	// a one pixel outline shows the exact edges of the frame
	fill_rect(core, 0, FRAME_HEIGHT - 1, FRAME_WIDTH, 1, .White)
	fill_rect(core, 0, SWATCH_H, 1, FRAME_HEIGHT - SWATCH_H, .White)
	fill_rect(core, FRAME_WIDTH - 1, SWATCH_H, 1, FRAME_HEIGHT - SWATCH_H, .White)
	fill_rect(core, core.marker_x, core.marker_y, MARKER_SIZE, MARKER_SIZE, core.marker_hue)
}
