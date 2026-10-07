#+build !js
package tests

import "core:strings"
import "core:testing"
import "kmh:game"

// An in-memory storage service (tests run on one thread).
store: map[string]string
store_fail: bool

mem_get :: proc(key: string, allocator := context.allocator) -> (string, bool) {
	v, ok := store[key]
	if !ok { return "", false }
	return strings.clone(v, allocator), true
}
mem_set :: proc(key, value: string) -> bool {
	if store_fail { return false }
	if old, had := store[key]; had { delete(old); store[key] = strings.clone(value); return true }
	store[strings.clone(key)] = strings.clone(value)
	return true
}
mem_remove :: proc(key: string) { if old, had := store[key]; had { delete(old); delete_key(&store, key) } } // (the key string is not freed: only a fake)
log_count: int
mem_log :: proc(message: string) { log_count += 1 }

store_reset :: proc() {
	for k, v in store { delete(v); delete(k) }
	delete(store) // not clear: the map must not keep an allocator from an earlier test
	store = {}
	store_fail = false
	log_count = 0
}

mem_services :: proc(desktop := false) -> game.Services {
	return {storage_get = mem_get, storage_set = mem_set, storage_remove = mem_remove, entropy = proc() -> u64 { return 11 }, log = mem_log, desktop = desktop}
}

boot :: proc(desktop := false) -> Rig {
	r := Rig{core = new(game.Core)}
	game.core_init(r.core, mem_services(desktop))
	game.core_step(r.core, {now_ms = 0}, &r.out)
	return r
}

sample_world :: proc() -> game.World {
	w := game.world_new()
	w.experience, w.skill, w.difficulty, w.skill_cost, w.difficulty_cost = 4321, 5, 7, 400, 800
	w.murder_counter, w.attempt_counter, w.success_streak, w.record_streak = 30, 50, 3, 9
	w.has_auto, w.next_auto_ms, w.scheduled_interval_ms, w.auto_interval_ms, w.auto_cost = true, 1_790_000_123_456.5, 60_000, 937.5, 4000
	game.message_add(&w, .Success, "Success!")
	game.message_add(&w, .Failure, "Failure!")
	game.message_add(&w, .Heading, "You get %d XP", 12)
	return w
}

@(test)
a_world_survives_a_round_trip_byte_for_byte :: proc(t: ^testing.T) {
	w := sample_world()
	text := game.world_to_json(&w)
	back: game.World
	testing.expect_value(t, game.world_from_json(text, &back), game.Load_Result.Ok)
	testing.expect_value(t, back.experience, 4321)
	testing.expect_value(t, back.record_streak, 9)
	testing.expect_value(t, back.next_auto_ms, 1_790_000_123_456.5)
	testing.expect_value(t, back.auto_interval_ms, 937.5)
	testing.expect_value(t, back.message_count, 3)
	testing.expect_value(t, game.message_text(&back.messages[2]), "You get 12 XP")
	testing.expect_value(t, back.messages[1].mood, game.Mood.Failure)
	testing.expect_value(t, game.world_to_json(&back), text)
	free_all(context.temp_allocator)
}

@(test)
a_fresh_world_and_the_extremes_round_trip :: proc(t: ^testing.T) {
	w := game.world_new()
	text := game.world_to_json(&w)
	back: game.World
	testing.expect_value(t, game.world_from_json(text, &back), game.Load_Result.Ok)
	testing.expect_value(t, game.world_to_json(&back), text)
	big := game.world_new()
	big.experience, big.skill, big.difficulty, big.skill_cost = game.SATURATION, game.SATURATION, game.SATURATION, game.SATURATION
	big.attempt_counter, big.murder_counter, big.success_streak, big.record_streak = game.SATURATION, game.SATURATION, game.SATURATION, game.SATURATION
	big.difficulty_cost, big.auto_cost = game.SATURATION, game.SATURATION
	text = game.world_to_json(&big)
	testing.expect_value(t, game.world_from_json(text, &back), game.Load_Result.Ok)
	testing.expect_value(t, back.experience, game.SATURATION)
	testing.expect_value(t, game.world_to_json(&back), text)
	free_all(context.temp_allocator)
}

