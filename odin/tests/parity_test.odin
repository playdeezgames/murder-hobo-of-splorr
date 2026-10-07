#+build !js
package tests

// The scripted game that tools/wasm_parity.js plays through the built wasm; both write the same digest and tools/test.sh
// requires them to be equal (the wasm has a 32-bit int and its own allocator; the game must not notice).

import "core:fmt"
import "core:os"
import "core:strconv"
import "core:strings"
import "core:testing"
import "kmh:game"

parity_lcg: u32
parity_next :: proc() -> u32 {
	parity_lcg = parity_lcg * 1664525 + 1013904223
	return parity_lcg
}
fnv :: proc(h: u32, b: u8) -> u32 { return (h ~ u32(b)) * 16777619 }

@(test)
write_the_native_digest_for_the_wasm_parity_check :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	r := Rig{core = new(game.Core)}; defer rig_free(&r)
	services := mem_services()
	services.entropy = proc() -> u64 { return 7 } // the node script's stub gives 7 too
	game.core_init(r.core, services)
	parity_lcg = 12345
	frames := u32(2166136261)
	trace := make([dynamic]string); defer { for line in trace { if line != "-" { delete(line) } }; delete(trace) }
	now := f64(1_790_000_000_000)
	steps := 2000
	if text := os.get_env("PARITY_STEPS", context.temp_allocator); text != "" { if n, ok := strconv.parse_int(text); ok { steps = n } }
	for step in 0 ..< steps {
		sel := (parity_next() >> 16) % 10
		ev: game.Input_Event
		if sel == 9 {
			x := (parity_next() >> 16) % 384
			y := (parity_next() >> 16) % 216
			precise := (parity_next() >> 16) % 2 == 0
			ev = {kind = .Tap, x = i16(x), y = i16(y), precise = precise}
		} else {
			ev = {kind = .Command, command = game.Command(1 + (parity_next() >> 16) % 6)}
		}
		now += f64(1000 + (parity_next() >> 16) % 90000)
		if step % 50 == 49 { now += 6 * 3600 * 1000 }
		game.core_step(r.core, {dt = 0.016, now_ms = now, events = {ev}}, &r.out)
		free_all(context.temp_allocator)
		if !r.out.frame_changed { frames = fnv(frames, 0); append(&trace, "-"); continue } // an unchanged frame is hashed as one zero byte
		one := u32(2166136261)
		for pixel in r.out.frame {
			for shift in ([]u32{0, 8, 16, 24}) { frames = fnv(frames, u8(pixel >> shift)); one = fnv(one, u8(pixel >> shift)) }
		}
		append(&trace, fmt.aprintf("%d", one))
		if dump_step, ok := strconv.parse_int(os.get_env("DUMP_STEP", context.temp_allocator)); ok && dump_step == step {
			_ = os.write_entire_file("build/parity_native_frame.bin", (transmute([^]byte)&r.out.frame[0])[:game.FRAME_WIDTH * game.FRAME_HEIGHT * 4])
		}
	}
	save := u32(2166136261)
	saved_text, has_save := store[game.SAVE_KEY]
	for b in transmute([]u8)saved_text { save = fnv(save, b) }
	line := fmt.tprintf("frames=%d save=%d saved=%v\n", frames, save, has_save)
	testing.expect(t, os.write_entire_file("build/parity_native.txt", transmute([]byte)line) == nil)
	_ = os.write_entire_file("build/parity_native_trace.txt", transmute([]byte)fmt.tprintf("%s\n", strings.join(trace[:], "\n", context.temp_allocator)))
	_ = os.write_entire_file("build/parity_native_save.txt", transmute([]byte)saved_text)
}
