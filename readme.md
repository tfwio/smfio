
SMF / MIDI Parser (reader) for DOTNET
=========================================

[SMF-1.0]: https://www.midi.org/specifications/item/the-midi-1-0-specification

ALPHA NOTES
----------------

### BIG VS LITTLE ENDIAN

Endian-ness has not been tested on LE architecture.  
Its assumed to work while possible that some strings (for humans)
may print incorrectly.  
*Help (testing) in this matter would be appreciated.*

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
