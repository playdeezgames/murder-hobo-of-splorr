package game

// Interim core (phase 3): the neutral and shoppe dialogs of a real world, with the original's cursor movement.
// Phase 4 replaces the little Screen enum with the full state machine (splash, menus, game menu, ...).

Screen :: enum u8 { Neutral, Shoppe }

Core :: struct {
	services: Services,
	hues:     Hue_Frame,
	frame:    [FRAME_WIDTH * FRAME_HEIGHT]u32,
	dirty:    bool,
	world:    World,
	rng:      Rng,
	screen:   Screen,
	cursor:   [Screen]int, // remembered per screen (decision: quirk 6)
	view:     View,
	last_ms:  f64,
}

core_init :: proc(core: ^Core, services: Services) {
	core^ = {}
	core.services = services
	core.world = world_new()
	rng_seed(&core.rng, services.entropy != nil ? services.entropy() : 1)
	core.dirty = true
}

core_build_view :: proc(core: ^Core, now_ms: f64) {
	switch core.screen {
	case .Neutral: neutral_view(&core.world, now_ms, &core.view)
	case .Shoppe:  shoppe_view(&core.world, &core.view)
	}
}

// The original's cursor movement: three columns; Left/Right by one, Up/Down by a row, clamped.
cursor_move :: proc(cursor, count: int, command: Command) -> int {
	switch command {
	case .Right: return min(cursor + 1, count - 1)
	case .Left:  return max(cursor - 1, 0)
	case .Up:    return max(cursor - CHOICE_COLUMNS, 0)
	case .Down:  return min(cursor + CHOICE_COLUMNS, count - 1)
	case .None, .Confirm, .Cancel:
	}
	return cursor
}

core_choose :: proc(core: ^Core, now_ms: f64) {
	choice := view_choice_text(&core.view.choices[core.cursor[core.screen]])
	switch choice {
	case "Murder!":               attempt_murder(&core.world, &core.rng)
	case "Shoppe":                core.screen = .Shoppe
	case "Cancel":                core.screen = .Neutral
	case "Skill Increase":        buy_skill(&core.world)
	case "Difficulty Increase":   buy_difficulty(&core.world)
	case "Auto-murder Increase":  buy_auto(&core.world, now_ms)
	}
}

core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output) {
	if auto_tick(&core.world, &core.rng, input.now_ms) > 0 { core.dirty = true }
	core_build_view(core, input.now_ms)
	for e in input.events {
		if e.kind != .Command { continue }
		switch e.command {
		case .Up, .Down, .Left, .Right:
			core.cursor[core.screen] = cursor_move(core.cursor[core.screen], core.view.choice_count, e.command)
		case .Confirm:
			if core.cursor[core.screen] >= core.view.choice_count { core.cursor[core.screen] = 0 }
			core_choose(core, input.now_ms)
			core_build_view(core, input.now_ms)
		case .Cancel:
			if core.screen == .Shoppe { core.screen = .Neutral; core_build_view(core, input.now_ms) }
		case .None:
		}
		core.dirty = true
	}
	// the countdown line changes with the clock, so the neutral screen redraws while auto-murder runs
	if core.screen == .Neutral && core.world.has_auto && input.now_ms != core.last_ms { core.dirty = true }
	core.last_ms = input.now_ms
	out.frame_changed = core.dirty
	if core.dirty {
		if core.cursor[core.screen] >= core.view.choice_count { core.cursor[core.screen] = 0 } // as in the original's Render
		render_dialog(&core.hues, &core.view, core.cursor[core.screen])
		frame_to_rgba(&core.hues, &core.frame)
		core.dirty = false
	}
	out.frame = &core.frame
	out.quit_requested = false
}
