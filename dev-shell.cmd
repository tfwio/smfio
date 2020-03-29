@ECHO OFF
:: C:\Program Files (x86)\msbuild\14.0\bin
:: C:\Windows\Microsoft.NET
:: x64        Native Tools-VS2017   :: BUILD_ENV=%MSVS_BASE%\VC\Auxiliary\Build\vcvars64.bat
:: x64_x86    Cross Tools-VS2017    :: BUILD_ENV=%MSVS_BASE%\VC\Auxiliary\Build\vcvarsamd64_x86.bat
:: x86 Native Tools-VS2017          :: BUILD_ENV=%MSVS_BASE%\VC\Auxiliary\Build\vcvars32.bat
:: x86_x64    Cross Tools-VS2017    :: BUILD_ENV=%MSVS_BASE%\VC\Auxiliary\Build\vcvarsx86_amd64.bat
:: 
SET MSVS_BASE=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community
SET DNET_BASE=C:\Program Files (x86)\Microsoft SDKs
SET BUILD_ENV=%MSVS_BASE%\VC\Auxiliary\Build\vcvars64.bat
SET MSBU_PATH=%MSVS_BASE%\MSBuild\15.0\Bin
SET MS_TOOLS=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools
SET PATH=%PATH%;%MSBU_PATH%;%MS_TOOLS%

cmd /E:ON /V:ON /k
:: SET PJ_CONF=Debug
:: IF NOT "%PJ_CONF%"=="" SET PJ_CONF=%~1
:: SET PJ_PLAT=Win32
:: IF NOT "%PJ_PLAT%"=="" SET PJ_PLAT=%~2
:: SET PJ_MODE=rebuild
:: IF NOT "%PJ_MODE%"=="" SET PJ_MODE=%~3
:: GOTO:EOF
:: :REPORT
::   ECHO - Configuration^: %PJ_CONF%
::   ECHO - Platform^:      %PJ_PLAT%
::   ECHO - Build-Mode^:    %PJ_MODE%
::   GOTO:EOF
:: 
