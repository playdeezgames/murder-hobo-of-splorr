#+build !js
package main

// Native platform: an SDL2 window that shows the core's frame. core:os and SDL2 are allowed here only.
import "core:fmt"
import "core:os"
import "core:strconv"
import "core:strings"
import "core:time"
import SDL "vendor:sdl2"
import "kmh:game"

wall_clock_ms :: proc() -> f64 { return f64(time.now()._nsec) / 1e6 }

fixed_seed := -1 // set by --seed N

native_services :: proc() -> game.Services {
	return {
		storage_get = storage_get, storage_set = storage_set, storage_remove = storage_remove,
		entropy = proc() -> u64 {
			if fixed_seed >= 0 { return u64(fixed_seed) }
			return u64(SDL.GetPerformanceCounter()) * 2862933555777941757 + u64(SDL.GetTicks())
		},
		log = proc(message: string) { fmt.println(message) },
		desktop = true,
	}
}

command_of_key :: proc(sym: SDL.Keycode) -> game.Command {
	#partial switch sym {
	case .UP, .W, .KP_8:               return .Up
	case .DOWN, .S, .KP_2:             return .Down
	case .LEFT, .A, .KP_4:             return .Left
	case .RIGHT, .D, .KP_6:            return .Right
	case .RETURN, .KP_ENTER, .SPACE, .KP_5: return .Confirm
	case .ESCAPE, .BACKSPACE, .KP_0:   return .Cancel
	}
	return .None
}

main :: proc() {
	if SDL.Init({.VIDEO, .EVENTS}) != 0 { fmt.eprintln(SDL.GetError()); os.exit(1) }
	defer SDL.Quit()
	// QA options: --data DIR (save directory), --seed N (fixed random numbers), --script "confirm,down,tap:100:50" (one input per
	// frame, then quit) and --dump FILE (write the last frame as a PPM before quitting). Used to check the real window.
	data_override, script_text, dump_path := "", "", ""
	for arg, i in os.args {
		if i + 1 >= len(os.args) { continue }
		switch arg {
		case "--data":   data_override = os.args[i + 1]
		case "--script": script_text = os.args[i + 1]
		case "--dump":   dump_path = os.args[i + 1]
		case "--seed":   if n, ok := strconv.parse_int(os.args[i + 1]); ok && n >= 0 && n < 1_000_000_000 { fixed_seed = n }
		}
	}
	init_storage(data_override)
	script: [dynamic]string
	if script_text != "" { for token in strings.split(script_text, ",", context.temp_allocator) { append(&script, token) } }
	script_pos := 0
	scripted := script_text != ""
	frames_after_script := 0
	core := new(game.Core)
	game.core_init(core, native_services())
	window := SDL.CreateWindow("Murder Hobo of SPLORR!!", SDL.WINDOWPOS_CENTERED, SDL.WINDOWPOS_CENTERED,
		i32(game.FRAME_WIDTH * core.window_scale), i32(game.FRAME_HEIGHT * core.window_scale),
		core.fullscreen ? {.SHOWN, .RESIZABLE, .FULLSCREEN, ._INTERNAL_FULLSCREEN_DESKTOP} : {.SHOWN, .RESIZABLE})
	renderer := SDL.CreateRenderer(window, -1, {.ACCELERATED, .PRESENTVSYNC})
	SDL.RenderSetLogicalSize(renderer, game.FRAME_WIDTH, game.FRAME_HEIGHT)
	SDL.RenderSetIntegerScale(renderer, true)
	texture := SDL.CreateTexture(renderer, .ABGR8888, .STREAMING, game.FRAME_WIDTH, game.FRAME_HEIGHT) // bytes R,G,B,A in memory
	defer { SDL.DestroyTexture(texture); SDL.DestroyRenderer(renderer); SDL.DestroyWindow(window) }

	out: game.Step_Output
	events: [dynamic]game.Input_Event
	prev := SDL.GetTicks()
	applied_scale := core.window_scale
	applied_fullscreen := core.fullscreen
	quit := false
	for !quit {
		clear(&events)
		e: SDL.Event
		for SDL.PollEvent(&e) {
			#partial switch e.type {
			case .QUIT: quit = true
			case .KEYDOWN:
				if e.key.repeat != 0 { continue }
				if cmd := command_of_key(e.key.keysym.sym); cmd != .None { append(&events, game.Input_Event{kind = .Command, command = cmd}) }
			case .MOUSEBUTTONDOWN:
				// SDL's logical-size event watch has already mapped mouse positions to frame pixels
				append(&events, game.Input_Event{kind = .Tap, x = i16(e.button.x), y = i16(e.button.y), precise = true})
			}
		}
		if scripted {
			if script_pos < len(script) {
				if event, ok := scripted_event(script[script_pos]); ok { append(&events, event) } else { fmt.eprintln("bad script token:", script[script_pos]) }
				script_pos += 1
			} else {
				frames_after_script += 1
				if frames_after_script >= 3 { // one more frame to draw, one to settle
					if dump_path != "" { dump_frame(dump_path, out.frame) }
					quit = true
				}
			}
		}
		now := SDL.GetTicks()
		game.core_step(core, {dt = f64(now - prev) / 1000, now_ms = wall_clock_ms(), events = events[:]}, &out)
		prev = now
		if out.frame_changed { SDL.UpdateTexture(texture, nil, out.frame, game.FRAME_WIDTH * 4) }
		SDL.RenderClear(renderer)
		SDL.RenderCopy(renderer, texture, nil, nil)
		SDL.RenderPresent(renderer)
		if out.window_scale != applied_scale {
			applied_scale = out.window_scale
			SDL.SetWindowSize(window, i32(game.FRAME_WIDTH * applied_scale), i32(game.FRAME_HEIGHT * applied_scale))
		}
		if out.fullscreen != applied_fullscreen {
			applied_fullscreen = out.fullscreen
			SDL.SetWindowFullscreen(window, applied_fullscreen ? {.FULLSCREEN, ._INTERNAL_FULLSCREEN_DESKTOP} : {})
		}
		if out.quit_requested { quit = true }
		free_all(context.temp_allocator)
	}
}

