/* These functions are supposed to be included by passing
         * -s DEFAULT_LIBRARY_FUNCS_TO_INCLUDE=[...] to the emcc linker,
         * but MSBuild makes it impossible to do that. Instead I copied
         * them from Emscripten's library.js directly into here. -caleb
         */
function listenOnce(object, event, func) {
    object.addEventListener(event, func, { 'once': true });
}
function autoResumeAudioContext(ctx, elements) {
    if (!elements) {
        elements = [document, document.getElementById('canvas')];
    }
    ['keydown', 'mousedown', 'touchstart'].forEach(function (event) {
        elements.forEach(function (element) {
            if (element) {
                listenOnce(element, event, function () {
                    if (ctx.state === 'suspended') ctx.resume();
                });
            }
        });
    });
}
function dynCallLegacy(sig, ptr, args) {
    assert(('dynCall_' + sig) in Module, 'bad function pointer type - no table for sig \'' + sig + '\'');
    if (args && args.length) {
        // j (64-bit integer) must be passed in as two numbers [low 32, high 32].
        assert(args.length === sig.substring(1).replace(/j/g, '--').length);
    } else {
        assert(sig.length == 1);
    }
    var f = Module["dynCall_" + sig];
    return args && args.length ? f.apply(null, [ptr].concat(args)) : f.call(null, ptr);
}
function dynCall(sig, ptr, args) {
    if (sig.indexOf('j') != -1) {
        return dynCallLegacy(sig, ptr, args);
    }
    assert(wasmTable.get(ptr), 'missing table entry in dynCall: ' + ptr);
    return wasmTable.get(ptr).apply(null, args)
}

const SDL_TRUE = 1;