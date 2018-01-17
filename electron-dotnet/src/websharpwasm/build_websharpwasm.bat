set EXTRA_EXPORTED_RUNTIME_METHODS=['ccall', 'cwrap', 'intArrayFromString', 'intArrayToString', 'setValue', 'getValue', 'allocate', 'Pointer_stringify', 'AsciiToString', 'stringToAscii', 'UTF8ArrayToString', 'UTF8ToString', 'stringToUTF8Array', 'stringToUTF8', 'UTF16ToString', 'stringToUTF16', 'lengthBytesUTF16', 'UTF32ToString', 'stringToUTF32', 'lengthBytesUTF32', 'allocateUTF8', 'stackTrace', 'writeStringToMemory', 'writeArrayToMemory', 'writeAsciiToMemory' ]

@rem Build WebSharp WASM
call emcc -g4 -Os -s WASM=1 -s FORCE_FILESYSTEM=1 -s "EXTRA_EXPORTED_RUNTIME_METHODS=%EXTRA_EXPORTED_RUNTIME_METHODS%" -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -s ASSERTIONS=2 --js-library library_mono.js build/monoembedding.o libmonosgen-2.0.a -o build/websharpwasm.js

