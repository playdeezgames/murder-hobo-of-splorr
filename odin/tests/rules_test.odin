#+build !js
package tests

import "core:testing"
import "kmh:game"

// Test helpers ---------------------------------------------------------------------------------------
texts :: proc(w: ^game.World, allocator := context.temp_allocator) -> []string {
	out := make([]string, w.message_count, allocator)
	for i in 0 ..< w.message_count { out[i] = game.message_text(&w.messages[i]) }
	return out
}

expect_messages :: proc(t: ^testing.T, w: ^game.World, expected: ..string, loc := #caller_location) {
	got := texts(w)
	testing.expect_value(t, len(got), len(expected), loc)
	for i in 0 ..< min(len(got), len(expected)) { testing.expect_value(t, got[i], expected[i], loc) }
}

// Murder ----------------------------------------------------------------------------------------------
@(test)
new_world_has_the_original_defaults :: proc(t: ^testing.T) {
	w := game.world_new()
	testing.expect_value(t, w.skill, 1)
	testing.expect_value(t, w.difficulty, 1)
	testing.expect_value(t, w.skill_cost, 50)
	testing.expect_value(t, w.difficulty_cost, 25)
	testing.expect_value(t, w.auto_cost, 1000)
	testing.expect_value(t, w.auto_interval_ms, 60_000.0)
	testing.expect(t, !w.has_auto)
	_, ok := game.success_rate(&w)
	testing.expect(t, !ok)
}

@(test)
roll_at_most_skill_succeeds :: proc(t: ^testing.T) {
	w := game.world_new()
	w.skill, w.difficulty = 3, 2
	testing.expect_value(t, game.roll_bound(&w), 5)
	game.attempt_murder_rolled(&w, 3)
	testing.expect_value(t, w.murder_counter, 1)
	game.attempt_murder_rolled(&w, 4)
	testing.expect_value(t, w.murder_counter, 1)
	testing.expect_value(t, w.attempt_counter, 2)
}

@(test)
success_message_order_streak_and_record :: proc(t: ^testing.T) {
	w := game.world_new()
	game.attempt_murder_rolled(&w, 1) // first success: no bonus, streak 0 -> 1, new record
	expect_messages(t, &w, "Success!", "New Record Success Streak!", "You get 1 XP")
	testing.expect_value(t, w.experience, 1)
	game.attempt_murder_rolled(&w, 1) // streak 1: bonus 1, award 1 + 1
	expect_messages(t, &w, "Success!", "Streak bonus 1 XP!", "New Record Success Streak!", "You get 2 XP")
	testing.expect_value(t, w.success_streak, 2)
	testing.expect_value(t, w.record_streak, 2)
	testing.expect_value(t, w.experience, 3)
}

@(test)
failure_pays_double_difficulty_and_breaks_the_streak :: proc(t: ^testing.T) {
	w := game.world_new()
	w.difficulty = 4
	w.success_streak, w.record_streak = 5, 5
	game.attempt_murder_rolled(&w, 99) // above the skill: failure
	expect_messages(t, &w, "Failure!", "You get 8 XP")
	testing.expect_value(t, w.success_streak, 0)
	testing.expect_value(t, w.record_streak, 5)
	testing.expect_value(t, w.experience, 8)
	testing.expect_value(t, w.messages[0].mood, game.Mood.Failure)
	testing.expect_value(t, w.messages[1].mood, game.Mood.Success)
}

@(test)
no_record_message_when_the_record_is_not_beaten :: proc(t: ^testing.T) {
	w := game.world_new()
	w.success_streak, w.record_streak = 1, 10
	game.attempt_murder_rolled(&w, 1)
	expect_messages(t, &w, "Success!", "Streak bonus 1 XP!", "You get 2 XP")
	testing.expect_value(t, w.record_streak, 10)
}

@(test)
every_attempt_clears_the_previous_messages :: proc(t: ^testing.T) {
	w := game.world_new()
	for _ in 0 ..< 5 { game.attempt_murder_rolled(&w, 1) }
	testing.expect(t, w.message_count <= 4)
	game.attempt_murder_rolled(&w, 2)
	expect_messages(t, &w, "Failure!", "You get 2 XP")
}

@(test)
success_rate_truncates :: proc(t: ^testing.T) {
	w := game.world_new()
	w.attempt_counter, w.murder_counter = 3, 2
	rate, ok := game.success_rate(&w)
	testing.expect(t, ok)
	testing.expect_value(t, rate, 66)
	w.attempt_counter, w.murder_counter = 4, 3
	rate, _ = game.success_rate(&w)
	testing.expect_value(t, rate, 75)
}

