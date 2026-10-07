package game

// The screens and their transitions: ports of the AOS.UI boilerplate states and MHOS's NeutralState.

import "core:fmt"

TEXT_CONTINUE_GAME :: "Continue Game"
TEXT_EMBARK :: "Embark!"
TEXT_OPTIONS :: "Options..."
TEXT_ABOUT :: "About..."
TEXT_QUIT :: "Quit"
TEXT_ABANDON :: "Abandon Game"
TEXT_FULLSCREEN :: "Toggle Full Screen"
TEXT_WINDOW_SIZE :: "Window Size..."
TEXT_NO :: "No"
TEXT_YES :: "Yes"

window_scales := WINDOW_SCALES // a constant array cannot be indexed at run time

// The picker screens' header and items. Strings live in the temp allocator (freed by the platform each frame).
menu_header :: proc(core: ^Core) -> string {
	#partial switch core.screen {
	case .Main_Menu:       return "Main Menu"
	case .Game_Menu:       return "Menu..."
	case .Confirm_Abandon: return "Are you sure you want to abandon?"
	case .Confirm_Quit:    return "Are you sure you want to quit?"
	case .Options:         return "Options"
	case .Window_Size:     return fmt.tprintf("Current Size: %dx%d", FRAME_WIDTH * core.window_scale, FRAME_HEIGHT * core.window_scale)
	}
	return ""
}

menu_status :: proc(core: ^Core) -> string {
	#partial switch core.screen {
	case .Main_Menu: return controls_text("Sel", core.services.desktop ? "Quit" : "")
	}
	return controls_text("Sel", "Cancel")
}

menu_items :: proc(core: ^Core) -> []string {
	items := make([dynamic]string, context.temp_allocator)
	#partial switch core.screen {
	case .Main_Menu:
		if core.has_world { append(&items, TEXT_CONTINUE_GAME) }
		append(&items, TEXT_EMBARK)
		if core.services.desktop { append(&items, TEXT_OPTIONS) }
		append(&items, TEXT_ABOUT)
		if core.services.desktop { append(&items, TEXT_QUIT) }
	case .Game_Menu:
		append(&items, TEXT_CONTINUE_GAME)
		if core.services.desktop { append(&items, TEXT_OPTIONS) }
		append(&items, TEXT_ABANDON)
	case .Confirm_Abandon, .Confirm_Quit:
		append(&items, TEXT_NO)
		append(&items, TEXT_YES)
	case .Options:
		append(&items, TEXT_FULLSCREEN)
		append(&items, TEXT_WINDOW_SIZE)
	case .Window_Size:
		for scale in WINDOW_SCALES { append(&items, fmt.tprintf("%dx%d", FRAME_WIDTH * scale, FRAME_HEIGHT * scale)) }
	}
	return items[:]
}

is_picker :: proc(s: Screen) -> bool {
	switch s {
	case .Main_Menu, .Game_Menu, .Confirm_Abandon, .Confirm_Quit, .Options, .Window_Size: return true
	case .Splash, .Neutral, .Shoppe, .About: return false
	}
	return false
}

// Entering a picker screen resets its cursor (BasePickerState.OnStart); the window size list starts on the current size.
enter :: proc(core: ^Core, screen: Screen) {
	core.screen = screen
	core.menu_index = 0
	if screen == .Window_Size {
		for scale, i in WINDOW_SCALES { if scale == core.window_scale { core.menu_index = i } }
	}
}

handle_command :: proc(core: ^Core, command: Command, now_ms: f64) {
	switch core.screen {
	case .Splash:
		if command == .Confirm { enter(core, .Main_Menu) }
	case .About:
		enter(core, .Main_Menu) // any command leaves it
	case .Neutral, .Shoppe:
		handle_dialog_command(core, command, now_ms)
	case .Main_Menu, .Game_Menu, .Confirm_Abandon, .Confirm_Quit, .Options, .Window_Size:
		handle_picker_command(core, command)
	}
}

