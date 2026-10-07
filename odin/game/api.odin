package game

// The platform interface. Everything the portable core exchanges with a platform.
// The core never imports a platform package; platforms never reach into core state.
//
//   platform -> core:  Input_Event list per frame, dt, the wall clock, and Services supplied once at init
//   core -> platform:  Step_Output per frame (pixels, quit flag) and synchronous calls through Services

import "base:runtime"

// ---- fixed geometry: the original's 384 by 216 view, shown with square pixels ------------------------
FRAME_WIDTH :: 384
FRAME_HEIGHT :: 216

// ---- input -------------------------------------------------------------------------------------
// The six commands of the original (AOS.UI Command: A, B, Up, Down, Left, Right).
Command :: enum u8 { None, Up, Down, Left, Right, Confirm, Cancel }

Input_Kind :: enum u8 {
	None,
	Command, // a key, or a gamepad button, already mapped
	Tap,     // a mouse press or touch: x, y are FRAME pixel coordinates (may be outside the frame)
}

Input_Event :: struct {
	kind:    Input_Kind,
	command: Command,
	x, y:    i16,
}

// ---- output ------------------------------------------------------------------------------------
Step_Output :: struct {
	frame:          ^[FRAME_WIDTH * FRAME_HEIGHT]u32, // bytes in memory order R,G,B,A (little-endian u32 0xAABBGGRR)
	frame_changed:  bool,
	quit_requested: bool,
	fullscreen:     bool, // desktop only: what the Options menu asks for (the web page ignores both)
	window_scale:   int,  // desktop only: the window is the view times this
}

Step_Input :: struct {
	dt:     f64, // seconds since the previous step (presentation only)
	now_ms: f64, // wall clock, milliseconds since the Unix epoch; f64 because int is 32 bits on wasm
	events: []Input_Event,
}

// ---- services (platform functions the core calls synchronously) ------------------------------------
// Supplied once to core_init, so the core has no platform imports and tests can pass fakes.
Services :: struct {
	storage_get:    proc(key: string, allocator: runtime.Allocator) -> (value: string, ok: bool),
	storage_set:    proc(key, value: string) -> bool, // false on quota or when storage is unavailable
	storage_remove: proc(key: string),
	entropy:        proc() -> u64, // for seeding; not reproducible
	log:            proc(message: string),
	desktop:        bool, // true on the native client: Quit, full screen and window size make sense there
}

// The core's entry points are core_init and core_step (core.odin).
// core_step is called once per presented frame; it must not block, must not keep pointers into `input`
// after returning, and fills `out` completely every call.
