﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;

namespace pcsm
{
    static public class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }

    static public class SingleInstance
{
    public static readonly int WM_SHOWFIRSTINSTANCE =
        WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
    static Mutex mutex;
    static public bool Start()
    {
        bool onlyInstance = false;
        string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

        // if you want your app to be limited to a single instance
        // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
        // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);
        
        mutex = new Mutex(true, mutexName, out onlyInstance);
        return onlyInstance;
    }
    static public void ShowFirstInstance()
    {
        WinApi.PostMessage(
            (IntPtr)WinApi.HWND_BROADCAST,
            WM_SHOWFIRSTINSTANCE,
            IntPtr.Zero,
            IntPtr.Zero);
    }
    static public void Stop()
    {
        mutex.ReleaseMutex();
    }
}

    static public class ProgramInfo
{
      static public string AssemblyGuid
      {
        get
        {
            object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            if (attributes.Length == 0) {
                return String.Empty;
            }
            return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
        }
      }
}

    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Main mainForm = new Main();                
                SplashScreen.ShowSplashScreen();
                Application.DoEvents();
                SplashScreen.SetStatus("Checking for Updates...");                
                mainForm.check_update();
                SplashScreen.SetStatus("Verifying Installed Programs...");
                System.Threading.Thread.Sleep(500);                
                SplashScreen.SetStatus("Checking System Informantion...");
                System.Threading.Thread.Sleep(500);
                mainForm.get_system_info();              
                Application.Run(mainForm);
                
            }
            catch (Exception e)
            {                
                MessageBox.Show(e.Message);
            }

            SingleInstance.Stop();
        }
    }
}
