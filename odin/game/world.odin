package game

// The world: everything the original kept in WorldData, as one plain struct.
// All counts are i64 and saturate at SATURATION (decision: never wrap; int is 32 bits on wasm, and costs double on every purchase).

SATURATION :: i64(9_000_000_000_000_000) // below 2^53, so it also crosses to JS as an exact f64

Mood :: enum u8 { Normal, Success, Failure, Heading }

MESSAGE_CAP :: 48
MAX_MESSAGES :: 8

Message :: struct {
	text:   [MESSAGE_CAP]u8,
	length: u8,
	mood:   Mood,
}

// Original defaults (WorldData.vb); the interval is stored in milliseconds.
START_SKILL_COST :: 50
START_DIFFICULTY_COST :: 25
START_AUTO_COST :: 1000
START_AUTO_INTERVAL_MS :: 60_000.0

World :: struct {
	murder_counter:     i64,
	attempt_counter:    i64,
	experience:         i64,
	skill:              i64,
	difficulty:         i64,
	skill_cost:         i64,
	difficulty_cost:    i64,
	success_streak:     i64,
	record_streak:      i64,
	has_auto:           bool, // WorldData.NextAutoMurder.HasValue
	next_auto_ms:       f64,  // epoch milliseconds
	scheduled_interval_ms: f64, // the interval in force when next_auto_ms was set (see auto_tick)
	auto_interval_ms:   f64,
	auto_cost:          i64,
	messages:           [MAX_MESSAGES]Message,
	message_count:      int,
}

world_new :: proc() -> World {
	return World{
		skill = 1, difficulty = 1,
		skill_cost = START_SKILL_COST, difficulty_cost = START_DIFFICULTY_COST,
		auto_cost = START_AUTO_COST, auto_interval_ms = START_AUTO_INTERVAL_MS,
	}
}
