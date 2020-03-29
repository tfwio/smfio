@ECHO OFF
SET MSVS_BASE=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community
SET DNET_BASE=C:\Program Files (x86)\Microsoft SDKs
SET BUILD_ENV=%MSVS_BASE%\VC\Auxiliary\Build\vcvars64.bat
SET MSBU_PATH=%MSVS_BASE%\MSBuild\15.0\Bin
SET MS_TOOLS=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools
SET PATH=%PATH%;%MSBU_PATH%;%MS_TOOLS%
