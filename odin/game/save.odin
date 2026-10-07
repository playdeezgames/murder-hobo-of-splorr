package game

// Saving: one JSON text under one key, the same on both platforms. The loader validates every field, so a damaged or
// hand-edited file is rejected whole and leaves the game untouched. Abandoning writes an "empty" marker rather than removing
// the key (a removed key would be indistinguishable from a first run).

import "core:encoding/json"
import "core:fmt"
import "core:math"
import "core:strings"

SAVE_KEY :: "mhos:save"
CONFIG_KEY :: "mhos:config"
SAVE_VERSION :: 1
SAVE_MAX_BYTES :: 16 * 1024 // a real save is under 1 KB

Load_Result :: enum u8 { Ok, Empty, Invalid }

EMPTY_SAVE :: `{"version":1,"empty":true}`

MOOD_NAMES := [Mood]string{.Normal = "Normal", .Success = "Success", .Failure = "Failure", .Heading = "Heading"}

// ---- writing --------------------------------------------------------------------------------------------
// Hand-written so the output is the same on every target and a re-save is byte for byte identical.
world_to_json :: proc(w: ^World, allocator := context.temp_allocator) -> string {
	b := strings.builder_make(allocator)
	fmt.sbprintf(&b, `{{"version":%d,"empty":false,"world":{{`, SAVE_VERSION)
	fmt.sbprintf(&b, `"murder_counter":%d,"attempt_counter":%d,"experience":%d,"skill":%d,"difficulty":%d,`,
		w.murder_counter, w.attempt_counter, w.experience, w.skill, w.difficulty)
	fmt.sbprintf(&b, `"skill_cost":%d,"difficulty_cost":%d,"success_streak":%d,"record_streak":%d,`,
		w.skill_cost, w.difficulty_cost, w.success_streak, w.record_streak)
	fmt.sbprintf(&b, `"has_auto":%v,"next_auto_ms":%.6f,"scheduled_interval_ms":%.12f,"auto_interval_ms":%.12f,"auto_cost":%d,"messages":[`,
		w.has_auto, w.next_auto_ms, w.scheduled_interval_ms, w.auto_interval_ms, w.auto_cost)
	for i in 0 ..< w.message_count {
		m := &w.messages[i]
		if i > 0 { strings.write_byte(&b, ',') }
		fmt.sbprintf(&b, `{{"text":"%s","mood":"%s"}}`, message_text(m), MOOD_NAMES[m.mood]) // texts are printable ASCII without quotes (checked on load)
	}
	strings.write_string(&b, "]}}")
	return strings.to_string(b)
}

// ---- reading --------------------------------------------------------------------------------------------
// i64 field in [lo, hi]; JSON integers only (a float such as 1.5 or 1e3 is rejected).
read_int :: proc(o: json.Object, key: string, lo, hi: i64) -> (value: i64, ok: bool) {
	v, found := o[key]
	if !found { return 0, false }
	i, is_int := v.(json.Integer)
	if !is_int || i < lo || i > hi { return 0, false }
	return i, true
}

read_number :: proc(o: json.Object, key: string, lo, hi: f64) -> (value: f64, ok: bool) {
	v, found := o[key]
	if !found { return 0, false }
	f: f64
	#partial switch n in v {
	case json.Float:   f = n
	case json.Integer: f = f64(n)
	case: return 0, false
	}
	if math.is_nan(f) || math.is_inf(f) || f < lo || f > hi { return 0, false }
	return f, true
}

read_bool :: proc(o: json.Object, key: string) -> (value: bool, ok: bool) {
	v, found := o[key]
	if !found { return false, false }
	b, is_bool := v.(json.Boolean)
	return bool(b), is_bool
}

mood_from_name :: proc(name: string) -> (mood: Mood, ok: bool) {
	for m in Mood { if MOOD_NAMES[m] == name { return m, true } }
	return .Normal, false
}