// "confirm", "cancel", "up", "down", "left", "right", or "tap:X:Y" (a mouse press at frame pixel X, Y).
scripted_event :: proc(token: string) -> (event: game.Input_Event, ok: bool) {
	switch token {
	case "confirm": return {kind = .Command, command = .Confirm}, true
	case "cancel":  return {kind = .Command, command = .Cancel}, true
	case "up":      return {kind = .Command, command = .Up}, true
	case "down":    return {kind = .Command, command = .Down}, true
	case "left":    return {kind = .Command, command = .Left}, true
	case "right":   return {kind = .Command, command = .Right}, true
	}
	if strings.has_prefix(token, "tap:") {
		parts := strings.split(token, ":", context.temp_allocator)
		if len(parts) == 3 {
			x, x_ok := strconv.parse_int(parts[1])
			y, y_ok := strconv.parse_int(parts[2])
			if x_ok && y_ok { return {kind = .Tap, x = i16(x), y = i16(y), precise = true}, true }
		}
	}
	return {}, false
}

dump_frame :: proc(path: string, frame: ^[game.FRAME_WIDTH * game.FRAME_HEIGHT]u32) {
	if frame == nil { return }
	b := strings.builder_make(context.temp_allocator)
	fmt.sbprintf(&b, "P6\n%d %d\n255\n", game.FRAME_WIDTH, game.FRAME_HEIGHT)
	for pixel in frame { strings.write_byte(&b, u8(pixel)); strings.write_byte(&b, u8(pixel >> 8)); strings.write_byte(&b, u8(pixel >> 16)) }
	_ = os.write_entire_file(path, transmute([]byte)strings.to_string(b))
}
