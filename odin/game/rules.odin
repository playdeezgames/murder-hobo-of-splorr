package game

// The rules of Murder Hobo of SPLORR!!, ported from World.vb. Pure: the clock (`now_ms`) and the die (`roll`) are arguments.

import "core:fmt"

// Quirk decisions (docs/QUIRKS.md)
AUTO_CATCHUP_CAP :: 1000      // quirk 1: at most this many missed attempts run at once; the rest are forfeited
MIN_AUTO_INTERVAL_MS :: 1.0   // safety floor so repeated halving can never reach zero

sat_add :: proc(a, b: i64) -> i64 { return min(a + b, SATURATION) } // a, b are in [0, SATURATION], so the sum cannot overflow i64
sat_double :: proc(a: i64) -> i64 { return min(a * 2, SATURATION) }

// ---- messages ----------------------------------------------------------------------------------
messages_clear :: proc(w: ^World) { w.message_count = 0 }

message_add :: proc(w: ^World, mood: Mood, format: string, args: ..any) {
	if w.message_count >= MAX_MESSAGES { return }
	m := &w.messages[w.message_count]
	text := fmt.bprintf(m.text[:], format, ..args)
	m.length = u8(len(text))
	m.mood = mood
	w.message_count += 1
}

message_text :: proc(m: ^Message) -> string { return string(m.text[:m.length]) }

// ---- queries -----------------------------------------------------------------------------------
// 100 * murders \ attempts, truncated (quirk 11); not ok before the first attempt (the original shows "?")
success_rate :: proc(w: ^World) -> (rate: i64, ok: bool) {
	if w.attempt_counter == 0 { return 0, false }
	return 100 * w.murder_counter / w.attempt_counter, true // at most 100 * SATURATION, well inside i64
}

can_buy_skill :: proc(w: ^World) -> bool { return w.experience >= w.skill_cost }
can_buy_difficulty :: proc(w: ^World) -> bool { return w.experience >= w.difficulty_cost }
can_buy_auto :: proc(w: ^World) -> bool { return w.experience >= w.auto_cost }

// Seconds until the next auto-murder, or not ok when auto-murder has not been bought.
auto_time_remaining :: proc(w: ^World, now_ms: f64) -> (seconds: f64, ok: bool) {
	if !w.has_auto { return 0, false }
	return max(0, (w.next_auto_ms - now_ms) / 1000), true
}

// ---- murder ------------------------------------------------------------------------------------
// The die is uniform in [1, skill + difficulty]; the attempt succeeds when it is at most the skill.
roll_bound :: proc(w: ^World) -> i64 { return w.skill + w.difficulty }

attempt_murder_rolled :: proc(w: ^World, roll: i64) {
	w.attempt_counter = sat_add(w.attempt_counter, 1)
	messages_clear(w)
	if roll <= w.skill { murder_succeeded(w) } else { murder_failed(w) }
}

attempt_murder :: proc(w: ^World, r: ^Rng) { attempt_murder_rolled(w, rng_range(r, 1, roll_bound(w))) }

award_xp :: proc(w: ^World, award: i64) {
	message_add(w, .Success, "You get %d XP", award)
	w.experience = sat_add(w.experience, award)
}

murder_failed :: proc(w: ^World) {
	message_add(w, .Failure, "Failure!")
	w.success_streak = 0
	award_xp(w, sat_double(w.difficulty)) // failing pays double the difficulty (quirk 7, kept)
}

murder_succeeded :: proc(w: ^World) {
	award := w.difficulty
	w.murder_counter = sat_add(w.murder_counter, 1)
	message_add(w, .Success, "Success!")
	if w.success_streak > 0 {
		message_add(w, .Success, "Streak bonus %d XP!", w.success_streak)
		award = sat_add(award, w.success_streak)
	}
	w.success_streak = sat_add(w.success_streak, 1)
	if w.success_streak > w.record_streak {
		w.record_streak = w.success_streak
		message_add(w, .Success, "New Record Success Streak!") // before the XP line, as in the original (quirk 8)
	}
	award_xp(w, award)
}

// ---- the shoppe --------------------------------------------------------------------------------
buy_skill :: proc(w: ^World) {
	if !can_buy_skill(w) { return }
	w.experience -= w.skill_cost
	w.skill = sat_add(w.skill, 1)
	w.skill_cost = sat_double(w.skill_cost)
}

buy_difficulty :: proc(w: ^World) {
	if !can_buy_difficulty(w) { return }
	w.experience -= w.difficulty_cost
	w.difficulty = sat_add(w.difficulty, 1)
	w.difficulty_cost = sat_double(w.difficulty_cost)
}

// The first purchase starts the timer at now + interval; later ones halve the interval without touching the
// scheduled time (quirk 5, kept), so the new speed applies after the next firing.
buy_auto :: proc(w: ^World, now_ms: f64) {
	if !can_buy_auto(w) { return }
	w.experience -= w.auto_cost
	if w.has_auto {
		w.auto_interval_ms = max(w.auto_interval_ms / 2, MIN_AUTO_INTERVAL_MS)
	} else {
		w.has_auto = true
		w.next_auto_ms = now_ms + w.auto_interval_ms
		w.scheduled_interval_ms = w.auto_interval_ms
	}
	w.auto_cost = sat_double(w.auto_cost)
}

// ---- auto-murder -------------------------------------------------------------------------------
// Runs the attempts that are due. Returns how many ran.
//  * Clock clamp (quirk 12): a next time further ahead than the interval it was scheduled with is pulled back to
//    now + that interval, so a backwards clock jump (or a save from the future) stalls it for one interval at most.
//    Using the scheduled interval, not the current one, keeps quirk 5: halving the interval does not pull it forward.
//  * Catch-up (quirk 1): like the original, one attempt per missed interval, each step advancing from the previous
//    scheduled time, but at most AUTO_CATCHUP_CAP of them. Anything beyond is forfeited and the timer restarts at now + interval.
auto_tick :: proc(w: ^World, r: ^Rng, now_ms: f64) -> int {
	if !w.has_auto { return 0 }
	if w.next_auto_ms > now_ms + w.scheduled_interval_ms { w.next_auto_ms = now_ms + w.scheduled_interval_ms }
	ran := 0
	for now_ms >= w.next_auto_ms {
		if ran >= AUTO_CATCHUP_CAP {
			w.next_auto_ms = now_ms + w.auto_interval_ms
			w.scheduled_interval_ms = w.auto_interval_ms
			break
		}
		attempt_murder(w, r)
		ran += 1
		w.next_auto_ms += w.auto_interval_ms
		w.scheduled_interval_ms = w.auto_interval_ms
	}
	return ran
}
