package game

// The core: one state machine over the screens of the original, driven once per presented frame.

Screen :: enum u8 { Splash, Main_Menu, Neutral, Shoppe, Game_Menu, Confirm_Abandon, About, Options, Window_Size, Confirm_Quit }

// The window sizes of the original's Options menu: the 384 by 216 view times these.
WINDOW_SCALES :: [9]int{3, 4, 5, 9, 10, 14, 15, 19, 20}
DEFAULT_WINDOW_SCALE :: 3

Core :: struct {
	services:       Services,
	hues:           Hue_Frame,
	frame:          [FRAME_WIDTH * FRAME_HEIGHT]u32,
	dirty:          bool,
	world:          World,
	has_world:      bool, // a world is in play (Embark! or Continue Game) and not abandoned
	rng:            Rng,
	screen:         Screen,
	menu_index:     int,         // the picker screens' cursor; reset whenever one is entered
	cursor:         [Screen]int, // the dialogs' cursor, remembered per screen (decision: quirk 6)
	options_return: Screen,      // where Options returns to (the original pushes it over the caller)
	view:           View,
	last_ms:        f64,
	fullscreen:     bool,
	window_scale:   int,
	quit:           bool,
	save_failed:    bool,        // a failed save was already reported
	save_pending:   bool,        // the world may have changed since the last save
	config_pending: bool,
}

core_init :: proc(core: ^Core, services: Services) {
	core^ = {}
	core.services = services
	core.world = world_new()
	core.window_scale = DEFAULT_WINDOW_SCALE
	rng_seed(&core.rng, services.entropy != nil ? services.entropy() : 1)
	core_load(core)
	core.dirty = true
}

core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output) {
	now := input.now_ms
	// auto-murder runs while a world is on the neutral screen or in the Shoppe (decision: quirk 3)
	if core.has_world && (core.screen == .Neutral || core.screen == .Shoppe) {
		if auto_tick(&core.world, &core.rng, now) > 0 { core.dirty = true; core.save_pending = true }
	}
	for e in input.events {
		if e.kind == .Command && e.command != .None {
			handle_command(core, e.command, now)
			core.dirty = true
			if core.has_world { core.save_pending = true }
		}
	}
	// the countdown line changes with the clock, so the neutral screen redraws while auto-murder runs
	if core.screen == .Neutral && core.world.has_auto && now != core.last_ms { core.dirty = true }
	core.last_ms = now
	if core.save_pending { core_save(core); core.save_pending = false }
	if core.config_pending { core_save_config(core); core.config_pending = false }
	out.frame_changed = core.dirty
	if core.dirty {
		render_screen(core, now)
		frame_to_rgba(&core.hues, &core.frame)
		core.dirty = false
	}
	out.frame = &core.frame
	out.quit_requested = core.quit
	out.fullscreen = core.fullscreen
	out.window_scale = core.window_scale
}