@(test)
random_rolls_are_in_range_and_follow_the_odds :: proc(t: ^testing.T) {
	w := game.world_new()
	w.skill, w.difficulty = 1, 3
	r: game.Rng
	game.rng_seed(&r, 12345)
	N :: 20_000
	wins := 0
	for _ in 0 ..< N {
		roll := game.rng_range(&r, 1, game.roll_bound(&w))
		testing.expect(t, roll >= 1 && roll <= 4)
		if roll <= w.skill { wins += 1 }
	}
	testing.expect(t, wins > N * 22 / 100 && wins < N * 28 / 100, "about one in four")
	// the same seed gives the same sequence
	a, b: game.Rng
	game.rng_seed(&a, 7); game.rng_seed(&b, 7)
	for _ in 0 ..< 100 { testing.expect_value(t, game.rng_next(&a), game.rng_next(&b)) }
	// huge bounds do not overflow
	for _ in 0 ..< 100 { x := game.rng_range(&r, 1, 2 * game.SATURATION); testing.expect(t, x >= 1 && x <= 2 * game.SATURATION) }
}

// Shoppe ----------------------------------------------------------------------------------------------
@(test)
purchases_cost_xp_and_double :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = 24
	testing.expect(t, !game.can_buy_difficulty(&w))
	game.buy_difficulty(&w)
	testing.expect_value(t, w.difficulty, 1) // refused
	w.experience = 100
	game.buy_difficulty(&w)
	testing.expect_value(t, w.difficulty, 2)
	testing.expect_value(t, w.experience, 75)
	testing.expect_value(t, w.difficulty_cost, 50)
	game.buy_skill(&w)
	testing.expect_value(t, w.skill, 2)
	testing.expect_value(t, w.experience, 25)
	testing.expect_value(t, w.skill_cost, 100)
	game.buy_skill(&w)
	testing.expect_value(t, w.skill, 2) // 25 < 100: refused
}

@(test)
costs_and_counts_saturate_instead_of_wrapping :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = game.SATURATION
	w.skill_cost = game.SATURATION
	w.skill = game.SATURATION
	game.buy_skill(&w)
	testing.expect_value(t, w.skill, game.SATURATION)
	testing.expect_value(t, w.skill_cost, game.SATURATION)
	testing.expect_value(t, w.experience, 0)
	// doubling from 50 never wraps, however long it runs
	c := i64(50)
	for _ in 0 ..< 200 { c = game.sat_double(c); testing.expect(t, c > 0 && c <= game.SATURATION) }
	testing.expect_value(t, c, game.SATURATION)
	// XP and attempts saturate too
	w2 := game.world_new()
	w2.experience, w2.attempt_counter, w2.difficulty = game.SATURATION - 1, game.SATURATION, 5
	game.attempt_murder_rolled(&w2, 99)
	testing.expect_value(t, w2.experience, game.SATURATION)
	testing.expect_value(t, w2.attempt_counter, game.SATURATION)
	rate, ok := game.success_rate(&w2)
	testing.expect(t, ok && rate == 0)
}

// Auto-murder -----------------------------------------------------------------------------------------
@(test)
first_auto_purchase_starts_the_timer_later_ones_halve_the_interval :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = 10_000
	game.buy_auto(&w, 1_000_000)
	testing.expect(t, w.has_auto)
	testing.expect_value(t, w.next_auto_ms, 1_060_000.0)
	testing.expect_value(t, w.auto_interval_ms, 60_000.0)
	testing.expect_value(t, w.auto_cost, 2000)
	testing.expect_value(t, w.experience, 9000)
	secs, ok := game.auto_time_remaining(&w, 1_030_000)
	testing.expect(t, ok)
	testing.expect_value(t, secs, 30.0)
	game.buy_auto(&w, 1_030_000)
	testing.expect_value(t, w.auto_interval_ms, 30_000.0)
	testing.expect_value(t, w.next_auto_ms, 1_060_000.0) // quirk 5: not rescheduled
	testing.expect_value(t, w.auto_cost, 4000)
}

@(test)
no_time_remaining_before_auto_is_bought_and_none_negative :: proc(t: ^testing.T) {
	w := game.world_new()
	_, ok := game.auto_time_remaining(&w, 0)
	testing.expect(t, !ok)
	w.has_auto, w.next_auto_ms = true, 100
	secs, _ := game.auto_time_remaining(&w, 5000)
	testing.expect_value(t, secs, 0.0)
}

