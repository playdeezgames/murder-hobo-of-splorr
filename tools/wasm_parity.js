// Runs the built web platform (build/web/platform.wasm) under node through the same scripted game as
// odin/tests/parity_test.odin runs natively, and writes a digest of every frame and of the final save.
// The two digests must be equal: that proves the wasm build (32-bit int, its own allocator) plays exactly like the native one.
// Usage: ODIN_JS=<odin.js> node tools/wasm_parity.js build/web build/parity_wasm.txt
const fs = require("fs"), vm = require("vm");
global.window = global; global.self = global;
global.requestAnimationFrame = (f) => setTimeout(() => f(performance.now()), 0);
vm.runInThisContext(fs.readFileSync(process.env.ODIN_JS, "utf8"));
vm.runInThisContext(fs.readFileSync(process.argv[2] + "/storage.js", "utf8"));
const store = new Map();
global.localStorage = { getItem: (k) => (store.has(k) ? store.get(k) : null), setItem: (k, v) => store.set(k, String(v)), removeItem: (k) => store.delete(k) };
global.fetch = async (p) => ({ arrayBuffer: async () => fs.readFileSync(p) });

const STEPS = +(process.env.STEPS || 2000), W = 384, H = 216;
let lcg = 12345;
const next = () => { lcg = (Math.imul(lcg, 1664525) + 1013904223) >>> 0; return lcg; };
const fnv = (h, byte) => (Math.imul(h ^ byte, 16777619) >>> 0);

(async () => {
  const mem = new odin.WasmMemoryInterface();
  let half = 0;
  const platform = { js_entropy_u32() { half ^= 1; return half ? 0 : 7; }, js_log() {} }; // entropy is 7, as in the native test
  await odin.runWasm(process.argv[2] + "/platform.wasm", null, { ...storageImports(mem), platform }, mem);
  const ex = mem.exports;
  let frames = 2166136261, now = 1790000000000;
  const trace = [];
  for (let step = 0; step < STEPS; step++) {
    const sel = (next() >>> 16) % 10;
    if (sel === 9) {
      const x = (next() >>> 16) % W, y = (next() >>> 16) % H, precise = (next() >>> 16) % 2 === 0;
      ex.platform_tap(x, y, precise);
    } else {
      ex.platform_command(1 + (next() >>> 16) % 6);
    }
    now += 1000 + (next() >>> 16) % 90000;
    if (step % 50 === 49) now += 6 * 3600 * 1000;
    ex.platform_frame(0.016, now);
    if (!ex.platform_frame_changed()) { frames = fnv(frames, 0); trace.push("-"); continue; } // an unchanged frame is hashed as one zero byte
    const bytes = new Uint8Array(mem.memory.buffer, ex.platform_frame_ptr(), W * H * 4);
    let one = 2166136261;
    for (let i = 0; i < bytes.length; i++) { frames = fnv(frames, bytes[i]); one = fnv(one, bytes[i]); }
    trace.push(one);
    if (process.env.DUMP_STEP && +process.env.DUMP_STEP === step) fs.writeFileSync('build/parity_wasm_frame.bin', Buffer.from(bytes));
  }
  let save = 2166136261;
  for (const b of Buffer.from(store.get("mhos:save") || "")) save = fnv(save, b);
  fs.writeFileSync(process.argv[3].replace(".txt", "_trace.txt"), trace.join("\n") + "\n");
  fs.writeFileSync(process.argv[3].replace(".txt", "_save.txt"), store.get("mhos:save") || "");
  fs.writeFileSync(process.argv[3], `frames=${frames} save=${save} saved=${store.has("mhos:save")}\n`);
  console.log("wasm parity digest written");
})().catch((e) => { console.error("FAILED:", e); process.exit(1); });