handle_dialog_command :: proc(core: ^Core, command: Command, now_ms: f64) {
	build_dialog_view(core, now_ms)
	cursor := &core.cursor[core.screen]
	switch command {
	case .Up, .Down, .Left, .Right:
		cursor^ = cursor_move(cursor^, core.view.choice_count, command)
	case .Confirm:
		if cursor^ >= core.view.choice_count { cursor^ = 0 }
		choose(core, view_choice_text(&core.view.choices[cursor^]), now_ms)
	case .Cancel:
		if core.screen == .Shoppe { core.screen = .Neutral } else { enter(core, .Game_Menu) }
	case .None:
	}
}

build_dialog_view :: proc(core: ^Core, now_ms: f64) {
	if core.screen == .Shoppe { shoppe_view(&core.world, &core.view) } else { neutral_view(&core.world, now_ms, &core.view) }
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

choose :: proc(core: ^Core, choice: string, now_ms: f64) {
	switch choice {
	case "Murder!":               attempt_murder(&core.world, &core.rng)
	case "Shoppe":                core.screen = .Shoppe
	case "Cancel":                core.screen = .Neutral
	case "Skill Increase":        buy_skill(&core.world)
	case "Difficulty Increase":   buy_difficulty(&core.world)
	case "Auto-murder Increase":  buy_auto(&core.world, now_ms)
	}
}

handle_picker_command :: proc(core: ^Core, command: Command) {
	items := menu_items(core)
	switch command {
	case .Up:   core.menu_index = (core.menu_index + len(items) - 1) % len(items)
	case .Down: core.menu_index = (core.menu_index + 1) % len(items)
	case .Cancel: picker_cancel(core)
	case .Confirm: picker_activate(core, items[core.menu_index])
	case .Left, .Right, .None:
	}
}

picker_cancel :: proc(core: ^Core) {
	#partial switch core.screen {
	case .Main_Menu:       if core.services.desktop { enter(core, .Confirm_Quit) }
	case .Game_Menu:       core.screen = .Neutral
	case .Confirm_Abandon: enter(core, .Game_Menu)
	case .Confirm_Quit:    enter(core, .Main_Menu)
	case .Options:         enter(core, core.options_return)
	case .Window_Size:     enter(core, .Options)
	}
}

picker_activate :: proc(core: ^Core, item: string) {
	switch item {
	case TEXT_CONTINUE_GAME:
		core.screen = .Neutral
	case TEXT_EMBARK:
		core.world = world_new()
		core.has_world = true
		core.screen = .Neutral
	case TEXT_OPTIONS:
		core.options_return = core.screen
		enter(core, .Options)
	case TEXT_ABOUT:
		core.screen = .About
	case TEXT_QUIT:
		enter(core, .Confirm_Quit)
	case TEXT_ABANDON:
		enter(core, .Confirm_Abandon)
	case TEXT_FULLSCREEN:
		core.fullscreen = !core.fullscreen
		core.config_pending = true
	case TEXT_WINDOW_SIZE:
		enter(core, .Window_Size)
	case TEXT_NO:
		enter(core, core.screen == .Confirm_Abandon ? .Game_Menu : .Main_Menu)
	case TEXT_YES:
		if core.screen == .Confirm_Abandon {
			core.world = world_new()
			core.has_world = false
			core_store(core, SAVE_KEY, EMPTY_SAVE) // not a removal: the empty marker says the player chose this
			enter(core, .Main_Menu)
		} else {
			core.quit = true
		}
	case:
		// a window size entry
		if core.screen == .Window_Size { core.window_scale = window_scales[core.menu_index]; core.config_pending = true }
	}
}

render_screen :: proc(core: ^Core, now_ms: f64) {
	switch core.screen {
	case .Splash: render_splash(&core.hues)
	case .About:  render_about(&core.hues)
	case .Neutral, .Shoppe:
		build_dialog_view(core, now_ms)
		if core.cursor[core.screen] >= core.view.choice_count { core.cursor[core.screen] = 0 } // as in the original's Render
		render_dialog(&core.hues, &core.view, core.cursor[core.screen])
	case .Main_Menu, .Game_Menu, .Confirm_Abandon, .Confirm_Quit, .Options, .Window_Size:
		render_menu(&core.hues, menu_header(core), menu_status(core), menu_items(core), core.menu_index)
	}
}