@(test)
the_empty_marker_and_damaged_files_are_recognised :: proc(t: ^testing.T) {
	w := sample_world()
	before := w
	testing.expect_value(t, game.world_from_json(game.EMPTY_SAVE, &w), game.Load_Result.Empty)
	bad := []string{
		"", "{", "[]", "null", "42", `{"version":1}`, `{"version":2,"empty":true}`, `{"version":1,"empty":"yes"}`,
		`{"version":1,"empty":false}`, `{"version":1,"empty":false,"world":[]}`, `{"version":1,"empty":false,"world":{}}`,
	}
	for text in bad { testing.expect_value(t, game.world_from_json(text, &w), game.Load_Result.Invalid) }
	testing.expect_value(t, w, before) // nothing was touched
	free_all(context.temp_allocator)
}

// Each mutation breaks one rule of a valid save; every one must be rejected.
@(test)
every_rule_of_a_valid_save_is_enforced :: proc(t: ^testing.T) {
	w := sample_world()
	good := game.world_to_json(&w)
	Mutation :: struct { from, to: string }
	mutations := []Mutation{
		{`"skill":5`, `"skill":0`}, {`"difficulty":7`, `"difficulty":0`}, {`"skill":5`, `"skill":-1`}, {`"experience":4321`, `"experience":-1`},
		{`"experience":4321`, `"experience":9000000000000001`}, {`"skill_cost":400`, `"skill_cost":0`}, {`"auto_cost":4000`, `"auto_cost":0`},
		{`"skill":5`, `"skill":5.5`}, {`"skill":5`, `"skill":"5"`}, {`"skill":5`, `"skill":1e3`}, {`"skill":5`, `"skill":null`},
		{`"murder_counter":30`, `"murder_counter":51`}, {`"success_streak":3`, `"success_streak":10`}, {`"record_streak":9`, `"record_streak":31`},
		{`"has_auto":true`, `"has_auto":1`}, {`"has_auto":true`, `"has_auto":"true"`},
		{`"next_auto_ms":1790000123456.500000`, `"next_auto_ms":-5.0`}, {`"next_auto_ms":1790000123456.500000`, `"next_auto_ms":2e15`},
		{`"scheduled_interval_ms":60000.000000000000`, `"scheduled_interval_ms":0.5`}, {`"scheduled_interval_ms":60000.000000000000`, `"scheduled_interval_ms":60001.0`},
		{`"auto_interval_ms":937.500000000000`, `"auto_interval_ms":0.0`}, {`"auto_interval_ms":937.500000000000`, `"auto_interval_ms":99999.0`},
		{`"mood":"Success"`, `"mood":"Cheerful"`}, {`"mood":"Success"`, `"mood":3`}, {`"text":"Success!"`, `"text":5`},
		{`"text":"Success!"`, `"text":"Succ\"ess"`}, {`"text":"Success!"`, `"text":"Succ\u0001ess"`}, {`"text":"Success!"`, `"text":"Succéss"`},
		{`"version":1`, `"version":1,"version2":1,`}, {`"messages":[`, `"messages":{"a":`}, {`"empty":false`, `"empty":0`},
		{`"text":"Success!"`, `"text":"` + "0123456789012345678901234567890123456789012345678901234567890" + `"`},
	}
	for m in mutations {
		bad, _ := strings.replace(good, m.from, m.to, 1, context.temp_allocator)
		if bad == good { testing.expectf(t, false, "the mutation %q does not apply", m.from); continue }
		out: game.World
		testing.expectf(t, game.world_from_json(bad, &out) == .Invalid, "accepted a save with %q -> %q", m.from, m.to)
	}
	// more than eight messages
	many := strings.concatenate({good[:len(good) - 3], `,{"text":"a","mood":"Normal"}`, `,{"text":"a","mood":"Normal"}`, `,{"text":"a","mood":"Normal"}`, `,{"text":"a","mood":"Normal"}`, `,{"text":"a","mood":"Normal"}`, `,{"text":"a","mood":"Normal"}`, "]}}"}, context.temp_allocator)
	out: game.World
	testing.expect_value(t, game.world_from_json(many, &out), game.Load_Result.Invalid)
	// a file that is simply too big
	huge := strings.repeat(" ", game.SAVE_MAX_BYTES + 1, context.temp_allocator)
	testing.expect_value(t, game.world_from_json(huge, &out), game.Load_Result.Invalid)
	free_all(context.temp_allocator)
}

