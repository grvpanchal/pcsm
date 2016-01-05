using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void schtime(String time)
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
                ts.RootFolder.RegisterTaskDefinition(@"Performance Maintainer", td);

                
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