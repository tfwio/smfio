#! cmd.exe /c
@echo off
pushd %~dp0
"%LOCALAPPDATA%\Programs\Python\Python36\python.exe" "bootstrap"
popd
