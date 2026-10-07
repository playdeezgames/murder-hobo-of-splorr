# Port plan: Murder Hobo of SPLORR!! to Odin / js_wasm32

Status: **plan decided, nothing ported yet.** Recipe: the vault's `Tech/Odin wasm recipe.md` (Shark Attackers / Feretory variants). Rule-level oddities are in `QUIRKS.md`; decide those before or after the port, as with the earlier ports.

## What the game is (small)

Roughly 12 rules. The other ~5,000 lines are the AOS menu/UI framework (splash, main menu, options, save/load slots, window size, volume, about), a pixel font renderer, and a layered dialog framework. None of that needs porting as code.

Rules, all from `MHOS.Persistence.Implementation/World.vb`:

| Thing | Behaviour |
| --- | --- |
| Start | skill 1, difficulty 1, XP 0, counters 0, streak 0, record 0, no auto-murder |
| Murder roll | `rand(1..skill+difficulty) <= skill` (chance skill/(skill+difficulty)) |
| Success | murders+1, "Success!", streak bonus (if streak>0) added to award, streak+1, "New Record Success Streak!" if beaten; award = difficulty + old streak |
| Failure | "Failure!", streak 0, award = difficulty x 2 |
| Messages | cleared at the start of every attempt; XP line `You get N XP` last |
| Shoppe | Skill +1 (cost 50), Difficulty +1 (cost 25), Auto-murder (cost 1000); every cost doubles on purchase; only affordable choices are listed, Cancel always first |
| Auto-murder | first buy starts a timer (interval 60 s); later buys halve the interval; each elapsed interval runs one murder attempt |
| Success rate | `100 * murders \ attempts` (integer), `?` before the first attempt |
| Neutral screen | messages, then Skill, Difficulty, Murder Counter, Attempt Counter, Success Rate, Success Streak, XP, and `Auto-murder Time Remaining: N.NN` once bought |
| Moods | Normal light gray, Success green, Failure red, Heading orange |

Choices are `Murder!` and `Shoppe` (neutral); `Cancel`, `Skill Increase`, `Difficulty Increase`, `Auto-murder Increase` (shoppe). Texts are in `MHOS.Business/Dialogs` and `Choices`.

## Layout (decided; Kordanor's Cabal pattern)

```
odin/game/             package game: portable core, no platform imports
odin/platform/web/     js_wasm32 main + page/ (index.html, platform.js)
odin/platform/native/  SDL2 window; blits the same frame
odin/tests/            portable tests; native `odin test`
tools/                 build.sh [web|native|all], test.sh, serve.sh, ship.sh
docs/                  PORT_PLAN.md, QUIRKS.md
src/                   original VB.NET, deleted in its own commit after shipping
```

- **Core:** `core_step(core, input, out)` once per presented frame; `Services` struct passed in once (storage get/set/remove, entropy, log, clock). The core renders a **384x216 RGBA frame** with the original bitmap font (`Content/Fonts/*.json`, 8x8 `UIFont`) and the 3-column selector. Input is six commands (Up, Down, Left, Right, Confirm, Cancel) plus taps/clicks in cell coordinates.
- **Time and dice are arguments** (`now_ms: f64`, roll), never read inside, so tests force outcomes.
- **Screens** are a state machine over a `Screen` enum: Splash, Main Menu, Neutral, Shoppe, Game Menu, Confirm Abandon, About, Options.
- **Save** (`save.odin`): JSON, versioned, every field range-checked, enums validated, abandon = `empty` marker. Web key `mhos:save` (localStorage); native a file in the user data dir. Same format on both.
- **Web page:** own `requestAnimationFrame` loop calling exported `proc "c"` functions, ImageData blit with CSS stretch, `e.key` before `e.code`.
- **Native:** SDL2 (`vendor:sdl2`), texture blit, integer scaling, resizable and fullscreen; no audio so no mixer.
- `#+build js` / `#+build !js` split, `odin test -define:ODIN_TEST_THREADS=1`, wasm built too whenever tests run (32-bit `int`).

## Phases

1. Scaffold: core + both platforms drawing a test frame; build.sh, test.sh.
2. Rules + tests (forced rolls, streaks/record, cost doubling with i64 saturation, auto-murder catch-up cap, clock clamp).
3. Framebuffer renderer: font loader, selector, mood colours; reference-frame tests against the original where practical.
4. Screens/state machine + menus; tests walk it via commands.
5. Save/load, resume, abandon (both platforms).
6. Native polish (scale, fullscreen), web page, soak tests.
7. Ship only when told (channels chosen at ship time). Commit first.

## Mapping from the original

