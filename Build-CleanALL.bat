@echo off
call build-path

pushd .src > nul
  DEL *.snk
popd > nul
msbuild ".sln\\smfio.sln" "/t:smfio:Clean" "/p:Platform=Any CPU;Configuration=Release"
msbuild ".sln\\smfio.sln" "/t:smfio:Clean" "/p:Platform=Any CPU;Configuration=Debug"
rmdir /S /Q build
pause