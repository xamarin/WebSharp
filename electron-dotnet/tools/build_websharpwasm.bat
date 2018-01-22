@echo off

set SELF=%~dp0

if not exist "%SELF%\build\downloadX.exe" (
	csc /out:"%SELF%\build\downloadX.exe" "%SELF%\download.cs"
)

set EXTRA_EXPORTED_RUNTIME_METHODS=['ccall', 'cwrap', 'setValue', 'getValue']

@rem , 'intArrayFromString', 'intArrayToString', 'setValue', 'getValue', 'allocate', 'Pointer_stringify', 'AsciiToString', 'stringToAscii', 'UTF8ArrayToString', 'UTF8ToString', 'stringToUTF8Array', 'stringToUTF8', 'UTF16ToString', 'stringToUTF16', 'lengthBytesUTF16', 'UTF32ToString', 'stringToUTF32', 'lengthBytesUTF32', 'allocateUTF8', 'stackTrace', 'writeStringToMemory', 'writeArrayToMemory', 'writeAsciiToMemory' ]

@rem Build WebSharp WASM
cd "%SELF%\..\src\websharpwasm\"
mkdir "build" > nul 2>&1

@rem Make sure we have the latest Emscripten SDK
mkdir "emsdk" > nul 2>&1

if not exist ".\emsdk\emsdk-portable-64bit.zip" (
    echo Downloading Emscripten SDK
	"..\..\tools\build\downloadX.exe" https://s3.amazonaws.com/mozilla-games/emscripten/releases/emsdk-portable-64bit.zip ".\emsdk\emsdk-portable-64bit.zip"
)

if not exist ".\emsdk\emsdk.bat" (
    echo Unzipping Emscripten SDK
	pushd ".\emsdk\"
	cscript //B ..\..\..\tools\unzip.vbs emsdk-portable-64bit.zip
	popd

    echo Installing Emscripten SDK
    @rem Fetch the latest registry of available tools.
    call .\emsdk\emsdk update

    @rem Download and install the latest SDK tools.
    call .\emsdk\emsdk install latest

    @rem Make the "latest" SDK "active" for the current user. (writes ~/.emscripten file)
    call .\emsdk\emsdk activate latest

    @rem Activate PATH and other environment variables in the current terminal
    @rem call .\emsdk\emsdk_env.bat
)

SETLOCAL

echo Setting up Emscripten paths
call .\emsdk\emsdk_env.bat


@rem Build MonoEmbedding module
echo Building monoembedding module

call em++ -g -Os -s WASM=1 -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -std=c++11 ./src/Mono/monoembedding.cpp -c -o build/monoembedding.o

if %ERRORLEVEL% neq 0 (
	echo Failure building monoembedding module
	cd "%SELF%"
	exit /b -1
)

@rem Generating Websharp Wasm
echo Generating Websharp Wasm files

call emcc -g4 -Os -s WASM=1 -s FORCE_FILESYSTEM=1 -s "EXTRA_EXPORTED_RUNTIME_METHODS=%EXTRA_EXPORTED_RUNTIME_METHODS%" -s ALLOW_MEMORY_GROWTH=1 -s BINARYEN=1 -s "BINARYEN_TRAP_MODE='clamp'" -s TOTAL_MEMORY=134217728 -s ALIASING_FUNCTION_POINTERS=0 -s ASSERTIONS=2 --js-library library_mono.js build/monoembedding.o libmonosgen-2.0.a -o build/websharpwasm.js

if %ERRORLEVEL% neq 0 (
	echo Failure generating websharpwasm.js
	cd "%SELF%"
	exit /b -1
)

ENDLOCAL


echo Fixing up websharpwasm.js file

rem Fix up generated wasm file so it can be loaded from a node module.
echo.var Module = module.parent.exports.Module; > wasmtemp
type .\build\websharpwasm.js >> wasmtemp
move /y wasmtemp .\build\websharpwasm.js

set DESTDIR= "..\..\lib\wasm\"

if not exist %DESTDIR%\NUL mkdir %DESTDIR%

echo Copying Generated files to %DESTDIR%
copy /y .\build\websharpwasm.js %DESTDIR%
copy /y .\build\websharpwasm.wasm* %DESTDIR%
copy /y .\build\websharpwasm.wast %DESTDIR%

@rem Building WebSharp Wasm C# interface
echo Building WebSharp Wasm C# interface

csc /nostdlib /unsafe /target:library -out:build/websharpwasm.dll /reference:mscorlib.dll /reference:System.Core.dll /reference:System.dll ./src/Mono.WebAssembly/Runtime.cs
if %ERRORLEVEL% neq 0 (
	echo Failure building websharpwasm C# interface
	cd "%SELF%"
	exit /b -1
)

csc /nostdlib /unsafe -out:build/MonoEmbedding.exe /reference:mscorlib.dll /reference:System.Core.dll /reference:System.dll ./src/Mono/clrfuncreflectionwrap.cs ./src/Mono/monoembedding.cs
if %ERRORLEVEL% neq 0 (
	echo Failure building monoembedding C# interface
	cd "%SELF%"
	exit /b -1
)

echo Copying WebSharp Wasm C# interface
copy /y .\build\websharpwasm.dll %DESTDIR%
copy /y .\build\MonoEmbedding.exe %DESTDIR%

cd "%SELF%"
echo SUCCESS. WebSharp Wasm interface build.

exit /b 0
