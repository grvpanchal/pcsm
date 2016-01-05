using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using Shell32;
using Microsoft.Win32.TaskScheduler;

namespace pcsw
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        #region GlobalVariables
        public class Global
        {
            public static bool startupset = false;
            public static bool serviceset = false;
            public static bool schtset = false;

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

        public void IniWriteValue(string path, string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }

        public string IniReadValue(string path, string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }
        #endregion

        public string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        public void checkwriteINI(bool check, string section, string key)
        {
            if (check == true)
            {
                IniWriteValue("c:\\val.ini", section, key, "true");
            }

            else
            {
                IniWriteValue("c:\\val.ini", section, key, "false");
            }
         
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);


        private void button1_Click(object sender, EventArgs e)
        {
            bool is64bit = !string.IsNullOrEmpty(
    Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
            if (is64bit)
            {
                IntPtr ptr = new IntPtr();
                bool isWow64FsRedirectionDisabled = Wow64DisableWow64FsRedirection(ref ptr);
            }
            if (tabControl1.SelectedIndex < tabControl1.TabCount - 1)
            {
                tabControl1.SelectedIndex++;
            }

            #region Tab1
            if (tabControl1.SelectedIndex == 1)
            {
                dccheck.Checked = true;
            }
            #endregion 

            #region Tab2
            if (tabControl1.SelectedIndex == 2)
            {
                chkdskcheck.Checked = true;
            }
            #endregion

            #region Tab3
            if (tabControl1.SelectedIndex == 3)
            {
                regccheck.Checked = true;
                regdcheck.Checked = true;
            }
            #endregion

            #region Tab4
            if (tabControl1.SelectedIndex == 4 && Global.startupset != true)
            {
                
                RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,RegistryView.Registry64);
                RegistryKey root = localKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion");
                using (RegistryKey key = root.OpenSubKey("Run"))
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        this.dataGridView1.Rows.Add(true, valueName, key.GetValue(valueName).ToString());
                    }
                }

                localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                root = localKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion");
                using (RegistryKey key = root.OpenSubKey("Run"))
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        this.dataGridView1.Rows.Add(true, valueName, key.GetValue(valueName).ToString());
                    }
                }
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) == ".lnk")
                    {
                        string link = GetShortcutTargetFile(file);
                        
                        this.dataGridView1.Rows.Add(true, Path.GetFileNameWithoutExtension(file) , link.ToString());
                    }
                }

                files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup));
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) == ".lnk")
                    {
                        string link = GetShortcutTargetFile(file);

                        this.dataGridView1.Rows.Add(true, Path.GetFileNameWithoutExtension(file), link.ToString());
                    }
                }
                Global.startupset = true;


            }
            #endregion

            #region Tab5

            if (tabControl1.SelectedIndex == 5 && Global.serviceset != true)
            {
                ServiceController[] scServices;
                scServices = ServiceController.GetServices(System.Environment.MachineName);

            foreach (ServiceController scTemp in scServices)
            {
                Regex r = new Regex(@"("".*?"")|(\S+)");
                MatchCollection mc = r.Matches(scTemp.ImagePath);
                string val = mc[0].ToString();
                  //  MessageBox.Show(mc[0].ToString());
                string sub = val.Substring(0, 1);
                if (sub == "\"")
                {
                    val = val.Substring(1, val.Length - 1);
                }
                if (val.GetLast(1) == "\"")
                {

                    val = val.Substring(0, val.Length - 1);
                }
                if (val.GetLast(4) != ".exe" && val.GetLast(4) != ".EXE")
                {
                    
                       val = val + ".exe";
                }

                
                
                if (File.Exists(val))
                {
                    FileVersionInfo myFile = FileVersionInfo.GetVersionInfo(val);
                    if (myFile.CompanyName != "Microsoft Corporation")
                    {
                        this.dataGridView2.Rows.Add(true, scTemp.DisplayName, myFile.CompanyName, scTemp.Status);
                    }
                    else if (myFile.CompanyName == "Microsoft Corporation" && String.Compare(val.Substring(0, 16), @"c:\Program files", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        this.dataGridView2.Rows.Add(true, scTemp.DisplayName, myFile.CompanyName, scTemp.Status);
                    }
                }
                else
                {
                    
                    this.dataGridView2.Rows.Add(true, scTemp.DisplayName, "");
                }
            }
            Global.serviceset = true;
            }
            #endregion 

            #region Tab6
            if (tabControl1.SelectedIndex == 6 && Global.schtset != true)
            {
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string computername = System.Environment.MachineName;
                TaskService ts = new TaskService();
                TaskFolder tf = ts.RootFolder;
                foreach (Microsoft.Win32.TaskScheduler.Task t in tf.Tasks)
                {
                    try
                    {

                        foreach (Microsoft.Win32.TaskScheduler.Trigger trg in t.Definition.Triggers)
                        {
                            MessageBox.Show(trg.ToString());
                            if (checkBox1.Checked != true)
                            {
                                if (trg.ToString() == "Run at system startup" || trg.ToString() == "At system startup" || trg.ToString() == "At log on of " + computername + "\\" + userName || trg.ToString() == "Run at user logon")
                                {
                                    this.dataGridView3.Rows.Add(true, t.Name, trg.ToString());
                                    break;
                                }
                            }
                            else
                            {
                                 this.dataGridView3.Rows.Add(true, t.Name, trg.ToString());
                                    break;
                            }

                        }

                    }
                    catch { }
                }

                /*ScheduledTasks st = new ScheduledTasks();
                foreach (string taskName in st.GetTaskNames())
                {
                    using (TaskScheduler.Task task = st.OpenTask(taskName))
                    {
                        if (task != null)
                        {
                            foreach (TaskScheduler.Trigger tr in task.Triggers)
                            {
                                if (tr is OnSystemStartTrigger || tr is OnLogonTrigger)
                                {
                                    //  Do something, such as log the name, or store the task for later
                                    this.dataGridView3.Rows.Add(true, task.Name, "");
                                    //  break out and move to the next task
                                    break;
                                }
                            }
                        }
                    }
                }*/
                Global.schtset = true;
            }
            #endregion

            #region Tab7
            if (tabControl1.SelectedIndex == 7)
            {
                defragcheck.Checked = true;
            }
            #endregion

            #region Tab8
            if (tabControl1.SelectedIndex == 8)
            {
                
            }
            #endregion

            #region Tab9
            if (tabControl1.SelectedIndex == 9)
            {
                File.Delete("c:\\val.ini");

                checkwriteINI(dccheck.Checked, "DiskClean", "DiskCleanStatus");
                checkwriteINI(chkdskcheck.Checked, "ScanDisk", "chkdskStatus");
                checkwriteINI(regccheck.Checked, "Registry", "RegCleanerStatus");
                checkwriteINI(regdcheck.Checked, "Registry", "RegDefragStatus");
                checkwriteINI(srcheck.Checked, "System", "SystemRestoreStatus");
                checkwriteINI(ischeck.Checked, "System", "IndexingServiceStatus");
                checkwriteINI(aerocheck.Checked, "System", "AeroStatus");
                checkwriteINI(defragcheck.Checked, "Defragment", "DefragStatus");
                
                int j = 0;


                foreach (DataGridViewRow dgvR in dataGridView1.Rows)
                {

                    string val = dgvR.Cells["Column1"].FormattedValue.ToString();
                    if (val == "False")
                    {
                        IniWriteValue("c:\\val.ini", "Startups", "stu" + j, dgvR.Cells["Column2"].FormattedValue.ToString());
                        j++;
                    }

                }
                IniWriteValue("c:\\val.ini", "Startups", "stuTotal", "" + j);
                j = 0;

                foreach (DataGridViewRow dgvR in dataGridView2.Rows)
                {
                    string val = dgvR.Cells["srvColumn1"].FormattedValue.ToString();
                    if (val == "False")
                    {
                        IniWriteValue("c:\\val.ini", "Services", "srv" + j, dgvR.Cells["srvColumn2"].FormattedValue.ToString());
                        j++;
                    }

                }

                IniWriteValue("c:\\val.ini", "Services", "srvTotal", "" + j);
                j = 0;
            }
            #endregion

            #region Tab10
            if (tabControl1.SelectedIndex == 10)
            {
                
            }
            #endregion
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex > 0)
            {
                tabControl1.SelectedIndex--;
            }

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
        
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string computername = System.Environment.MachineName;
            TaskService ts = new TaskService();
            TaskFolder tf = ts.RootFolder;
            foreach (Microsoft.Win32.TaskScheduler.Task t in tf.Tasks)
            {
                try
                {

                    foreach (Microsoft.Win32.TaskScheduler.Trigger trg in t.Definition.Triggers)
                    {
                        
                        if (checkBox1.Checked != true)
                        {
                            if (trg.ToString() == "Run at system startup" || trg.ToString() == "At system startup" || trg.ToString() == "At log on of " + computername + "\\" + userName || trg.ToString() == "Run at user logon")
                            {
                                
                                this.dataGridView3.Rows.Add(true, t.Name, trg.ToString());
                                break;
                            }
                        }
                        else
                        {
                            
                            this.dataGridView3.Rows.Add(true, t.Name, trg.ToString());
                            break;
                        }

                    }

                }
                catch { }
            }
        }
           

        
    }

    

}
