// Browser side of the platform interface: draw one RGBA frame, turn keys and taps into commands, store text.
(async function () {
	const FRAME_W = 384, FRAME_H = 216;
	const COMMAND = { up: 1, down: 2, left: 3, right: 4, confirm: 5, cancel: 6 };
	const debugLog = new URLSearchParams(location.search).has("log"); // ?log=1 prints every input sent to the game
	const logInput = (...a) => { if (debugLog) console.log("[input]", ...a); };

	const mem = new odin.WasmMemoryInterface();
	const platformImports = {
		js_entropy_u32() { return crypto.getRandomValues(new Uint32Array(1))[0] | 0; },
		js_log(p, n) { console.log(mem.loadString(p, n)); },
	};
	await odin.runWasm("platform.wasm", null, { ...storageImports(mem), platform: platformImports }, mem);
	const exports = mem.exports;

	// ---- layout: the largest whole multiple of the frame that fits, else a smooth fit; centred on black ----------
	const canvas = document.getElementById("screen"), ctx = canvas.getContext("2d");
	let laidOutFor = "";
	function layout() {
		const w = innerWidth, h = innerHeight;
		laidOutFor = w + "x" + h;
		if (w === 0 || h === 0) return;
		let scale = Math.min(w / FRAME_W, h / FRAME_H);
		if (scale >= 1) scale = Math.floor(scale);
		const cw = Math.floor(FRAME_W * scale), ch = Math.floor(FRAME_H * scale);
		canvas.style.width = cw + "px"; canvas.style.height = ch + "px";
		canvas.style.left = Math.floor((w - cw) / 2) + "px"; canvas.style.top = Math.floor((h - ch) / 2) + "px";
	}
	addEventListener("resize", layout); layout();

	// ---- input. The logical key (e.key) is read before the physical code: remote desktops can send wrong codes. -------
	const KEYS_BY_KEY = { ArrowUp: "up", ArrowDown: "down", ArrowLeft: "left", ArrowRight: "right", Enter: "confirm", " ": "confirm", Escape: "cancel", Backspace: "cancel",
		w: "up", s: "down", a: "left", d: "right", W: "up", S: "down", A: "left", D: "right" };
	const KEYS_BY_CODE = { Numpad8: "up", Numpad2: "down", Numpad4: "left", Numpad6: "right", Numpad5: "confirm", NumpadEnter: "confirm", Numpad0: "cancel" };
	addEventListener("keydown", (e) => {
		const name = KEYS_BY_KEY[e.key] || KEYS_BY_CODE[e.code];
		if (!name || e.repeat) return;
		e.preventDefault();
		logInput("key", e.key, e.code, "->", name);
		exports.platform_command(COMMAND[name]);
	});
	canvas.addEventListener("pointerdown", (e) => {
		e.preventDefault();
		const r = canvas.getBoundingClientRect();
		const x = Math.floor((e.clientX - r.left) / r.width * FRAME_W), y = Math.floor((e.clientY - r.top) / r.height * FRAME_H);
		logInput("tap", x, y, e.pointerType);
		exports.platform_tap(x, y);
	});
	addEventListener("contextmenu", (e) => e.preventDefault());

	// ---- the frame loop ---------------------------------------------------------------------------------
	let prev = performance.now();
	function frame(now) {
		if (laidOutFor !== innerWidth + "x" + innerHeight) layout(); // a pane that was hidden at load reports 0 by 0
		exports.platform_frame(Math.min((now - prev) / 1000, 0.25), Date.now()); prev = now;
		const ptr = exports.platform_frame_ptr();
		if (ptr && exports.platform_frame_changed()) {
			ctx.putImageData(new ImageData(new Uint8ClampedArray(mem.memory.buffer, ptr, FRAME_W * FRAME_H * 4), FRAME_W, FRAME_H), 0, 0);
		}
		requestAnimationFrame(frame);
	}
	window.__platform = { exports, mem };
	requestAnimationFrame(frame);
})();
