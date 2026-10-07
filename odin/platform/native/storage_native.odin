#+build !js
package main

import "core:os"
import "core:strings"
import SDL "vendor:sdl2"

// One file per key in the per-user data directory (SDL_GetPrefPath: ~/.local/share/... on Linux, AppData on Windows).
// `--data DIR` (see main.odin) overrides it, for tests and for trying the game without touching a real save.
save_dir: string

init_storage :: proc(override: string) {
	if override != "" {
		save_dir = strings.concatenate({strings.trim_right(override, "/"), "/"})
		os.make_directory(save_dir[:len(save_dir) - 1])
		return
	}
	path := SDL.GetPrefPath("TheGrumpyGameDev", "murder-hobo-of-splorr")
	if path != nil { save_dir = strings.clone_from_cstring(path); SDL.free(rawptr(path)) } else { save_dir = "saves/"; os.make_directory("saves") }
}

key_path :: proc(key: string) -> string {
	safe, _ := strings.replace_all(key, ":", "_", context.temp_allocator)
	return strings.concatenate({save_dir, safe, ".json"}, context.temp_allocator)
}

storage_set :: proc(key, value: string) -> bool { return os.write_entire_file(key_path(key), transmute([]byte)value) == nil }
storage_remove :: proc(key: string) { os.remove(key_path(key)) }
storage_get :: proc(key: string, allocator := context.allocator) -> (value: string, ok: bool) {
	data, read_err := os.read_entire_file(key_path(key), allocator)
	if read_err != nil { return "", false }
	return string(data), true
}
