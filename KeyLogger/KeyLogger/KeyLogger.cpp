// KeyLogger.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <iostream>

int _tmain(int argc, _TCHAR* argv[])
{

	HINSTANCE hinst = LoadLibrary(_T("HookDll.dll"));
	if (hinst == NULL)
	{
		printf("null hinst");
	}
	typedef void(*Install)();
	typedef void(*Uninstall)();

	Install install = (Install)GetProcAddress(hinst, "install");
	Uninstall uninstall = (Uninstall)GetProcAddress(hinst, "uninstall");


	install();
	int foo;
	std::cin >> foo;

	uninstall();
	return 0;

}
