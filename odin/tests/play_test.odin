#+build !js
package tests

import "core:testing"
import "kmh:game"

tap :: proc(r: ^Rig, x, y: int, precise := true, now_ms := f64(0)) {
	game.core_step(r.core, {now_ms = now_ms, events = {{kind = .Tap, x = i16(x), y = i16(y), precise = precise}}}, &r.out)
	free_all(context.temp_allocator)
}

// ---- embark asks first when a game is saved --------------------------------------------------------------
@(test)
embark_confirms_only_when_there_is_a_game_to_lose :: proc(t: ^testing.T) {
	store_reset(); defer store_reset()
	r := boot(); defer rig_free(&r)
	embark(&r) // no save yet: straight in
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	play(&r, 0, .Confirm) // Murder!
	r2 := boot(); defer rig_free(&r2)
	play(&r2, 0, .Confirm, .Down, .Confirm) // splash, Main Menu: Continue Game, Embark! -> Embark!
	testing.expect_value(t, r2.core.screen, game.Screen.Confirm_Embark)
	testing.expect_value(t, game.menu_header(r2.core), "Embark anew? Your saved game will be lost.")
	play(&r2, 0, .Cancel)
	testing.expect_value(t, r2.core.screen, game.Screen.Main_Menu)
	testing.expect_value(t, r2.core.world.attempt_counter, 1) // untouched
	play(&r2, 0, .Down, .Confirm, .Confirm) // Embark!, No
	testing.expect_value(t, r2.core.screen, game.Screen.Main_Menu)
	play(&r2, 0, .Down, .Confirm, .Down, .Confirm) // Embark!, Yes
	testing.expect_value(t, r2.core.screen, game.Screen.Neutral)
	testing.expect_value(t, r2.core.world.attempt_counter, 0)
	testing.expect(t, len(store[game.SAVE_KEY]) > 0)
}

// ---- taps -----------------------------------------------------------------------------------------------------
@(test)
a_mouse_press_selects_and_confirms_a_choice :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	tap(&r, 50, 50) // splash
	testing.expect_value(t, r.core.screen, game.Screen.Main_Menu)
	tap(&r, 192, 108) // the selected item, Embark!
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	tap(&r, 10, 215) // Murder! (bottom left)
	testing.expect_value(t, r.core.world.attempt_counter, 1)
	tap(&r, 140, 215) // Shoppe, in the second column
	testing.expect_value(t, r.core.screen, game.Screen.Shoppe)
	testing.expect_value(t, r.core.cursor[.Shoppe], 0)
	tap(&r, 10, 3) // the hint line is Escape
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	tap(&r, 300, 215) // an empty third column does nothing
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	tap(&r, 10, 3)
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
	tap(&r, 192, 116) // the item below the bar: Abandon Game (web: Continue, Abandon)
	testing.expect_value(t, r.core.screen, game.Screen.Confirm_Abandon)
	tap(&r, 100, 214) // the status bar is Escape
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
	tap(&r, 192, 108) // Continue Game
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
}

@(test)
a_finger_selects_first_and_confirms_on_the_selected :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	embark(&r)
	tap(&r, 140, 215, false) // Shoppe: only selected
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	testing.expect_value(t, r.core.cursor[.Neutral], 1)
	tap(&r, 140, 215, false) // again: confirmed
	testing.expect_value(t, r.core.screen, game.Screen.Shoppe)
	tap(&r, 10, 3, false) // Escape works at once
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	tap(&r, 10, 3, false)
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
	tap(&r, 192, 116, false) // Abandon Game: selected
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
	testing.expect_value(t, r.core.menu_index, 1)
	tap(&r, 192, 108, false) // now the selected item sits in the middle bar
	testing.expect_value(t, r.core.screen, game.Screen.Confirm_Abandon)
}

