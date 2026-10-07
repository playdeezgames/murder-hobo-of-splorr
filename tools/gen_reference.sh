#!/bin/bash
# Re-records docs/reference/vb/*.txt by driving the original VB game headlessly (needs the dotnet SDK).
set -euo pipefail
cd "$(dirname "$0")/../tools/vb-oracle"
dotnet run -- ../../src/MHOS/Content/Fonts ../../docs/reference/vb
