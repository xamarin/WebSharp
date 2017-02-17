@echo off
set SELF=%~dp0

mkdir "%SELF%\build\nuget\content\websharp" > nul 2>&1
mkdir "%SELF%\build\nuget\lib\net451" > nul 2>&1
mkdir "%SELF%\build\nuget\lib\netstandard1.6" > nul 2>&1
mkdir "%SELF%\build\nuget\tools" > nul 2>&1
mkdir "%SELF%\..\src\websharpjs\WebSharp.js\bin\Release\net451" > nul 2>&1

if not exist "%SELF%\build\download.exe" (
	csc /out:"%SELF%\build\download.exe" "%SELF%\download.cs"
)

if not exist "%SELF%\build\nuget.exe" (
	"%SELF%\build\download.exe" http://nuget.org/nuget.exe "%SELF%\build\nuget.exe"
	"%SELF%\build\nuget.exe" update -self
)

csc /out:"%SELF%\..\src\websharpjs\WebSharp.js\bin\Release\net451\WebSharpJs.dll" /target:library "%SELF%\..\src\websharpjs\WebSharp.js\dotnet\WebSharpJs.cs"
if %ERRORLEVEL% neq 0 exit /b -1

cd "%SELF%\..\src\websharpjs\WebSharp.js"
dotnet restore
if %ERRORLEVEL% neq 0 exit /b -1
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 exit /b -1
dotnet pack --configuration Release --no-build

if %ERRORLEVEL% neq 0 (
	echo Failure building Nuget package
	cd "%SELF%"
	exit /b -1
)

cd "%SELF%"
copy /y "%SELF%\..\src\websharpjs\WebSharp.js\bin\Release\*.nupkg" "%SELF%\build\nuget"
rem Make it available to the electron-dotnet module
copy /y "%SELF%\..\src\websharpjs\WebSharp.js\bin\Release\net451\*.dll" "%SELF%\..\lib\bin"
echo SUCCESS. Nuget package at %SELF%\build\nuget

exit /b 0
