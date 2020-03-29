@ECHO OFF

PUSHD %~dp0 > nul
  CALL :DOTHIS Python36
  CALL :DOTHIS Python37
  ::CALL :DOTHIS Python38
POPD > nul
PAUSE
GOTO:EOF

:DOTHIS
  if EXIST "%LOCALAPPDATA%\Programs\Python\%~1" "%LOCALAPPDATA%\Programs\Python\%~1\python.exe" "get-sha" > nul
  GOTO:EOF

