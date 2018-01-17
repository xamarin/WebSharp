TOP=$(realpath $(CURDIR))

EMCC=source $(TOP)/emsdk/emsdk_env.sh && emcc
EMPLUSPLUS=source $(TOP)/emsdk/emsdk_env.sh && em++
WASM_BCL_DIR=$(TOP)/bcl/wasm
EXTRA_EXPORTED_RUNTIME_METHODS=['ccall', 'cwrap']
#, 'intArrayFromString', 'intArrayToString', 'setValue', 'getValue', 'allocate', 'Pointer_stringify', 'AsciiToString', 'stringToAscii', 'UTF8ArrayToString', 'UTF8ToString', 'stringToUTF8Array', 'stringToUTF8', 'UTF16ToString', 'stringToUTF16', 'lengthBytesUTF16', 'UTF32ToString', 'stringToUTF32', 'lengthBytesUTF32', 'allocateUTF8', 'stackTrace', 'writeStringToMemory', 'writeArrayToMemory', 'writeAsciiToMemory' ]

BCL_ASSEMBLIES= \
        mscorlib.dll    \
        System.dll      \
        System.Core.dll

APP_SOURCES = \
	websharpwasm.cs	

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

monoembedding.o: monoembedding.cpp | out/
	$(EMPLUSPLUS) -g -Os -s WASM=1 -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -std=c++11 monoembedding.cpp -c -o $(TOP)/build/monoembedding.o

websharpwasm.js: monoembedding.o library_mono.js $(TOP)/libmonosgen-2.0.a
	$(EMCC) -g4 -Os -s WASM=1 -s FORCE_FILESYSTEM=1 -s "EXTRA_EXPORTED_RUNTIME_METHODS=$(EXTRA_EXPORTED_RUNTIME_METHODS)" -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -s ASSERTIONS=2 --js-library library_mono.js $(TOP)/build/monoembedding.o libmonosgen-2.0.a -o $(TOP)/build/websharpwasm.js

build-native: websharpwasm.js

#$(patsubst %,managed/%,$(BCL_ASSEMBLIES)): managed/%: $(WASM_BCL_DIR)/% | managed/
#	cp $< $@

websharpwasm.exe: $(APP_SOURCES) | out/
	csc /nostdlib /unsafe -out:$(TOP)/build/$@ /reference:mscorlib.dll $(APP_SOURCES)

build-managed: websharpwasm.exe

build: toolchain build-native build-managed

clean:
	rm -rf .stamp-wasm-toolchain

package:
