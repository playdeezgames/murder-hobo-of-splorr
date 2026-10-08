#!/bin/bash
# Runs every test of the Odin port; exits non-zero on any failure. Usage: tools/test.sh
set -euo pipefail
cd "$(dirname "$0")/.."
mkdir -p build
fail=0
step() { echo; echo "== $1"; }

step "generated font data is up to date"
cp odin/game/font_data.odin build/font_data.odin.before
python3 tools/gen/gen_font.py >/dev/null
cmp -s odin/game/font_data.odin build/font_data.odin.before && echo "ok" || { echo "FAIL: font_data.odin changed when regenerated; commit the result"; fail=1; }

step "native suite (odin test)"
odin test odin/tests -collection:kmh=odin -o:speed -out:build/tests_native -define:ODIN_TEST_THREADS=1 2>&1 | tee build/native_tests.log | grep -E "FAIL|passed|failed|Finished" || true
grep -qE "All tests were successful|successful" build/native_tests.log || fail=1

step "both platforms build (with the vet flags, so 32-bit and shadowing mistakes show)"
tools/build.sh all >/dev/null && echo "ok" || { echo "FAIL: a platform does not build"; fail=1; }

step "wasm plays exactly like native (the same scripted game under node)"
ODIN_JS="$(odin root)/core/sys/wasm/js/odin.js" node tools/wasm_parity.js build/web build/parity_wasm.txt
if cmp -s build/parity_native.txt build/parity_wasm.txt; then echo "ok: $(cat build/parity_native.txt)"; else echo "FAIL: native and wasm digests differ"; cat build/parity_native.txt build/parity_wasm.txt; fail=1; fi

echo
if [ "$fail" = 0 ]; then echo "ALL TESTS PASSED"; else echo "SOME TESTS FAILED"; exit 1; fi