| Original | Port |
| --- | --- |
| Dialog + Choice classes, `IWorld`/`IWorldModel` layers | one `Screen` enum, `choose(i)` |
| `WorldData` JSON file save + Quick save/load slots | one localStorage save, autosave after each event |
| `DateTimeOffset NextAutoMurder`, `Double AutoMurderInterval` | `next_auto_ms: f64` (epoch ms), `interval_ms: f64`; pass as `f64` to JS (`int` is 32 bits on wasm, `i64` imports need BigInt) |
| `RNG.FromRange` (System.Random) | `rand.int_max`; seed with `rand.reset` in `main`; roll passed in for tests |
| Main Menu: Embark / Scum Load / Load / Options / About / Quit | see decisions; Quit and file Load/Save have no web meaning |
| Options: window size, fullscreen, sfx/mux volume | window size and fullscreen are moot in a page; keep a music volume at most |
| 384x216 pixel buffer, bitmap font, 3-column gamepad-style menu | see decision 1 |
| Keys via `keys.json` (Space/Enter=A, Esc=B, arrows/WASD/ZQSD/numpad) | page keys: read `e.key` first, then `e.code` (remote desktop gotcha); buttons are also clickable |

## Things the port must handle

- **Time:** the game needs a live clock (countdown shown to 0.01 s; auto-murder fires by itself). A pure event-driven DOM (as in Check Yer Butthole) is not enough: use `step(dt)` or a JS timer that calls an export, and update only the countdown line, not the whole view, if buttons should keep focus.
- **Catch-up after absence:** the original loops once per missed interval (see QUIRKS 1). A tab closed for a day at a 60 s interval is 1,440 murders; at a tiny interval it is millions and **freezes the tab**. Cap the loop (every retry/catch-up loop is capped in the vault's Odin games).
- **32-bit `int`:** costs double on every purchase (1000 x 2^n). VB would throw on overflow; Odin on wasm wraps silently. Use `i64` (range-check on load, `f64` across the JS boundary) or cap purchases.
- **Messages in saves:** the original saves the current message list. Keep (resume looks the same), validate the mood enum on load (unknown enum names unmarshal silently).
- **Namespaced storage key** (`mhos:save`); itch.io games share one origin.
- **Audio (dropped, see decisions):** only `MainTheme.ogg` (1.9 MB) ever plays; `CombatTheme`, `we_will_prevail` (victory) and the DeathTheme are never started, and none of the 13 sfx wavs are triggered by the game (only a volume test beep, `SfxVolumeTest` -> `PlayerHit.wav`). Browsers block autoplay until a gesture, so music would start on the first click. Asset licences/credits: `aboot.txt` credits the Urizen tileset and zooperdan's victory theme (copied from the older game); the MainTheme/CombatTheme provenance is **not recorded** anywhere I could find. Confirm before shipping them.
- **Fonts:** `Content/Fonts/Cy*.json` and the 8x8 `UIFont` are bitmap fonts from `FontMaker`; DOM does not need them.
- **Title:** `Murder Hobo of SPLORR!!`, tagline `Itinerance and slaughter!`.

## Decisions (the user's, all made)

| # | Question | Answer |
| --- | --- | --- |
| 1 | Presentation | **Shared framebuffer**: one core renders 384x216 with the original bitmap font; web and native only blit |
| 2 | Fidelity | **Decide quirks before porting** (done, see QUIRKS.md) |
| 3 | Menus | **Title, Main Menu, Game Menu**; drop file Load/Save and Scum slots; web drops Quit, window size, fullscreen; native keeps Quit, window size, fullscreen |
| 4 | Saves | **Autosave, one slot**; Continue/Embark on Main Menu; Abandon writes an empty marker; no export/import, no VB save import |
| 5 | Audio | **None**; no music, no sfx, no volume screens, no decoder dependency |
| 6 | Layout | `odin/game` + `platform/web` + `platform/native` (**SDL2**); `src/` deleted in its own commit afterwards |
| 7 | Shipping | **Decide at ship time**; build both now. Old script's targets: `thegrumpygamedev/murder-hobo-of-splorr:windows|linux|mac`; `README.md` and `shippit.sh` get replaced |
| 8 | Embark! with a saved game | **Confirm first** ("Embark anew? Your saved game will be lost.", No first); with no save it starts at once |

Consequence of dropping audio: `Content/Audio`, `mux.json`, `sfx.json`, Options volume items go away; Options is only window size/fullscreen on native and is absent on web (so on web the Main Menu has no Options entry).

## QA hooks (phase 6)

- **Web:** `?seed=N` fixes the random numbers (up to 9 digits); `?log=1` prints every input sent to the game.
- **Native:** `--seed N`, `--data DIR` (save directory instead of the per-user one), `--script "confirm,down,tap:100:50,..."` (one input per frame, then quit) and `--dump FILE` (the last frame as a PPM). Example: `build/native/murder-hobo --data /tmp/x --script "confirm,confirm,confirm" --dump /tmp/x/frame.ppm`.
- **Pointer:** a mouse press selects and confirms; a finger selects first and confirms on the selected item. The hint line of a dialog and the status bar of a menu act as Escape.
- `tools/test.sh` runs the generated-data check, the native suite built with `-o:speed` (about 70 tests including three long random "soak" games and a save-fuzzer), and a full build of both platforms with the vet flags. `tools/gen_reference.sh` re-records the VB reference frames (needs dotnet).
