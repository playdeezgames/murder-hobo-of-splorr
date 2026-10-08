# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

"Murder Hobo of SPLORR!!": a small idle clicker ("metaphor") by TheGrumpyGameDev. Attempt murders, earn XP, buy skill, difficulty and auto-murder in the Shoppe. **There is no ending, and failing paying double XP is deliberate; do not "fix" either.** The game was rebuilt in Odin in October 2026 (a browser build and a native SDL2 client over one core). The original VB.NET / MonoGame game is still in `src/` as the reference the port was checked against, and is to be deleted after shipping.

Background lives in the Obsidian vault at `/home/yermom/git/bok-of-splorr/splorr/` (`Games/Murder Hobo of SPLORR!!.md` is this game's page; `Home.md`, `Gotchas.md`, `Tech/Odin wasm recipe.md` and `Tech/Shipping to itch.io.md` are the useful notes). `docs/PORT_PLAN.md` records the port's decisions and `docs/QUIRKS.md` the decided quirks of the original; read them before changing behaviour that looks odd.

## Standing rules

- **Never run `tools/ship.sh --push`, `shippit.sh --push`, `butler push` or `git push` unless the user says so in chat.** `tools/ship.sh` without `--push` only tests, builds and zips.
- Commit only when asked. End commit messages with the attribution line the harness gives.
- Do not describe deliberate design as a bug (see above and the vault page's "Rules for future sessions").
- Delete `src/` (with `tools/vb-oracle` and `tools/gen_reference.sh`) only after shipping, in its own commit, and only when told.

## Commands

```bash
tools/test.sh                # font-data check, native tests (-o:speed), builds both platforms with vet flags, wasm parity under node
tools/build.sh [web|native]  # output in build/ (git-ignored); ODIN_FLAGS="-o:size" for the shipping build
tools/serve.sh               # serves build/web on http://localhost:8080 (PORT=... to change; 8080 may be taken, use another)
tools/ship.sh [--push]       # tests, size-optimized web build, zip to build/murder-hobo-html5.zip; uploads only with --push
tools/gen_reference.sh       # re-records docs/reference/vb/*.txt from the VB game (needs the dotnet SDK and src/)
python3 tools/gen/gen_font.py  # regenerates odin/game/font_data.odin and tools/vb-oracle/m5x7.json (needs Pillow)
```

`tools/test.sh` takes about a minute; a tool call over 120 s is moved to the background, so run it with `run_in_background` when you also build. Run a single test with `odin test odin/tests -o:speed -collection:kmh=odin -out:build/t -define:ODIN_TEST_THREADS=1 -define:ODIN_TEST_NAMES=tests.<name>` (the thread count must be 1: tests share globals). Odin is `dev-2026-07-nightly` at `/home/yermom/ODIN/odin`; SDL2 must be installed for the native client.

QA hooks: web `?seed=N` (fixed dice) and `?log=1` (every input to the console); native `--seed N`, `--data DIR` (use a scratch save directory), `--script "confirm,down,tap:100:50,..."` (one input per frame, then quit) and `--dump FILE` (last frame as PPM). The browser pane caches `platform.wasm` hard and does not run frames while hidden; see the vault's Gotchas.

## Architecture (`odin/`)

- **`odin/game/`** is the portable core (package `game`, imported as `kmh:game` via `-collection:kmh=odin`; no platform imports). `api.odin` is the whole platform interface: `core_step(core, {dt, now_ms, events}, &out)` once per frame, a `Services` struct given once (storage get/set/remove, entropy, log, `desktop` flag), and a 384 by 216 RGBA frame out. **Time (`now_ms: f64`) and dice are arguments, never read inside**, so tests control them.
  - `rules.odin`, `world.odin`, `rng.odin` (own splitmix64): the game. All counts are `i64` saturating at 9e15 (`SATURATION`); wasm `int` is 32 bits, so never use `int` for game numbers.
  - `render.odin`, `views.odin`, `menus.odin`: a software rasterizer on hue indexes (16 hues, converted to RGBA last) with a proportional bitmap font; `font_data.odin` is **generated** from `odin/assets/m5x7.ttf` (m5x7 by Daniel Linssen, CC0, credited on the About screen).
  - `screens.odin`, `core.odin`: one state machine over ten screens (Splash, Main Menu, Neutral, Shoppe, Game Menu, Confirm Abandon/Embark/Quit, About, Options, Window Size), commands and taps. Web has no Quit/Options; native does.
  - `save.odin`: one JSON text per key (`mhos:save`, desktop options in `mhos:config`), hand-written so a re-save is byte-identical, every field validated on load; a bad file is ignored and left on disk; Abandon writes an `"empty"` marker instead of removing the key.
- **`odin/platform/web/`** (`#+build js`): `js_wasm32` main exporting `platform_frame`, `platform_command`, `platform_tap`; `page/` has `index.html`, `platform.js` (own `requestAnimationFrame` loop, canvas blit) and `storage.js` (localStorage shim). **`odin/platform/native/`** (`#+build !js`): SDL2 window, saves in the per-user data directory.
- **`odin/tests/`**: native tests (`#+build !js`). `reference_test.odin` compares rendered screens pixel for pixel with frames recorded from the real VB game (`docs/reference/vb/*.txt`, loaded with `#load`); `save_test.odin` includes single-rule mutations and a save fuzzer; `play_test.odin` has tap tests and long random "soak" games; `parity_test.odin` plays a scripted game whose digest must equal the one `tools/wasm_parity.js` gets from the built wasm under node.
- **`tools/vb-oracle/`** drives the original VB game headlessly and records its screens (uses `src/` and `m5x7.json`). Everything under `src/` is the reference VB.NET game; it can no longer run (its font was removed from use), and its `CyFont*.json` files are the unusable Windows XP font.

## Conventions and pitfalls

- A constant array (`X :: [N]T{...}`) cannot be indexed at run time: copy it to a variable (`font_rows := FONT_ROWS`). Do not put a composite literal in a `for ... in` header.
- Imports of `core:os` belong only in `!js` files. Put `-vet-shadowing`-clean code in the core: `tools/test.sh` runs the real build with the vet flags (`odin check` does not).
- Parse saves with `json.parse_string(text, .JSON, true, allocator)` (the default spec is JSON5).
- Do not rely on the order of two calls in one expression on wasm.
- Package-level maps in tests must be freed and reset per test (each test has its own allocator).
- `pkill -f` and `fuser -k` can end the shell or kill someone else's server; check the port first.
