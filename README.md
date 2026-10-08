# murder-hobo-of-splorr

*Itinerance and slaughter!* A small idle-clicker "metaphor" by TheGrumpyGameDev: attempt murders, earn XP, buy skill, difficulty and auto-murder in the Shoppe.

Play it in the browser: https://thegrumpygamedev.itch.io/murder-hobo-of-splorr

## Credits

The text is set in **m5x7** by Daniel Linssen (managore), https://managore.itch.io/m5x7, CC0 (`odin/assets/`). It replaced a font taken from Windows XP's Small Fonts.

## Layout

- `odin/` is the game: Odin, compiled to `js_wasm32` for the browser and to a native SDL2 window. `odin/game` is the portable core (rules, screens, software renderer, save format); `odin/platform/web` and `odin/platform/native` only show its 384 by 216 frame and pass input in.
- `docs/reference/vb/` holds screens recorded from the original VB.NET / MonoGame game, which the port is checked against pixel for pixel. The VB source itself was removed after shipping (it is in git history at `32b498e` and earlier).
- `docs/PORT_PLAN.md` and `docs/QUIRKS.md` record the port's decisions.

## Commands

```bash
tools/test.sh               # generated-data check, native tests, builds both platforms
tools/build.sh [web|native]
tools/serve.sh              # http://localhost:8080 (PORT=... to change)
tools/ship.sh [--push]      # tests, optimized web build, zip; uploads only with --push
```

Needs the Odin compiler (`dev-2026` nightly), SDL2 for the native client, Python 3 with Pillow for the font generator.
