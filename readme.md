SMF / MIDI Parser (reader) for DOTNET
=====================================
Reads normative [SMF-1.0] of ~1996.

[SMF-1.0]: https://www.midi.org/specifications/item/the-midi-1-0-specification

ALPHA NOTES
----------------

### BIG VS LITTLE ENDIAN

big-endian works (Windows architectures), little-endian has
yet to be looked at on such an architecture.  
*Help is welcome in this area! –as notes will be prepared to assist tackling
this simple matter.*

More consolidation, documentation and examples are needed geared towards read,
write and export of MIDI (smf) Formats 1-3 in addition to 'filtering' or applying
effects to a given channel/range.

### SET TEMPO

Needs tempo changes to project proper sample positions `HH:MM:SS.TTTTT`.

BUILDING on WINDOWS
-------------------

Clone [branch: smfio.view](https://github.com/tfwio/smfio/tree/smfio.view) for
the current list-view example.

```bash
# Clone into `./smfio.view` directory.
git clone -b smfio.view https://github.com/tfwio/smfio smfio.view

# enter the new directory
cd smfio.view

# clone the master branch here (dirname: smfio)
git clone https://github.com/tfwio/smfio
```

From this point it depends on what kind of MSBUILD configuration you're prepared for.

### Visual Studio Community 2017

Of course you would just load up the `./.sln/smfio.sln` file in devenv.

This would probably be the most productive work-environment considering this is
after all a System.Windows.Forms app.

Though I tend to use a modest machine, therefore mostly SharpDevelop and VisualStudioCode.

### BUILD SCRIPT(S)

The build scripts are configured to MSBUILD from Visual Studio 2017 Community Edition,  
Commented msbuild_path likely points to Visual Studio 2015 CE.

Check your setup and modify the batch script(s) accordingly.

```bat
@echo off
REM set msbuild_path=C:\Program Files (x86)\msbuild\14.0\bin
set msbuild_path=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%PATH%;%msbuild_path%
msbuild ".sln\\smfio.sln" "/t:smfio_view" "/p:Platform=Any CPU;Configuration=Release"
```

### Visual Studio Code

Though this is a Windows.Forms app, one might enjoy leveraging Visual Studio Code.

For convenience, there are a few tasks for building and running in the 
`.vscode` sub-directory that call on the BUILD scripts mentioned above.
