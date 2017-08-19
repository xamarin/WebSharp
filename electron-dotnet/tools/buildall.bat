@echo off
call %~dp0\build.bat release 1.4.0 6.5.0
call %~dp0\build.bat release 1.5.0 7.0.0 7.4.0
call %~dp0\build.bat release 1.6.0 7.4.0
call %~dp0\build.bat release 1.7.0 7.9.0