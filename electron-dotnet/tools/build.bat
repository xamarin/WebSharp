@echo off
set SELF=%~dp0
set argc=0
for %%x in (%*) do Set /A argc+=1

if /I "%argc%" LSS "2" (
    echo Usage: build.bat debug^|release target "{version} {version}" ...
    echo e.g. build.bat release 1.4.0 6.5.0
    echo e.g. build.bat release 1.5.0 7.0.0
    echo e.g. build.bat release 1.6.0 7.4.0
    exit /b -1
)

if /I "%argc%" geq "3" (
    set FLAVOR=%1
    set TARGET=%2
    shift
    shift
)

if /I "%argc%" equ "2" (
    set FLAVOR=release
    set TARGET=%1
    shift
)

if "%FLAVOR%" equ "" set FLAVOR=release

echo building using the following:
echo FLAVOR = %FLAVOR%
echo TARGET = %TARGET% 
echo VERSION = %1

if "%MONO_ROOT_X86%" equ "" set MONO_ROOT_X86="C:\Program Files (x86)\Mono"
if "%MONO_ROOT_X64%" equ "" set MONO_ROOT_X64="C:\Program Files\Mono"

for %%i in (node.exe) do set NODEEXE=%%~$PATH:i
if not exist "%NODEEXE%" (
    echo Cannot find node.exe
    popd
    exit /b -1
)
for %%i in ("%NODEEXE%") do set NODEDIR=%%~dpi
SET DESTDIRROOT=%SELF%\..\lib\native\win32
set VERSIONS=
:harvestVersions
if "%1" neq "" (
    set VERSIONS=%VERSIONS% %1
    shift
    goto :harvestVersions
)
if "%VERSIONS%" equ "" set VERSIONS=6.5.0
pushd %SELF%\..

for %%V in (%VERSIONS%) do call :build ia32 x86 %%V 
for %%V in (%VERSIONS%) do call :build x64 x64 %%V 
popd

exit /b 0

:build

set DESTDIR=%DESTDIRROOT%\%1\%TARGET%\%3
if exist "%DESTDIR%\node.exe" goto gyp
if not exist "%DESTDIR%\NUL" mkdir "%DESTDIR%"
echo Downloading node.exe %2 %3...
node "%SELF%\download.js" %2 %3 "%DESTDIR%"
if %ERRORLEVEL% neq 0 (
    echo Cannot download node.exe %2 v%3
    exit /b -1
)

:gyp

echo Building websharp.node %FLAVOR% for node.js %2 v%3
set NODEEXE=%DESTDIR%\node.exe
set GYP=%APPDATA%\npm\node_modules\node-gyp\bin\node-gyp.js
if not exist "%GYP%" (
    echo Cannot find node-gyp at %GYP%. Make sure to install with npm install node-gyp -g
    exit /b -1
)

"%NODEEXE%" "%GYP%" configure --%FLAVOR% build --target=%TARGET% --dist-url=https://atom.io/download/atom-shell --msvs_version=2015
if %ERRORLEVEL% neq 0 (
    echo Error building websharp.node %FLAVOR% for node.js %2 v%3
    exit /b -1
)

echo %DESTDIR%
copy /y .\build\%FLAVOR%\websharp_*.node "%DESTDIR%"
if %ERRORLEVEL% neq 0 (
    echo Error copying websharp.node %FLAVOR% for node.js %2 v%3
    exit /b -1
)

if exist ".\build\%FLAVOR%\MonoEmbedding*.exe" (
    copy /y .\build\%FLAVOR%\MonoEmbedding*.exe "%DESTDIR%"
    if %ERRORLEVEL% neq 0 (
        echo Error copying MonoEmbedding.exe %FLAVOR% for node.js %2 v%3
        exit /b -1
    )
)
REM copy /y "%DESTDIR%\..\msvcr120.dll" "%DESTDIR%"
REM if %ERRORLEVEL% neq 0 (
REM     echo Error copying msvcr120.dll %FLAVOR% to %DESTDIR%
REM     exit /b -1
REM )

echo Success building websharp.node %FLAVOR% for node.js %2 v%3
