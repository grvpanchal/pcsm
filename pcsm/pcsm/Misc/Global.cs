using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pcsm
{
    public class Global
    {
        // Settings
        public static string blbConf = "blb\\Bleachbit.ini";
        public static string defragConf = "settings\\defragsettings.ini";
        public static string regCleanConf = "settings\\regsections.ini";
        public static string settingsfile = "settings\\downloadsettings.ini";
        public static string taskConf = "settings\\TaskSettings.ini";

        // Exec Files
        public static string blbExec = "blb\\bleachbit_console.exe";
        public static string defragExec = "ud\\udefrag.exe";
        public static string regCleanExec = "lrc\\lrc.exe";

        public static string program = "";
        public static string defragmented = "";
        public static string[] fragmentedp = { "0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9" };
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
        public static bool serviceanalysed = false;
        public static List<string> cleanerslist;
        public static string cleanertext;
    }
}