// Parses and validates `text`. On Ok `world` is filled in; on Empty or Invalid it is left as it was.
world_from_json :: proc(text: string, world: ^World) -> Load_Result {
	if len(text) > SAVE_MAX_BYTES { return .Invalid }
	root_value, err := json.parse_string(text, .JSON, true, context.temp_allocator)
	if err != .None { return .Invalid }
	root, is_obj := root_value.(json.Object)
	if !is_obj { return .Invalid }
	if _, ok := read_int(root, "version", SAVE_VERSION, SAVE_VERSION); !ok { return .Invalid }
	empty, empty_ok := read_bool(root, "empty")
	if !empty_ok { return .Invalid }
	if empty { return .Empty }
	wv, has_world := root["world"]
	if !has_world { return .Invalid }
	o, world_is_obj := wv.(json.Object)
	if !world_is_obj { return .Invalid }

	w := world_new()
	ok := true
	read :: proc(o: json.Object, key: string, lo, hi: i64, ok: ^bool) -> i64 {
		v, good := read_int(o, key, lo, hi)
		if !good { ok^ = false }
		return v
	}
	w.murder_counter = read(o, "murder_counter", 0, SATURATION, &ok)
	w.attempt_counter = read(o, "attempt_counter", 0, SATURATION, &ok)
	w.experience = read(o, "experience", 0, SATURATION, &ok)
	w.skill = read(o, "skill", 1, SATURATION, &ok)
	w.difficulty = read(o, "difficulty", 1, SATURATION, &ok)
	w.skill_cost = read(o, "skill_cost", 1, SATURATION, &ok)
	w.difficulty_cost = read(o, "difficulty_cost", 1, SATURATION, &ok)
	w.success_streak = read(o, "success_streak", 0, SATURATION, &ok)
	w.record_streak = read(o, "record_streak", 0, SATURATION, &ok)
	w.auto_cost = read(o, "auto_cost", 1, SATURATION, &ok)
	if !ok { return .Invalid }
	if w.murder_counter > w.attempt_counter || w.success_streak > w.record_streak || w.record_streak > w.murder_counter { return .Invalid }

	has_auto, has_auto_ok := read_bool(o, "has_auto")
	if !has_auto_ok { return .Invalid }
	w.has_auto = has_auto
	next_ms, next_ok := read_number(o, "next_auto_ms", 0, 1e15)
	sched, sched_ok := read_number(o, "scheduled_interval_ms", MIN_AUTO_INTERVAL_MS, START_AUTO_INTERVAL_MS)
	interval, interval_ok := read_number(o, "auto_interval_ms", MIN_AUTO_INTERVAL_MS, START_AUTO_INTERVAL_MS)
	if !next_ok || !sched_ok || !interval_ok { return .Invalid }
	w.next_auto_ms, w.scheduled_interval_ms, w.auto_interval_ms = next_ms, sched, interval
	if !has_auto && (w.auto_interval_ms != START_AUTO_INTERVAL_MS) { return .Invalid } // the interval only changes after the first purchase

	mv, has_messages := o["messages"]
	if !has_messages { return .Invalid }
	messages, is_array := mv.(json.Array)
	if !is_array || len(messages) > MAX_MESSAGES { return .Invalid }
	for item in messages {
		mo, item_is_obj := item.(json.Object)
		if !item_is_obj { return .Invalid }
		text_value, has_text := mo["text"]
		mood_value, has_mood := mo["mood"]
		if !has_text || !has_mood { return .Invalid }
		message, text_is_string := text_value.(json.String)
		mood_name, mood_is_string := mood_value.(json.String)
		if !text_is_string || !mood_is_string || len(message) > MESSAGE_CAP { return .Invalid }
		for i in 0 ..< len(message) { if message[i] < 32 || message[i] > 126 || message[i] == '"' || message[i] == '\\' { return .Invalid } }
		mood, mood_ok := mood_from_name(string(mood_name))
		if !mood_ok { return .Invalid }
		message_add(&w, mood, "%s", string(message))
	}
	world^ = w
	return .Ok
}

// ---- the desktop options (window scale and full screen) -----------------------------------------------------
config_to_json :: proc(scale: int, fullscreen: bool, allocator := context.temp_allocator) -> string {
	return fmt.aprintf(`{{"version":1,"window_scale":%d,"fullscreen":%v}}`, scale, fullscreen, allocator = allocator)
}

config_from_json :: proc(text: string) -> (scale: int, fullscreen: bool, ok: bool) {
	if len(text) > SAVE_MAX_BYTES { return 0, false, false }
	value, err := json.parse_string(text, .JSON, true, context.temp_allocator)
	if err != .None { return 0, false, false }
	o, is_obj := value.(json.Object)
	if !is_obj { return 0, false, false }
	if _, v_ok := read_int(o, "version", 1, 1); !v_ok { return 0, false, false }
	s, s_ok := read_int(o, "window_scale", 1, 100)
	f, f_ok := read_bool(o, "fullscreen")
	if !s_ok || !f_ok { return 0, false, false }
	for allowed in window_scales { if int(s) == allowed { return int(s), f, true } }
	return 0, false, false
}

// ---- the core's use of it ------------------------------------------------------------------------------
// Loads the save (and, on the desktop, the options) at start-up. A save that is missing, empty or invalid leaves no world.
core_load :: proc(core: ^Core) {
	defer free_all(context.temp_allocator)
	if core.services.storage_get == nil { return }
	if text, found := core.services.storage_get(SAVE_KEY, context.temp_allocator); found {
		switch world_from_json(text, &core.world) {
		case .Ok:      core.has_world = true
		case .Empty:   core.has_world = false
		case .Invalid: core.has_world = false; core_log(core, "the saved game could not be read and was ignored")
		}
	}
	if core.services.desktop {
		if text, found := core.services.storage_get(CONFIG_KEY, context.temp_allocator); found {
			if scale, fullscreen, ok := config_from_json(text); ok { core.window_scale, core.fullscreen = scale, fullscreen }
		}
	}
}

core_log :: proc(core: ^Core, message: string) { if core.services.log != nil { core.services.log(message) } }

core_store :: proc(core: ^Core, key, value: string) {
	if core.services.storage_set == nil { return }
	if !core.services.storage_set(key, value) && !core.save_failed {
		core.save_failed = true // say so once, not on every frame
		core_log(core, "the game could not be saved")
	}
}

// Autosave: the world in play, after anything that may have changed it. (Abandoning stores the empty marker; see screens.odin.)
core_save :: proc(core: ^Core) {
	if core.has_world { core_store(core, SAVE_KEY, world_to_json(&core.world)) }
}

core_save_config :: proc(core: ^Core) {
	if core.services.desktop { core_store(core, CONFIG_KEY, config_to_json(core.window_scale, core.fullscreen)) }
}
