// HookDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <windows.h>
#include <strsafe.h>
#include <iostream>
#include <fstream>
#include <string>

#pragma comment( lib, "user32.lib") 
#pragma comment( lib, "gdi32.lib")

HWND gh_hwndMain;
HINSTANCE hinst;
HHOOK hhk;

using namespace std;

void writeToFile(string info);
LRESULT CALLBACK KeyboardProc(int nCode, WPARAM wParam, LPARAM lParam);
LRESULT WINAPI MainWndProc(HWND hwndMain, UINT uMsg, WPARAM wParam, LPARAM lParam);

LRESULT CALLBACK KeyboardProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	writeToFile("got into the dll!");
	CHAR szBuf[128];
	HDC hdc;
	static int c = 0;
	size_t cch;
	HRESULT hResult;

	if (nCode < 0)  // do not process message 
		return CallNextHookEx(NULL, nCode,
		wParam, lParam);

	hdc = GetDC(gh_hwndMain);
	hResult = StringCchPrintf((STRSAFE_LPWSTR)szBuf, 128 / sizeof(TCHAR), (STRSAFE_LPWSTR)"KEYBOARD - nCode: %d, vk: %d, %d times ", nCode, wParam, c++);
	if (FAILED(hResult))
	{
		// TODO: write error handler
	}
	hResult = StringCchLength((LPCWSTR)szBuf, 128 / sizeof(TCHAR), &cch);
	if (FAILED(hResult))
	{
		// TODO: write error handler
	}
	TextOut(hdc, 2, 115, (LPCWSTR)szBuf, cch);
	
	// convert buffer to string
	string s(reinterpret_cast< char const* >(szBuf));

	writeToFile(s);

	ReleaseDC(gh_hwndMain, hdc);

	return CallNextHookEx(NULL, nCode, wParam, lParam);
}

LRESULT WINAPI MainWndProc(HWND hwndMain, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	std::ofstream myfile;
	myfile.open ("example.txt");
	myfile << "Writing this to a file.\n";
	myfile.close();

	writeToFile("got into mainwndproc");

	SetWindowsHookEx(WH_KEYBOARD, KeyboardProc, (HINSTANCE)NULL, GetCurrentThreadId());

	return 0;
}

LRESULT CALLBACK wireKeyboardProc(int code,WPARAM wParam,LPARAM lParam) {  
	 writeToFile("got into dll");
	 CallNextHookEx(hhk,code,wParam,lParam);
	 return 0;
}

extern "C" __declspec(dllexport) void install() { 
	//writeToFile("\n");

	hhk = SetWindowsHookEx(WH_KEYBOARD, wireKeyboardProc, hinst, NULL);
}

void writeToFile(string info) {
	ofstream myFile;
	myFile.open("log.txt");
	myFile << info;

	myFile.close();
}
