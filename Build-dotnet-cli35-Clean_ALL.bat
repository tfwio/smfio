#! cmd.exe /c
@echo off
REM set msbuild_path=C:\Program Files (x86)\msbuild\14.0\bin
set msbuild_path=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%PATH%;%msbuild_path%
msbuild /m ".sln\\smfio.sln" "/t:smfio:Clean" "/p:Platform=Any CPU;Configuration=Release"
msbuild /m ".sln\\smfio.sln" "/t:smfio:Clean" "/p:Platform=Any CPU;Configuration=Debug"
msbuild /m ".sln\\smfio.sln" "/t:smfio:Clean" "/p:Platform=Win32;Configuration=Release"
msbuild /m ".sln\\smfio.sln" "/t:smfio:Clean" "/p:Platform=Win32;Configuration=Debug"
msbuild /m ".sln\\smfio.sln" "/t:smfio_view:Clean" "/p:Platform=Any CPU;Configuration=Release"
msbuild /m ".sln\\smfio.sln" "/t:smfio_view:Clean" "/p:Platform=Any CPU;Configuration=Debug"
