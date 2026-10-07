#!/bin/bash
# Ships the browser build. Builds and zips only; add --push to upload. (The old script published the VB.NET builds and committed.)
exec "$(dirname "$0")/tools/ship.sh" "$@"
