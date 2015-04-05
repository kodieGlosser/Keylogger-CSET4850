#include "stdafx.h"
#include <windows.h>
#include <iostream>
#include <stdio.h>
#include <string>
#include <fstream>
#include <iostream>

HINSTANCE hinst;
HHOOK hhk;

void writeToFile(std::string info);

LRESULT CALLBACK wireKeyboardProc(int code, WPARAM wParam, LPARAM lParam) {
	writeToFile("got into dll");
	CallNextHookEx(hhk, code, wParam, lParam);
	return 0;
}

extern "C" __declspec(dllexport) void install() {
	writeToFile("seomtihg");
	hhk = SetWindowsHookEx(WH_KEYBOARD, wireKeyboardProc, hinst, NULL);
}
extern "C" __declspec(dllexport) void uninstall() {
	UnhookWindowsHookEx(hhk);
}


void writeToFile(std::string info) {
	std::ofstream myFile;
	myFile.open("log.txt");
	myFile << info;

	myFile.close();
}