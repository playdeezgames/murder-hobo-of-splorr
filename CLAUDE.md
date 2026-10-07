# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

"Murder Hobo of SPLORR!!": a VB.NET / MonoGame (DesktopGL) idle-clicker-style game. The player attempts murders, earns XP, and buys skill, difficulty and auto-murder upgrades. Part of TheGrumpyGameDev's "of SPLORR!!" games. Background lives in the Obsidian vault at `/home/yermom/git/bok-of-splorr/splorr/` (start at `Home.md`; `Gotchas.md` and `Tech/Shipping to itch.io.md` are the useful notes). The vault has no page for this game yet.

**`README.md` is useless**: it is a copy of the "solitary-ancient-ruins-of-splorr" jam link log and says nothing about this game. `shippit.sh` and `src/MHOS/aboot.txt` (credits, default `keys.json`) also carry that older game's lineage. The `src/SPLORR.Game` project (`Maze/`, `RNG.vb`) comes from the same ancestor; the murder game itself does not use the maze.

## Commands

No tests exist. The solution is `src/src.sln`.

```bash
dotnet build src/src.sln
dotnet run --project src/MHOS/MHOS.vbproj
```

Run from the build output directory (or via `dotnet run`, which copies `Content/`): `Program.vb` reads `Content/keys.json`, `hue.json`, `sfx.json`, `font.json` and `mux.json` by relative path at startup.

`shippit.sh` publishes self-contained single-file builds for linux/windows/mac, pushes each to itch.io with `butler`, then commits everything with `git add -A`. **Only run it when the user explicitly says to**: it publishes publicly and commits. It is not executable; use `bash shippit.sh`.

## Architecture

Layered projects; dependencies point downward. Everything is `netstandard2.1` except where noted.

- **MHOS** (net8.0 exe): `Program.vb` is the composition root. It loads the JSON content config, maps keys and gamepad buttons to abstract commands (`A`, `B`, `Up`, `Down`, `Left`, `Right`), and starts the `Host`.
- **AOS.Presentation** (net8.0, MonoGame): `Host` is the MonoGame `Game`; `DisplayBuffer` is the pixel surface. This is the only layer that touches MonoGame, apart from `MHOS`.
- **AOS.UI** (net8.0): the reusable, game-agnostic UI framework ("AOS"). A state-machine game controller (`BaseGameController`, `BaseGameState`, `BasePickerState`), pixel buffers, bitmap fonts, sprites, and a set of stock "Boilerplate" states (splash, main menu, options, save/load, confirm quit/abandon, volume, window size, about). Boilerplate states are keyed by `BoilerplateState` constants. Generic over a world model type.
- **MHOS.Presentation**: the game's UI. `GameController` extends `BaseGameController(Of IWorldModel)` and registers the game-specific `EmbarkState` and `NeutralState` on top of the boilerplate states. `MHOSContext` and `MHOSSettings` supply fonts, view size and settings.
- **MHOS.Business**: game logic as a dialog tree. `WorldModel` holds the current `IDialog` (`NeutralDialog`, `ShoppeDialog`); each dialog exposes a description and `IChoice`s (`MurderChoice`, `SkillIncreaseChoice`, `DifficultyIncreaseChoice`, `AutoMurderIncreaseChoice`, `ShoppeChoice`, `CancelChoice`). `MakeChoice` replaces the current dialog with whatever `choice.Choose()` returns; `GoBack` pops it. The UI only sees `IWorldModel`.
- **MHOS.Persistence**: the `IWorld` interface (murder, skill, XP, streak and auto-murder operations).
- **MHOS.Persistence.Implementation**: `World` implements `IWorld` over a `WorldData` object. `Moods.vb` supplies message moods.
- **MHOS.Data** (System.Text.Json): plain serializable data (`WorldData`, `MessageData`). Saving is `JsonSerializer.Serialize(WorldData)`, so a new persisted field must be a public property with a default value on `WorldData` (older saves then load with the default).
- **FontMaker** (net6.0, System.Drawing): standalone tool that builds the font data from the PNG sources in its folder; not part of the game's runtime.

Auto-murder is time-based: `WorldData.NextAutoMurder` and `AutoMurderInterval` (seconds) are stored in the save, and `IWorld.AutoMurderTimeRemaining` exposes the countdown to the UI.

## Conventions

- Source files are VB.NET, often with a UTF-8 BOM. Keep the BOM on files that have it.
- The vault's standing rules apply to sessions here: do not `git push` or run any publish/ship script unless the user explicitly says so, and do not describe a game's deliberate design as a bug.
