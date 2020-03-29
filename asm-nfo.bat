@echo off
pushd %~dp0
SET PYPATH=%LOCALAPPDATA%\Programs\Python\Python37
"%PYPATH%\python.exe" "bootstrap"
popd
PAUSE