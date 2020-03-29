#! cmd.exe /c
@echo off
call build-path

SET SNKN=smfio
IF NOT EXIST .src/smfio.snk (
  pushd .src > nul
    sn -k 4096 %SNKN%.snk
    sn -p %SNKN%.snk %SNKN%-public.snk sha512
    sn -tp %SNKN%-public.snk
  popd > nul
)

msbuild /m ".sln\\smfio.sln" "/t:smfio" "/p:Platform=Any CPU;Configuration=Release"
