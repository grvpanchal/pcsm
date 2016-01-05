using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32.TaskScheduler;
using System.Net;
using System.Management;

namespace pcsm
{


public static class OSVersion
    {

        public static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }

        public static bool is64BitProcess = (IntPtr.Size == 8);
        public static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }

        #region PInvoke Signatures
        public const byte VER_NT_WORKSTATION = 1;
        public const byte VER_NT_DOMAIN_CONTROLLER = 2;
        public const byte VER_NT_SERVER = 3;

        public const ushort VER_SUITE_SMALLBUSINESS = 1;
        public const ushort VER_SUITE_ENTERPRISE = 2;
        public const ushort VER_SUITE_TERMINAL = 16;
        public const ushort VER_SUITE_DATACENTER = 128;
        public const ushort VER_SUITE_SINGLEUSERTS = 256;
        public const ushort VER_SUITE_PERSONAL = 512;
        public const ushort VER_SUITE_BLADE = 1024;
        public const ushort VER_SUITE_WH_SERVER = 32768;

        public const uint PRODUCT_UNDEFINED = 0x00000000;
        public const uint PRODUCT_ULTIMATE = 0x00000001;
        public const uint PRODUCT_HOME_BASIC = 0x00000002;
        public const uint PRODUCT_HOME_PREMIUM = 0x00000003;
        public const uint PRODUCT_ENTERPRISE = 0x00000004;
        public const uint PRODUCT_HOME_BASIC_N = 0x00000005;
        public const uint PRODUCT_BUSINESS = 0x00000006;
        public const uint PRODUCT_STANDARD_SERVER = 0x00000007;
        public const uint PRODUCT_DATACENTER_SERVER = 0x00000008;
        public const uint PRODUCT_SMALLBUSINESS_SERVER = 0x00000009;
        public const uint PRODUCT_ENTERPRISE_SERVER = 0x0000000A;
        public const uint PRODUCT_STARTER = 0x0000000B;
        public const uint PRODUCT_DATACENTER_SERVER_CORE = 0x0000000C;
        public const uint PRODUCT_STANDARD_SERVER_CORE = 0x0000000D;
        public const uint PRODUCT_ENTERPRISE_SERVER_CORE = 0x0000000E;
        public const uint PRODUCT_ENTERPRISE_SERVER_IA64 = 0x0000000F;
        public const uint PRODUCT_BUSINESS_N = 0x00000010;
        public const uint PRODUCT_WEB_SERVER = 0x00000011;
        public const uint PRODUCT_CLUSTER_SERVER = 0x00000012;
        public const uint PRODUCT_HOME_SERVER = 0x00000013;
        public const uint PRODUCT_STORAGE_EXPRESS_SERVER = 0x00000014;
        public const uint PRODUCT_STORAGE_STANDARD_SERVER = 0x00000015;
        public const uint PRODUCT_STORAGE_WORKGROUP_SERVER = 0x00000016;
        public const uint PRODUCT_STORAGE_ENTERPRISE_SERVER = 0x00000017;
        public const uint PRODUCT_SERVER_FOR_SMALLBUSINESS = 0x00000018;
        public const uint PRODUCT_SMALLBUSINESS_SERVER_PREMIUM = 0x00000019;
        public const uint PRODUCT_HOME_PREMIUM_N = 0x0000001A;
        public const uint PRODUCT_ENTERPRISE_N = 0x0000001B;
        public const uint PRODUCT_ULTIMATE_N = 0x0000001C;
        public const uint PRODUCT_WEB_SERVER_CORE = 0x0000001D;
        public const uint PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT = 0x0000001E;
        public const uint PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY = 0x0000001F;
        public const uint PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING = 0x00000020;
        public const uint PRODUCT_SERVER_FOR_SMALLBUSINESS_V = 0x00000023;
        public const uint PRODUCT_STANDARD_SERVER_V = 0x00000024;
        public const uint PRODUCT_ENTERPRISE_SERVER_V = 0x00000026;
        public const uint PRODUCT_STANDARD_SERVER_CORE_V = 0x00000028;
        public const uint PRODUCT_ENTERPRISE_SERVER_CORE_V = 0x00000029;
        public const uint PRODUCT_HYPERV = 0x0000002A;

        public const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        public const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        public const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        public const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        public const int SM_SERVERR2 = 89;

        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFOEX
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public ushort wServicePackMajor;
            public ushort wServicePackMinor;
            public ushort wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public uint wProcessorArchitecture;
            public uint wReserved;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }

        [DllImport("Kernel32.dll")]
        internal static extern bool GetProductInfo(
           uint osMajorVersion,
           uint osMinorVersion,
           uint spMajorVersion,
           uint spMinorVersion,
           out uint edition);

        [DllImport("kernel32.dll")]
        internal static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(int nIndex);
        #endregion

        public static string GetOSVersion()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
            osVersionInfo.dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(OSVERSIONINFOEX));

            SYSTEM_INFO systemInfo = new SYSTEM_INFO();
            GetSystemInfo(ref systemInfo);

            string osName = "Microsoft ";

            if (!GetVersionEx(ref osVersionInfo))
                return string.Empty;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32Windows:
                    {
                        switch (osVersionInfo.dwMajorVersion)
                        {
                            case 4:
                                {
                                    switch (osVersionInfo.dwMinorVersion)
                                    {
                                        case 0:
                                            if (osVersionInfo.szCSDVersion == "B" ||
                                                osVersionInfo.szCSDVersion == "C")
                                                osName += "Windows 95 R2";
                                            else
                                                osName += "Windows 95";
                                            break;
                                        case 10:
                                            if (osVersionInfo.szCSDVersion == "A")
                                                osName += "Windows 98 SE";
                                            else
                                                osName += "Windows 98";
                                            break;
                                        case 90:
                                            osName += "Windows ME";
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case PlatformID.Win32NT:
                    {
                        switch (osVersionInfo.dwMajorVersion)
                        {
                            case 3:
                                osName += "Windows NT 3.5.1";
                                break;

                            case 4:
                                switch (osVersionInfo.wProductType)
                                {
                                    case 1:
                                        osName += "Windows NT 4.0";
                                        break;
                                    case 3:
                                        osName += "Windows NT 4.0 Server";
                                        break;
                                }
                                break;

                            case 5:
                                {
                                    switch (osVersionInfo.dwMinorVersion)
                                    {
                                        case 0:
                                            osName += "Windows 2000";
                                            break;
                                        case 1:
                                            osName += "Windows XP";
                                            break;
                                        case 2:
                                            {
                                                if (osVersionInfo.wSuiteMask == VER_SUITE_WH_SERVER)
                                                    osName += "Windows Home Server";
                                                else if (osVersionInfo.wProductType == VER_NT_WORKSTATION &&
                                                        systemInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                                                    osName += "Windows XP Professional";
                                                else
                                                    osName += GetSystemMetrics(SM_SERVERR2) == 0 ? "Windows Server 2003" : "Windows Server 2003 R2";
                                            }
                                            break;
                                    }

                                }
                                break;

                            case 6:
                                {
                                    switch (osVersionInfo.dwMinorVersion)
                                    {
                                        case 0:
                                            osName += osVersionInfo.wProductType == VER_NT_WORKSTATION ? "Windows Vista" : "Windows Server 2008";
                                            break;

                                        case 1:
                                            osName += osVersionInfo.wProductType == VER_NT_WORKSTATION ? "Windows 7" : "Windows Server 2008 R2";
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            osName += " ";

            switch (osVersionInfo.dwMajorVersion)
            {
                case 4:
                    {
                        switch (osVersionInfo.wProductType)
                        {
                            case VER_NT_WORKSTATION:
                                osName += "Workstation";
                                break;

                            case VER_NT_SERVER:
                                osName += (osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) != 0 ? "Enterprise Server" : "Standard Server";
                                break;
                        }
                    }
                    break;

                case 5:
                    {
                        switch (osVersionInfo.wProductType)
                        {
                            case VER_NT_WORKSTATION:
                                osName += (osVersionInfo.wSuiteMask & VER_SUITE_PERSONAL) != 0 ? "Home" : "Professional";
                                break;

                            case VER_NT_SERVER:
                                {
                                    switch (osVersionInfo.dwMinorVersion)
                                    {
                                        case 0:
                                            {
                                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) != 0)
                                                    osName += "Data Center Server";
                                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) != 0)
                                                    osName += "Advanced Server";
                                                else
                                                    osName += "Server";
                                            }
                                            break;

                                        default:
                                            {
                                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) != 0)
                                                    osName += "Data Center Server";
                                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) != 0)
                                                    osName += "Enterprise Server";
                                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_BLADE) != 0)
                                                    osName += "Web Edition";
                                                else
                                                    osName += "Standard Server";
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case 6:
                    {
                        uint ed;
                        if (GetProductInfo(osVersionInfo.dwMajorVersion, osVersionInfo.dwMinorVersion, osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor, out ed))
                        {
                            switch (ed)
                            {
                                case PRODUCT_BUSINESS:
                                    osName += "Business";
                                    break;
                                case PRODUCT_BUSINESS_N:
                                    osName += "Business N";
                                    break;
                                case PRODUCT_CLUSTER_SERVER:
                                    osName += "HPC Edition";
                                    break;
                                case PRODUCT_DATACENTER_SERVER:
                                    osName += "Data Center Server";
                                    break;
                                case PRODUCT_DATACENTER_SERVER_CORE:
                                    osName += "Data Center Server Core";
                                    break;
                                case PRODUCT_ENTERPRISE:
                                    osName += "Enterprise";
                                    break;
                                case PRODUCT_ENTERPRISE_N:
                                    osName += "Enterprise N";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER:
                                    osName += "Enterprise Server";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE:
                                    osName += "Enterprise Server Core Installation";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE_V:
                                    osName += "Enterprise Server Without Hyper-V Core Installation";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_IA64:
                                    osName += "Enterprise Server For Itanium Based Systems";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_V:
                                    osName += "Enterprise Server Without Hyper-V";
                                    break;
                                case PRODUCT_HOME_BASIC:
                                    osName += "Home Basic";
                                    break;
                                case PRODUCT_HOME_BASIC_N:
                                    osName += "Home Basic N";
                                    break;
                                case PRODUCT_HOME_PREMIUM:
                                    osName += "Home Premium";
                                    break;
                                case PRODUCT_HOME_PREMIUM_N:
                                    osName += "Home Premium N";
                                    break;
                                case PRODUCT_HYPERV:
                                    osName += "Hyper-V Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT:
                                    osName += "Essential Business Management Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING:
                                    osName += "Essential Business Messaging Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY:
                                    osName += "Essential Business Security Server";
                                    break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS:
                                    osName += "Essential Server Solutions";
                                    break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS_V:
                                    osName += "Essential Server Solutions Without Hyper-V";
                                    break;
                                case PRODUCT_SMALLBUSINESS_SERVER:
                                    osName += "Small Business Server";
                                    break;
                                case PRODUCT_STANDARD_SERVER:
                                    osName += "Standard Server";
                                    break;
                                case PRODUCT_STANDARD_SERVER_CORE:
                                    osName += "Standard Server Core Installation";
                                    break;
                                case PRODUCT_STANDARD_SERVER_CORE_V:
                                    osName += "Standard Server Without Hyper-V Core Installation";
                                    break;
                                case PRODUCT_STANDARD_SERVER_V:
                                    osName += "Standard Server Without Hyper-V";
                                    break;
                                case PRODUCT_STARTER:
                                    osName += "Starter";
                                    break;
                                case PRODUCT_STORAGE_ENTERPRISE_SERVER:
                                    osName += "Enterprise Storage Server";
                                    break;
                                case PRODUCT_STORAGE_EXPRESS_SERVER:
                                    osName += "Express Storage Server";
                                    break;
                                case PRODUCT_STORAGE_STANDARD_SERVER:
                                    osName += "Standard Storage Server";
                                    break;
                                case PRODUCT_STORAGE_WORKGROUP_SERVER:
                                    osName += "Workgroup Storage Server";
                                    break;
                                case PRODUCT_UNDEFINED:
                                    break;
                                case PRODUCT_ULTIMATE:
                                    osName += "Ultimate";
                                    break;
                                case PRODUCT_ULTIMATE_N:
                                    osName += "Ultimate N";
                                    break;
                                case PRODUCT_WEB_SERVER:
                                    osName += "Web Server";
                                    break;
                                case PRODUCT_WEB_SERVER_CORE:
                                    osName += "Web Server Core Installation";
                                    break;
                            }
                        }
                    }
                    break;
            }

            // If 64 bit OS -> Append (x64)
            if (is64BitProcess)
                osName += " (x64)";
            else
                // Otherwise (x86)
                osName += " (x86)";

            return osName;
        }
    }


    public class PCS
    {

        
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                using (var stream = client.OpenRead("http://sourceforge.net"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #region process


        public static void process( string filename , string arguments , bool hidden)
        {
            try
            {                

                Process SomeProgram = new Process();
                SomeProgram.StartInfo.FileName =  filename;
                SomeProgram.StartInfo.Arguments = arguments;
                if (hidden == true)
                {
                    SomeProgram.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    SomeProgram.StartInfo.CreateNoWindow = true;
                    SomeProgram.StartInfo.UseShellExecute = false;
                }
                SomeProgram.StartInfo.RedirectStandardOutput = false;
                SomeProgram.Start();
                int i = 0;
                while (!SomeProgram.HasExited)
                {
                    i++;
                    if (i == 150000)
                    {
                        SomeProgram.Kill();
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(6);

                }
                string SomeProgramOutput = SomeProgram.StandardOutput.ReadToEnd();
            }
            catch (Exception)
            {
                // Log the exception
            }
 
        }
        #endregion

        #region ReadWriteINI
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);

        public static void IniWriteValue(string path, string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }

        public static void IniWriteValue(string Section, string Key, string Value)
        {            
            WritePrivateProfileString(Section, Key, Value, Global.settingsfile);
        }


        public static string IniReadValue(string path, string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }

        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, Global.settingsfile);
            return temp.ToString();
        }
        #endregion

        #region Getlastfileindirectory
        public string GetLastFileInDirectory(string directory, string pattern = "*.*")
        {
            if (directory.Trim().Length == 0)
                return string.Empty; //Error handler can go here

            if ((pattern.Trim().Length == 0) || (pattern.Substring(pattern.Length - 1) == "."))
                return string.Empty; //Error handler can go here

            if (Directory.GetFiles(directory, pattern).Length == 0)
                return string.Empty; //Error handler can go here

            //string pattern = "*.txt"

            var dirInfo = new DirectoryInfo(directory);
            var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

            return file.ToString();
        }
        #endregion

        #region findword
        public String findword(string fileName, string searchKeyword, int cut)
        {

            string[] textLines = File.ReadAllLines(fileName);
            List<string> results = new List<string>();

            foreach (string line in textLines)
            {
                if (line.Contains(searchKeyword))
                {
                    results.Add(line);
                    String fragmented = line.Substring(cut);
                    return fragmented;
                }
            }
            return "";
        }
        #endregion

        #region Scheduled Task

        public void schtime(String time)
        {

            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "reg, disk, defrag";
                if (time == "hourly")
                {
                    DailyTrigger dt = new DailyTrigger();
                    dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(10);
                    dt.DaysInterval = 1;
                    dt.Repetition.Interval = TimeSpan.FromMinutes(60); // Default is TimeSpan.Zero (or never)
                    // Set the time the task will repeat to 1 day.
                    dt.Repetition.Duration = TimeSpan.FromDays(1); // Default is TimeSpan.Zero (or never)


                    td.Triggers.Add(dt);

                }


                if (time == "daily")
                {
                    td.Triggers.Add(new DailyTrigger());
                }

                if (time == "weekly")
                {
                    td.Triggers.Add(new WeeklyTrigger());
                }

                if (time == "monthly")
                {
                    td.Triggers.Add(new MonthlyDOWTrigger());
                }

                String os = System.Environment.OSVersion.Version.Major.ToString();
                int osn = Convert.ToInt32(os);
                if (osn > 5)
                {
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                }
                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(Global.system + "pcsmwin.exe", " /S ", "c:\\windows\\pcsm"));
                ts.RootFolder.RegisterTaskDefinition(@"Performance Maintainer", td);


            }
        }
        #endregion        

        public static string ConvertSizeToString(long Length)
        {
            if (Length < 0)
                return "";

            float nSize;
            string strSizeFmt, strUnit = "";

            if (Length < 1000)             // 1KB
            {
                nSize = Length;
                strUnit = " B";
            }
            else if (Length < 1000000)     // 1MB
            {
                nSize = Length / (float)0x400;
                strUnit = " KB";
            }
            else if (Length < 1000000000)   // 1GB
            {
                nSize = Length / (float)0x100000;
                strUnit = " MB";
            }
            else
            {
                nSize = Length / (float)0x40000000;
                strUnit = " GB";
            }

            if (nSize == (int)nSize)
                strSizeFmt = nSize.ToString("0");
            else if (nSize < 10)
                strSizeFmt = nSize.ToString("0.00");
            else if (nSize < 100)
                strSizeFmt = nSize.ToString("0.0");
            else
                strSizeFmt = nSize.ToString("0");

            return strSizeFmt + strUnit;
        }


    }

    public class Global
    {
        public static string program = "";
        public static string defragmented = "";
        public static string[] fragmentedp ={ "0","0.1","0.2","0.3","0.4","0.5","0.6","0.7","0.8","0.9"};
        public static string registry = "";
        public static string system = Environment.GetEnvironmentVariable("windir") + "\\pcsm\\";
        public static bool silent = false;
        public static int registryerrors;
        public static string cleanupsize;
        public static double newRegistrySize;
        public static double oldRegistrySize;
        public static string currentprocess;
        public static string diskdefragprogress;
        public static string diskdefragdrive;
        public static string compactprocess;
        public static bool compactstatus = false;
        public static string settingsfile = "settings\\downloadsettings.ini";
        public static bool serviceanalysed = false;
        public static List<string> cleanerslist;
        public static string cleanertext; 
    }


}