@(test)
auto_murder_fires_once_per_interval_and_catches_up :: proc(t: ^testing.T) {
	w := game.world_new()
	r: game.Rng
	game.rng_seed(&r, 1)
	w.has_auto, w.next_auto_ms, w.auto_interval_ms, w.scheduled_interval_ms = true, 1000, 1000, 1000
	testing.expect_value(t, game.auto_tick(&w, &r, 999), 0)
	testing.expect_value(t, game.auto_tick(&w, &r, 1000), 1)
	testing.expect_value(t, w.next_auto_ms, 2000.0)
	testing.expect_value(t, w.attempt_counter, 1)
	testing.expect_value(t, game.auto_tick(&w, &r, 5500), 4) // due at 2000, 3000, 4000, 5000
	testing.expect_value(t, w.attempt_counter, 5)
	testing.expect_value(t, w.next_auto_ms, 6000.0)
}

@(test)
auto_catchup_is_capped_and_the_rest_forfeited :: proc(t: ^testing.T) {
	w := game.world_new()
	r: game.Rng
	game.rng_seed(&r, 2)
	w.has_auto, w.next_auto_ms, w.auto_interval_ms, w.scheduled_interval_ms = true, 0, 1000, 1000
	day := f64(24 * 60 * 60 * 1000)
	testing.expect_value(t, game.auto_tick(&w, &r, day), game.AUTO_CATCHUP_CAP)
	testing.expect_value(t, w.attempt_counter, game.AUTO_CATCHUP_CAP)
	testing.expect_value(t, w.next_auto_ms, day + 1000)
	testing.expect_value(t, game.auto_tick(&w, &r, day + 500), 0)
	// a tiny interval over a long absence is bounded too
	w2 := game.world_new()
	w2.has_auto, w2.next_auto_ms, w2.auto_interval_ms, w2.scheduled_interval_ms = true, 0, game.MIN_AUTO_INTERVAL_MS, game.MIN_AUTO_INTERVAL_MS
	testing.expect_value(t, game.auto_tick(&w2, &r, 1e13), game.AUTO_CATCHUP_CAP)
}

@(test)
backwards_clock_stalls_for_at_most_one_interval :: proc(t: ^testing.T) {
	w := game.world_new()
	r: game.Rng
	game.rng_seed(&r, 3)
	w.has_auto, w.next_auto_ms, w.auto_interval_ms, w.scheduled_interval_ms = true, 10_000_000, 60_000, 60_000 // saved far in the future
	testing.expect_value(t, game.auto_tick(&w, &r, 1_000_000), 0)
	testing.expect_value(t, w.next_auto_ms, 1_060_000.0)
	testing.expect_value(t, game.auto_tick(&w, &r, 1_060_000), 1)
}

@(test)
clock_clamp_does_not_undo_the_unrescheduled_halving :: proc(t: ^testing.T) {
	w := game.world_new()
	r: game.Rng
	game.rng_seed(&r, 4)
	w.experience = 100_000
	game.buy_auto(&w, 0)             // next at 60 s, interval 60 s
	game.buy_auto(&w, 1000)          // interval 30 s, next still 60 s (quirk 5)
	game.buy_auto(&w, 2000)          // interval 15 s, next still 60 s
	testing.expect_value(t, game.auto_tick(&w, &r, 3000), 0)
	testing.expect_value(t, w.next_auto_ms, 60_000.0) // the clamp used the 60 s it was scheduled with
	testing.expect_value(t, game.auto_tick(&w, &r, 60_000), 1)
	testing.expect_value(t, w.next_auto_ms, 75_000.0) // the new 15 s interval now applies
}

@(test)
auto_interval_never_reaches_zero :: proc(t: ^testing.T) {
	w := game.world_new()
	w.experience = game.SATURATION
	w.auto_cost = 1
	w.has_auto = true
	for _ in 0 ..< 200 { w.experience = game.SATURATION; w.auto_cost = 1; game.buy_auto(&w, 0) }
	testing.expect_value(t, w.auto_interval_ms, game.MIN_AUTO_INTERVAL_MS)
}

@(test)
core_owns_a_seeded_world :: proc(t: ^testing.T) {
	core := new(game.Core); defer free(core)
	game.core_init(core, game.Services{entropy = proc() -> u64 { return 99 }})
	testing.expect_value(t, core.world.skill, 1)
	testing.expect_value(t, core.rng.state, 99)
}
