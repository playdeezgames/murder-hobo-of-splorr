package game

// The text of the two dialogs, from NeutralDialog.vb and ShoppeDialog.vb.

import "core:fmt"

view_set_hint :: proc(v: ^View, text: string) {
	n := min(len(text), len(v.hint))
	copy(v.hint[:n], text)
	v.hint_length = u8(n)
}

view_add_line :: proc(v: ^View, mood: Mood, format: string, args: ..any) {
	if v.line_count >= MAX_VIEW_LINES { return }
	l := &v.lines[v.line_count]
	text := fmt.bprintf(l.text[:], format, ..args)
	l.length = u8(len(text))
	l.mood = mood
	v.line_count += 1
}

view_add_choice :: proc(v: ^View, text: string) {
	if v.choice_count >= MAX_VIEW_CHOICES { return }
	c := &v.choices[v.choice_count]
	n := min(len(text), len(c.text))
	copy(c.text[:n], text)
	c.length = u8(n)
	v.choice_count += 1
}

// The neutral dialog. `can_enter_game_menu` is always true here (the neutral dialog has no way back).
neutral_view :: proc(w: ^World, now_ms: f64, v: ^View) {
	v^ = {}
	view_set_hint(v, "(Escape -> Game Menu)")
	for i in 0 ..< w.message_count { view_add_line(v, w.messages[i].mood, "%s", message_text(&w.messages[i])) }
	view_add_line(v, .Normal, "Murder Skill: %d", w.skill)
	view_add_line(v, .Normal, "Murder Difficulty: %d", w.difficulty)
	view_add_line(v, .Normal, "Murder Counter: %d", w.murder_counter)
	view_add_line(v, .Normal, "Attempt Counter: %d", w.attempt_counter)
	if rate, ok := success_rate(w); ok {
		view_add_line(v, .Normal, "Success Rate: %d%%", rate)
	} else {
		view_add_line(v, .Normal, "Success Rate: ?")
	}
	view_add_line(v, .Normal, "Success Streak: %d", w.success_streak)
	view_add_line(v, .Normal, "Experience Points: %d", w.experience)
	if seconds, ok := auto_time_remaining(w, now_ms); ok {
		view_add_line(v, .Normal, "Auto-murder Time Remaining: %.2f", seconds)
	}
	view_add_choice(v, "Murder!")
	view_add_choice(v, "Shoppe")
}

shoppe_view :: proc(w: ^World, v: ^View) {
	v^ = {}
	view_set_hint(v, "(Escape -> Go Back)")
	view_add_line(v, .Heading, "Shoppe:")
	view_add_line(v, can_buy_skill(w) ? .Success : .Failure, "Skill Increase: %d XP", w.skill_cost)
	view_add_line(v, can_buy_difficulty(w) ? .Success : .Failure, "Difficulty Increase: %d XP", w.difficulty_cost)
	view_add_line(v, can_buy_auto(w) ? .Success : .Failure, "Auto-murder Increase: %d XP", w.auto_cost)
	view_add_line(v, .Normal, "Experience Points: %d", w.experience)
	view_add_choice(v, "Cancel")
	if can_buy_skill(w) { view_add_choice(v, "Skill Increase") }
	if can_buy_difficulty(w) { view_add_choice(v, "Difficulty Increase") }
	if can_buy_auto(w) { view_add_choice(v, "Auto-murder Increase") }
}
