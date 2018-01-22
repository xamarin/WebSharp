TOP=$(realpath $(CURDIR))

EMCC=source $(TOP)/emsdk/emsdk_env.sh && emcc
EMPLUSPLUS=source $(TOP)/emsdk/emsdk_env.sh && em++
WASM_BCL_DIR=$(TOP)/bcl/wasm
EXTRA_EXPORTED_RUNTIME_METHODS=['ccall', 'cwrap', 'setValue', 'getValue']
#, 'intArrayFromString', 'intArrayToString', 'setValue', 'getValue', 'allocate', 'Pointer_stringify', 'AsciiToString', 'stringToAscii', 'UTF8ArrayToString', 'UTF8ToString', 'stringToUTF8Array', 'stringToUTF8', 'UTF16ToString', 'stringToUTF16', 'lengthBytesUTF16', 'UTF32ToString', 'stringToUTF32', 'lengthBytesUTF32', 'allocateUTF8', 'stackTrace', 'writeStringToMemory', 'writeArrayToMemory', 'writeAsciiToMemory' ]

BCL_ASSEMBLIES= \
        mscorlib.dll    \
        System.dll      \
        System.Core.dll

APP_SOURCES = \
	./src/Mono.WebAssembly/Runtime.cs	

EMBEDDING_SOURCES = \
    ./src/Mono/clrfuncreflectionwrap.cs  \
	./src/Mono/monoembedding.cs	

NATIVE_EMBEDDING_SOURCES = \
	./src/Mono/monoembedding.cpp	

$(TOP)/emsdk:
	git clone https://github.com/juj/emsdk.git $(TOP)/emsdk

.stamp-wasm-toolchain: | $(TOP)/emsdk
	cd $(TOP)/emsdk && ./emsdk install latest
	cd $(TOP)/emsdk && ./emsdk activate --embedded latest
	touch $@

.PHONY: toolchain
toolchain: .stamp-wasm-toolchain

out/:
	mkdir -p $(TOP)/build

monoembedding.o: $(NATIVE_EMBEDDING_SOURCES) | out/
	$(EMPLUSPLUS) -g -Os -s WASM=1 -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -std=c++11 $(NATIVE_EMBEDDING_SOURCES) -c -o $(TOP)/build/monoembedding.o

websharpwasm.js: monoembedding.o library_mono.js $(TOP)/libmonosgen-2.0.a
	$(EMCC) -g4 -Os -s WASM=1 -s FORCE_FILESYSTEM=1 -s "EXTRA_EXPORTED_RUNTIME_METHODS=$(EXTRA_EXPORTED_RUNTIME_METHODS)" -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -s ASSERTIONS=2 --js-library library_mono.js $(TOP)/build/monoembedding.o libmonosgen-2.0.a -o $(TOP)/build/websharpwasm.js

build-native: websharpwasm.js
	# Fix up generated wasm file so it can be loaded from a node module.
	echo 'var Module = module.parent.exports.Module;\n' | cat - $(TOP)/build/websharpwasm.js > wasmtemp && mv wasmtemp $(TOP)/build/websharpwasm.js
	cp $(TOP)/build/websharpwasm.js $(TOP)/../../lib/wasm
	cp $(TOP)/build/websharpwasm.wasm* $(TOP)/../../lib/wasm
	cp $(TOP)/build/websharpwasm.wast* $(TOP)/../../lib/wasm

websharpwasm.dll: $(APP_SOURCES) | out/
	csc /noconfig /unsafe+ /nowarn:1701,1702,2008 /nostdlib+ /target:library -out:$(TOP)/build/$@ /reference:mscorlib.dll /reference:System.Core.dll /reference:System.dll $(APP_SOURCES)
	cp $(TOP)/build/$@ $(TOP)/../../lib/wasm

MonoEmbedding.exe: $(EMBEDDING_SOURCES) | out/
	csc /noconfig /unsafe+ /nowarn:1701,1702,2008 /nostdlib+ -out:$(TOP)/build/$@ /reference:mscorlib.dll /reference:System.Core.dll /reference:System.dll $(EMBEDDING_SOURCES)
	cp $(TOP)/build/$@ $(TOP)/../../lib/wasm	

build-managed: websharpwasm.dll MonoEmbedding.exe

build: toolchain build-native build-managed

clean:
	rm -rf .stamp-wasm-toolchain

package:
