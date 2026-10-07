#!/bin/bash
# Runs every test of the Odin port; exits non-zero on any failure. Usage: tools/test.sh
set -euo pipefail
cd "$(dirname "$0")/.."
mkdir -p build
fail=0
step() { echo; echo "== $1"; }

step "generated font data is up to date"
cp odin/game/font_data.odin build/font_data.odin.before
cp tools/vb-oracle/m5x7.json build/m5x7.json.before
python3 tools/gen/gen_font.py >/dev/null
cmp -s odin/game/font_data.odin build/font_data.odin.before && cmp -s tools/vb-oracle/m5x7.json build/m5x7.json.before && echo "ok" || { echo "FAIL: the font files changed when regenerated; commit the result"; fail=1; }

step "native suite (odin test)"
odin test odin/tests -collection:kmh=odin -o:speed -out:build/tests_native -define:ODIN_TEST_THREADS=1 2>&1 | tee build/native_tests.log | grep -E "FAIL|passed|failed|Finished" || true
grep -qE "All tests were successful|successful" build/native_tests.log || fail=1

step "both platforms build (with the vet flags, so 32-bit and shadowing mistakes show)"
tools/build.sh all >/dev/null && echo "ok" || { echo "FAIL: a platform does not build"; fail=1; }

echo
if [ "$fail" = 0 ]; then echo "ALL TESTS PASSED"; else echo "SOME TESTS FAILED"; exit 1; fi
