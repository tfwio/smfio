
SMF / MIDI Parser/Reader in CSHARP
=========================================

[SMF]: https://www.midi.org/specifications/item/the-midi-1-0-specification

ALPHA NOTES
----------------

### BIG VS LITTLE ENDIAN

Hasn't been tested on little-endian architecture.

Usage Example
------

See branch: smfio.view  
https://github.com/tfwio/smfio/tree/smfio.view

BUILD (windows)
------------------

Its suggested that you build via instruction on smfio.view branch
since it provides the only current usage example.  

There is a particular **PRE-BUILD** step (using python3)
the CSPROJ uses to generate `.src/Properties/AssemblyInfo.cs`.

If needed, tell the `asm-nfo.bat` where to find python.exe (python3).

```cmd
#! cmd.exe /c
pushd %~dp0
"%LOCALAPPDATA%\Programs\Python\Python36\python.exe" "get-sha"
popd
```

> the `#! cmd.exe /c` was asserted at the beginning of the file
  so that I could run the command from msys2's bash.

Similarly you may need adjust the main build scripts so that
they have no problem finding msbuild.  
Its currently configured to build using the default location
of Microsoft Visual Studio 2017 Community.

```cmd
@echo off
REM set msbuild_path=C:\Program Files (x86)\msbuild\14.0\bin
set msbuild_path=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%PATH%;%msbuild_path%
msbuild ".sln\\smfio.sln" "/t:smfio" "/p:Platform=Any CPU;Configuration=Debug"
```
