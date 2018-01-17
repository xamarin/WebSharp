@rem Build MonoEmbedding module
em++ -g -Os -s WASM=1 -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -std=c++11 monoembedding.cpp -c -o build/monoembedding.o
