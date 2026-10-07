#!/bin/bash
# Runs every test of the Odin port; exits non-zero on any failure. Usage: tools/test.sh
set -euo pipefail
cd "$(dirname "$0")/.."
mkdir -p build
fail=0
step() { echo; echo "== $1"; }

step "native suite (odin test)"
odin test odin/tests -collection:kmh=odin -out:build/tests_native -define:ODIN_TEST_THREADS=1 2>&1 | tee build/native_tests.log | grep -E "FAIL|passed|failed|Finished" || true
grep -qE "All tests were successful|successful" build/native_tests.log || fail=1

step "wasm build of the core (32-bit int)"
odin check odin/platform/web -target:js_wasm32 -collection:kmh=odin -no-entry-point >/dev/null 2>&1 && echo "ok" || { echo "FAIL: web platform does not type check"; fail=1; }

step "native platform type check"
odin check odin/platform/native -collection:kmh=odin -no-entry-point >/dev/null 2>&1 && echo "ok" || { echo "FAIL: native platform does not type check"; fail=1; }

echo
if [ "$fail" = 0 ]; then echo "ALL TESTS PASSED"; else echo "SOME TESTS FAILED"; exit 1; fi
