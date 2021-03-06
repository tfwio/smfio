@echo off
call build-path
SET SNKSIZE=65536
SET SNKN=smfio
IF NOT EXIST .src/smfio.snk (
  pushd .src > nul
    echo - Generate Strong Name Key Pair ^(%SNKN%.snk^)
    sn -k %SNKSIZE% %SNKN%.snk > nul
    echo - Generate Public Key ^(%SNKN%-public.snk^)
    sn -p %SNKN%.snk %SNKN%-public.snk sha512 > nul
    echo - Public Key Information
    sn -tp %SNKN%-public.snk
  popd > nul
)

msbuild /m ".sln\\smfio.sln" "/t:smfio" "/p:Platform=Any CPU;Configuration=Debug"
echo - Assembly Public Key ^(%SNKN%-public.snk^)
sn -q -tp .src/%SNKN%-public.snk
echo - Assembly Public Key ^(build/AnyCPU-Debug/smfio.dll^)
sn -q -Tp build/AnyCPU-Debug/smfio.dll
pause