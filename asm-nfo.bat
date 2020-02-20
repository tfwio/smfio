@ECHO OFF
PUSHD %~dp0
  CALL :DOTHIS Python36
  CALL :DOTHIS Python37
POPD
PAUSE
GOTO:EOF


:DOTHIS
  if EXIST "%LOCALAPPDATA%\Programs\Python\%~1" "%LOCALAPPDATA%\Programs\Python\%~1\python.exe" "get-sha"
  GOTO:EOF

