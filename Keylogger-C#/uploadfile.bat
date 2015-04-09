@echo off
echo user kglosse> log.txt
echo kglosseCSET3100>> log.txt
echo bin>> log.txt
echo put %1>> log.txt
echo cd /home/kglosse/cset4850/logs/>> log.txt
echo quit>> log.txt
ftp -n -s: ftpcmd.dat et791.eng.utoledo.edu
del log.txt