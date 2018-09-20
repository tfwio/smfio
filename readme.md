
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

Version History
------------------

Goal(s)

- consolidate, refactor
- simple(r) example(s)
- MIDI Format 1 to Format 0 (in memory)
- Writer (file export/write)

0.1.3

- re-introduce modest-vst dependencies:  
  NoteParser derived on SmfReader

0.1.2

- tempo map & state
- simplify time calculations  
  Old SampleClock MTC and IClock classes are obsoleted.
- refactor/rename params/props of what is now Events.cs  
  see: MidiEventDelegate and MidiMessageEvent (and similarly derived functions)
- using long (vs ulong) for delta pulse in reader & events

0.1.1

- Removed a lot of un-used code
- Application of more comprehensive Endian utility
- Slight change of (interface) IMidiParser and its impl,
  and possibly other interfaces/impl.
- Tempo changes are collected (albeit it needs work)
  to `List<TempoChange>`.  
  TODO: changes are expected to soon reflect in `MidiReader`'s
  (`DictionaryList<int,MidiMessage> MidiDataList`)
  or perhaps some kind of wrapper that is more
  user-friendly when it comes to yielding adequate
  timing in Samples or Seconds.  
  Or at the very least there will be examples
  showing how to obtain adequate time info.

0.1.0

- Initial Dump
- Semantic versioning starts here.
- namespace (title) and other refactoring.
- The project is from around 2005, reworked
  into a prototypical VSTHost to test the engine.
- lacking feature: lacking SetTempo message support.
