@echo off
set SELF=%~dp0
if "%1" equ "" (
    echo Usage: build_peppersharp.bat {Configuration}
    echo e.g. build_peppersharp.bat Release
    exit /b -1
)

SET FLAVOR=%1

mkdir "%SELF%\build\nuget\tools" > nul 2>&1

if not exist "%SELF%\build\download.exe" (
	csc /out:"%SELF%\build\download.exe" "%SELF%\download.cs"
)

if not exist "%SELF%\build\repl.exe" (
	csc /out:"%SELF%\build\repl.exe" "%SELF%\repl.cs"
)

if not exist "%SELF%\build\nuget.exe" (
	"%SELF%\build\download.exe" http://nuget.org/nuget.exe "%SELF%\build\nuget.exe"
)

call :build_peppersharp
if %ERRORLEVEL% neq 0 exit /b -1

call "%SELF%\build\nuget.exe"  pack "%SELF%\nuget\Xamarin.PepperSharp.nuspec" -OutputDirectory "%SELF%\build\nuget" -properties Configuration=%FLAVOR%  
if %ERRORLEVEL% neq 0 (
	echo Failure building Nuget package
	cd "%SELF%"
	exit /b -1
)

cd "%SELF%"
echo SUCCESS. Nuget package at %SELF%\build\nuget

exit /b 0

:build_peppersharp

if exist "%SELF%\..\..\PepperSharp\bin\%FLAVOR%\Xamarin.PepperSharp.dll" exit /b 0

call msbuild  /t:build /p:Platform=AnyCPU /p:Configuration=%FLAVOR% "%SELF%\..\..\PepperSharp\PepperSharp.csproj"
echo Finished building PepperSharp

popd
exit /b 0