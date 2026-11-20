# Reference frames from the original VB.NET game

Each `.txt` is one screen of the original game: 216 lines of 384 characters, each character a hue index (0 to 15) in hex. They were recorded on October 7 and 8, 2026 by a small VB program that drove the real `GameController` without a window (a world loaded from JSON, then key commands), using the m5x7 font (the original's own font could not be shipped). `odin/tests/reference_test.odin` loads them with `#load`, draws the same screens with the Odin renderer and requires every pixel to match.

The VB source (`src/`) and the recording program (`tools/vb-oracle/`, `tools/gen_reference.sh`) were deleted after shipping. They are in git history: `git checkout 32b498e` brings them back; `tools/gen_reference.sh` there re-records the frames (needs the dotnet SDK).
