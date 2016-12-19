using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace pcsm
{
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

        public static void Process( string filename , string arguments , bool hidden)
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

        public static void IniDeleteSection(string path, string Section = null)
        {
            IniWriteValue(path, Section, null, null);
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
        public String FindWord(string fileName, string searchKeyword, int cut)
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

        public void SchTime(String time)
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
}
