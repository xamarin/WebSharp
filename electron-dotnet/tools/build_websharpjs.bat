@echo off
set SELF=%~dp0

set FLAVOR=%1

if "%FLAVOR%" equ "" set FLAVOR=Release

if not exist "%SELF%\build\download.exe" (
	csc /out:"%SELF%\build\download.exe" "%SELF%\download.cs"
)

if not exist "%SELF%\build\nuget.exe" (
	"%SELF%\build\download.exe" http://nuget.org/nuget.exe "%SELF%\build\nuget.exe"
	"%SELF%\build\nuget.exe" update -self
)

cd "%SELF%\..\src\websharpjs\WebSharp.js"
dotnet restore WebSharp.js.sln /p:Configuration=%FLAVOR% /p:Platform="Any CPU"
if %ERRORLEVEL% neq 0 exit /b -1
dotnet build WebSharp.js.sln /p:Configuration=%FLAVOR% /p:Platform="Any CPU"
if %ERRORLEVEL% neq 0 exit /b -1
dotnet pack WebSharp.js.sln /p:Configuration=%FLAVOR% /p:Platform="Any CPU" /p:IncludeSymbols=true

if %ERRORLEVEL% neq 0 (
	echo Failure building Nuget package
	cd "%SELF%"
	exit /b -1
)

cd "%SELF%"
copy /y "%SELF%\..\src\websharpjs\WebSharp.js\bin\%FLAVOR%\*.nupkg" "%SELF%\build\nuget"
rem Make it available to the electron-dotnet module
copy /y "%SELF%\..\src\websharpjs\WebSharp.js\bin\%FLAVOR%\net451\*.dll" "%SELF%\..\lib\bin"
echo SUCCESS. Nuget package at %SELF%\build\nuget

exit /b 0
