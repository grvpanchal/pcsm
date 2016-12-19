﻿using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

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
    }
}