@(test)
taps_outside_everything_do_nothing :: proc(t: ^testing.T) {
	r := rig_new(); defer rig_free(&r)
	embark(&r)
	for p in ([][2]int{{-5, 100}, {400, 100}, {100, -3}, {100, 400}, {383, 100}, {0, 100}}) { tap(&r, p.x, p.y) }
	testing.expect_value(t, r.core.screen, game.Screen.Neutral)
	testing.expect_value(t, r.core.world.attempt_counter, 0)
	play(&r, 0, .Cancel) // game menu
	for p in ([][2]int{{-5, 100}, {400, 100}, {100, 5}, {100, 300}, {100, 0}}) { tap(&r, p.x, p.y) }
	testing.expect_value(t, r.core.screen, game.Screen.Game_Menu)
}

// ---- a long random game ---------------------------------------------------------------------------------------
// Thousands of random inputs on a clock that mostly runs forward but sometimes jumps. Nothing may crash, every world must
// satisfy the invariants, the autosave must always load back to the same world, and the game must never get stuck.
random_event :: proc(r: ^game.Rng) -> game.Input_Event {
	switch game.rng_range(r, 0, 9) {
	case 0 ..= 6:
		return {kind = .Command, command = game.Command(game.rng_range(r, 1, 6))}
	case 7:
		return {kind = .Tap, x = i16(game.rng_range(r, -10, 400)), y = i16(game.rng_range(r, -10, 230)), precise = game.rng_range(r, 0, 1) == 0}
	case:
		return {kind = .Command, command = .Confirm}
	}
}

soak :: proc(t: ^testing.T, desktop: bool, seed: u64, steps: int) {
	store_reset(); defer store_reset()
	r := boot(desktop); defer rig_free(&r)
	dice: game.Rng
	game.rng_seed(&dice, seed)
	now := f64(1_790_000_000_000)
	seen: [game.Screen]bool
	for step in 0 ..< steps {
		switch game.rng_range(&dice, 0, 19) {
		case 0:  now += f64(game.rng_range(&dice, 0, 7 * 24 * 3600 * 1000)) // a long absence
		case 1:  now -= f64(game.rng_range(&dice, 0, 3 * 3600 * 1000))      // the clock jumps back
		case:    now += f64(game.rng_range(&dice, 0, 90_000))
		}
		now = max(now, 0)
		// buy into the economy now and then so every screen and purchase is reached
		if game.rng_range(&dice, 0, 40) == 0 && r.core.has_world { r.core.world.experience = game.sat_add(r.core.world.experience, game.rng_range(&dice, 0, 100_000)) }
		ev := random_event(&dice)
		game.core_step(r.core, {dt = 0.016, now_ms = now, events = {ev}}, &r.out)
		free_all(context.temp_allocator)
		seen[r.core.screen] = true
		if !game.world_valid(&r.core.world) { testing.expectf(t, false, "step %d: the world broke its rules", step); return }
		testing.expect(t, r.out.frame != nil)
		if r.core.has_world && r.core.save_failed { testing.expect(t, false, "saving failed"); return }
		if r.core.quit { r.core.quit = false; game.enter(r.core, .Main_Menu) } // keep playing after a Quit
		if step % 97 == 0 && r.core.has_world {
			// the stored save is this very world
			loaded: game.World
			testing.expect_value(t, game.world_from_json(store[game.SAVE_KEY], &loaded), game.Load_Result.Ok)
			testing.expect_value(t, game.world_to_json(&loaded), game.world_to_json(&r.core.world))
			free_all(context.temp_allocator)
		}
	}
	testing.expect(t, seen[.Neutral] && seen[.Shoppe] && seen[.Game_Menu] && seen[.Main_Menu], "the soak reached the main screens")
}

@(test)
soak_on_the_web_menus :: proc(t: ^testing.T) { soak(t, false, 1, 8000) }

@(test)
soak_on_the_desktop_menus :: proc(t: ^testing.T) { soak(t, true, 2, 8000) }

@(test)
soak_with_other_seeds :: proc(t: ^testing.T) {
	for seed in 10 ..< 14 { soak(t, seed % 2 == 0, u64(seed), 2000) }
}
