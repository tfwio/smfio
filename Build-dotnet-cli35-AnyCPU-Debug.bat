@echo off
SET SNKSIZE=4096
SET SNKN=smfio
SET PLAT=AnyCPU
SET CONF=Debug
SET CNF=%PLAT%-%CONF%

IF NOT EXIST smfio\.src\smfio.snk (
  pushd smfio\.src > nul
    echo - Generate Strong Name Key Pair ^(%SNKN%.snk^)
    sn -k %SNKSIZE% %SNKN%.snk > nul
    echo - Generate Public Key ^(%SNKN%-public.snk^)
    sn -p %SNKN%.snk %SNKN%-public.snk sha512 > nul
    echo - Public Key Information
    sn -tp %SNKN%-public.snk
  popd > nul
)

REM set msbuild_path=C:\Program Files (x86)\msbuild\14.0\bin
set msbuild_path=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%PATH%;%msbuild_path%
msbuild /m ".sln\\smfio.sln" "/t:smfio_view" "/p:Platform=Any CPU;Configuration=Debug"
pause
