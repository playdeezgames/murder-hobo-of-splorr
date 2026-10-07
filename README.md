# murder-hobo-of-splorr

*Itinerance and slaughter!* A small idle-clicker "metaphor" by TheGrumpyGameDev: attempt murders, earn XP, buy skill, difficulty and auto-murder in the Shoppe.

Play it in the browser: https://thegrumpygamedev.itch.io/murder-hobo-of-splorr

## Layout

- `odin/` is the game: Odin, compiled to `js_wasm32` for the browser and to a native SDL2 window. `odin/game` is the portable core (rules, screens, software renderer, save format); `odin/platform/web` and `odin/platform/native` only show its 384 by 216 frame and pass input in.
- `src/` is the original VB.NET / MonoGame game. It is kept as the reference the port was checked against (`tools/vb-oracle` drives it headlessly and records frames into `docs/reference/vb`).
- `docs/PORT_PLAN.md` and `docs/QUIRKS.md` record the port's decisions.

## Commands

```bash
tools/test.sh               # generated-data check, native tests, builds both platforms
tools/build.sh [web|native]
tools/serve.sh              # http://localhost:8080 (PORT=... to change)
tools/ship.sh [--push]      # tests, optimized web build, zip; uploads only with --push
```

Needs the Odin compiler (`dev-2026` nightly), SDL2 for the native client, Python 3 for the font generator, and the dotnet SDK only to re-record the VB reference frames (`tools/gen_reference.sh`).
