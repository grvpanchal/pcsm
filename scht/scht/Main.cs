using Microsoft.Win32.TaskScheduler;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace scht
{
    public partial class Main : Form
    {
        public Main() 
        {
            InitializeComponent();
        }

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
            WritePrivateProfileString(Section, Key, Value, settingsfile);
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
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, settingsfile);
            return temp.ToString();
        }

        public static void IniDeleteSection(string path, string Section = null)
        {
            IniWriteValue(path, Section, null, null);
        }
        #endregion

        public static string system = Environment.GetEnvironmentVariable("windir") + "\\pcsm\\";
        public static string taskConf = "settings\\TaskSettings.ini";
        public static string settingsfile = "settings\\downloadsettings.ini";
        public static string taskFile = system + taskConf;

        public void WriteTask()
        {
            string taskFile = system + taskConf;
            IniWriteValue(taskFile, "1", "name", "Default Task");
            IniWriteValue(taskFile, "1", "enabled", "1");
            IniWriteValue(taskFile, "1", "typeid", "1");
            IniWriteValue(taskFile, "1", "typename", "Registry cleanup, Disk cleanup and Disk Defrag");
        }

        private void schtime(String time)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "This is a default Performance maintainer task with registry cleaning, disk cleanup and disk defrag";

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
                    td.Triggers.Add(new MonthlyTrigger(1, monthsOfYear: MonthsOfTheYear.AllMonths));
                }

                String os = System.Environment.OSVersion.Version.Major.ToString();                
                int osn = Convert.ToInt32(os);
                if (osn > 5)
                {   
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                }
                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction("c:\\windows\\pcsm\\pcsmwin.exe", " /S ", "c:\\windows\\pcsm"));                                
                ts.RootFolder.RegisterTaskDefinition(@"Performance Maintainer Task 1", td);
                WriteTask();
                TaskFolder tf = ts.RootFolder;
                foreach (Microsoft.Win32.TaskScheduler.Task t in tf.Tasks)
                {
                    try
                    {
                        foreach (Microsoft.Win32.TaskScheduler.Trigger trg in t.Definition.Triggers)
                        {
                            if (t.Name == "Performance Maintainer Task 1")
                            {
                                IniWriteValue(taskFile, "1", "trigger", trg.ToString());
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            schtime("hourly");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            schtime("daily");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            schtime("weekly");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            schtime("Monthly");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg == "/hourly")
                {
                    schtime("hourly");
                    this.Close();
                }

                if (arg == "/daily")
                {
                    schtime("daily");
                    this.Close();
                }

                if (arg == "/weekly")
                {
                    schtime("weekly");
                    this.Close();
                }

                if (arg == "/monthly")
                {
                    schtime("monthly");
                    this.Close();
                }
            }
        }
    }
}