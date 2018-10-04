#! cmd.exe /c
@echo off
pushd %~dp0
set SMFIO_REPO=https://tfw.io/dot.io/smfio
"%LOCALAPPDATA%\Programs\Python\Python36\python.exe" "bootstrap"
popd