@(test)
interval_may_only_differ_from_the_start_after_buying_auto :: proc(t: ^testing.T) {
	w := game.world_new()
	w.auto_interval_ms = 30_000
	text := game.world_to_json(&w) // has_auto is false
	out: game.World
	testing.expect_value(t, game.world_from_json(text, &out), game.Load_Result.Invalid)
	free_all(context.temp_allocator)
}

@(test)
damaged_saves_never_crash_and_are_either_rejected_or_valid :: proc(t: ^testing.T) {
	w := sample_world()
	good := game.world_to_json(&w)
	r: game.Rng
	game.rng_seed(&r, 99)
	buf := make([]u8, len(good) + 8, context.temp_allocator)
	accepted := 0
	noise := "0123456789-.eE\"{}[],:tfn"
	for _ in 0 ..< 3000 {
		n := len(good)
		copy(buf, good)
		edits := int(game.rng_range(&r, 1, 4))
		for _ in 0 ..< edits {
			pos := int(game.rng_range(&r, 0, i64(n - 1)))
			switch game.rng_range(&r, 0, 3) {
			case 0: buf[pos] = u8(game.rng_range(&r, 0, 255))
			case 1: buf[pos] = noise[game.rng_range(&r, 0, i64(len(noise) - 1))]
			case 2: n = pos + 1 // truncate
			case 3: if pos + 1 < n { copy(buf[pos:], buf[pos + 1:n]); n -= 1 }
			}
			if n <= 0 { n = 1 }
		}
		out: game.World
		if game.world_from_json(string(buf[:n]), &out) == .Ok {
			accepted += 1
			// whatever was accepted must satisfy the rules the game relies on
			testing.expect(t, out.skill >= 1 && out.difficulty >= 1 && out.skill_cost >= 1 && out.auto_cost >= 1)
			testing.expect(t, out.murder_counter <= out.attempt_counter && out.success_streak <= out.record_streak)
			testing.expect(t, out.message_count <= game.MAX_MESSAGES)
			testing.expect(t, out.auto_interval_ms >= game.MIN_AUTO_INTERVAL_MS && out.scheduled_interval_ms >= game.MIN_AUTO_INTERVAL_MS)
			testing.expect(t, out.next_auto_ms >= 0)
			// and a re-save of it loads again
			again: game.World
			testing.expect_value(t, game.world_from_json(game.world_to_json(&out), &again), game.Load_Result.Ok)
		}
		free_all(context.temp_allocator)
	}
	testing.expect(t, accepted < 3000)
}

// ---- the core and its storage ---------------------------------------------------------------------------
@(test)
playing_autosaves_and_the_next_boot_can_continue :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	r := boot(); defer rig_free(&r)
	_, saved := store[game.SAVE_KEY]
	testing.expect(t, !saved, "nothing is saved before there is a world")
	embark(&r)
	testing.expect(t, strings.contains(store[game.SAVE_KEY], `"empty":false`))
	r.core.world.skill = 3 // the choice of Murder! below runs on this world
	play(&r, 0, .Confirm) // Murder!
	testing.expect_value(t, r.core.world.attempt_counter, 1)
	attempts := r.core.world.attempt_counter
	xp := r.core.world.experience
	// "close the tab": a new core over the same storage
	r2 := boot(); defer rig_free(&r2)
	testing.expect_value(t, r2.core.screen, game.Screen.Splash)
	testing.expect_value(t, r2.core.has_world, true)
	play(&r2, 0, .Confirm)
	testing.expect_value(t, game.menu_items(r2.core)[0], "Continue Game")
	testing.expect_value(t, game.menu_items(r2.core)[1], "Embark!")
	play(&r2, 0, .Confirm)
	testing.expect_value(t, r2.core.screen, game.Screen.Neutral)
	testing.expect_value(t, r2.core.world.attempt_counter, attempts)
	testing.expect_value(t, r2.core.world.experience, xp)
	testing.expect_value(t, r2.core.world.skill, 3)
}

