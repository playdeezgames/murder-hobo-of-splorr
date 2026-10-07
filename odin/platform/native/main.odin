#+build !js
package main

// Native platform: an SDL2 window that shows the core's frame. core:os and SDL2 are allowed here only.
import "core:fmt"
import "core:os"
import "core:time"
import SDL "vendor:sdl2"
import "kmh:game"

wall_clock_ms :: proc() -> f64 { return f64(time.now()._nsec) / 1e6 }

native_services :: proc() -> game.Services {
	return {
		storage_get = storage_get, storage_set = storage_set, storage_remove = storage_remove,
		entropy = proc() -> u64 { return u64(SDL.GetPerformanceCounter()) * 2862933555777941757 + u64(SDL.GetTicks()) },
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
	data_override := ""
	for arg, i in os.args { if arg == "--data" && i + 1 < len(os.args) { data_override = os.args[i + 1] } }
	init_storage(data_override)
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
				append(&events, game.Input_Event{kind = .Tap, x = i16(e.button.x), y = i16(e.button.y)})
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