@(test)
abandoning_writes_the_empty_marker_and_stops_the_resume :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	r := boot(); defer rig_free(&r)
	embark(&r)
	play(&r, 0, .Cancel, .Up, .Confirm, .Down, .Confirm) // Game Menu > Abandon Game > Yes
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
	testing.expect_value(t, store[game.SAVE_KEY], game.EMPTY_SAVE)
	play(&r, 0, .Confirm) // Embark!: a new game is saved again
	testing.expect(t, strings.contains(store[game.SAVE_KEY], `"empty":false`))
	play(&r, 0, .Cancel, .Up, .Confirm, .Down, .Confirm)
	r2 := boot(); defer rig_free(&r2)
	testing.expect_value(t, r2.core.has_world, false)
	play(&r2, 0, .Confirm)
	testing.expect_value(t, game.menu_items(r2.core)[0], "Embark!")
}

@(test)
an_unreadable_save_is_ignored_and_left_in_place :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	mem_set(game.SAVE_KEY, `{"version":1,"empty":false,"world":{"skill":0}}`)
	r := boot(); defer rig_free(&r)
	testing.expect_value(t, r.core.has_world, false)
	testing.expect(t, log_count >= 1, "the player is told in the log")
	play(&r, 0, .Confirm, .Cancel, .Left, .Up) // wandering the menus does not overwrite it
	testing.expect_value(t, store[game.SAVE_KEY], `{"version":1,"empty":false,"world":{"skill":0}}`)
}

@(test)
failing_storage_does_not_stop_the_game_and_is_reported_once :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	store_fail = true
	r := boot(); defer rig_free(&r)
	embark(&r)
	play(&r, 0, .Confirm, .Confirm, .Confirm)
	testing.expect_value(t, r.core.world.attempt_counter, 3)
	testing.expect_value(t, log_count, 1)
}

@(test)
returning_after_a_long_absence_catches_up_within_the_cap :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	w := game.world_new()
	w.has_auto, w.next_auto_ms, w.scheduled_interval_ms, w.auto_interval_ms = true, 1000, 1000, 1000
	mem_set(game.SAVE_KEY, game.world_to_json(&w))
	r := Rig{core = new(game.Core)}; defer rig_free(&r)
	game.core_init(r.core, mem_services())
	day := f64(24 * 3600 * 1000)
	play(&r, day, .Confirm, .Confirm) // splash, Continue Game; the neutral screen then ticks
	game.core_step(r.core, {now_ms = day}, &r.out)
	testing.expect_value(t, r.core.world.attempt_counter, game.AUTO_CATCHUP_CAP)
	testing.expect_value(t, r.core.world.next_auto_ms, day + 1000)
}

@(test)
desktop_options_are_stored_and_restored :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	r := boot(true); defer rig_free(&r)
	play(&r, 0, .Confirm, .Down, .Confirm, .Confirm) // Options, Toggle Full Screen
	testing.expect_value(t, store[game.CONFIG_KEY], `{"version":1,"window_scale":3,"fullscreen":true}`)
	play(&r, 0, .Down, .Confirm, .Down, .Down, .Confirm) // Window Size..., then the third size (5)
	testing.expect_value(t, store[game.CONFIG_KEY], `{"version":1,"window_scale":5,"fullscreen":true}`)
	r2 := boot(true); defer rig_free(&r2)
	testing.expect_value(t, r2.core.window_scale, 5)
	testing.expect_value(t, r2.core.fullscreen, true)
	// the web page never reads or writes them
	r3 := boot(false); defer rig_free(&r3)
	testing.expect_value(t, r3.core.window_scale, game.DEFAULT_WINDOW_SCALE)
}

@(test)
bad_options_are_ignored :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	texts := []string{`{"version":1,"window_scale":7,"fullscreen":false}`, `{"version":1,"window_scale":3}`, `{"version":1,"window_scale":"3","fullscreen":false}`, `[`, ``}
	for text in texts {
		mem_set(game.CONFIG_KEY, text)
		r := boot(true); defer rig_free(&r)
		testing.expect_value(t, r.core.window_scale, game.DEFAULT_WINDOW_SCALE)
		testing.expect_value(t, r.core.fullscreen, false)
	}
}
