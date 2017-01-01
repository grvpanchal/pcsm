using Microsoft.Win32.TaskScheduler;
using pcsm.Processes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace pcsm.Scheduler
{
    public partial class NewTask : Form
    {   
        public NewTask(string taskid)
        {
            id = taskid;
            InitializeComponent();

            ReadRegSections();

            label2.Text = "Task " + id;
            if (PCS.IniReadValue(taskfile, id, "name") != "")
            {
                editmode = true;
                comboBox1.Visible = false;
                textBox1.Text = PCS.IniReadValue(taskfile, id, "name");
                label3.Text = PCS.IniReadValue(taskfile, id, "trigger");
                comboBox2.Text = PCS.IniReadValue(taskfile, id, "typename");
            }
            else
            {
                editmode = false;
                if (Directory.Exists(Global.system + "settings\\tasks\\" + id.ToString()))
                    Directory.Delete(Global.system + "settings\\tasks\\" + id.ToString(), true);
            }
            Directory.CreateDirectory(Global.system + "settings\\tasks\\" + id);

        }

        public static string id;
        public static string taskfile = Global.system + Global.taskConf;
        public static bool editmode = false;
        public static int n = 0;
        private TreeView _fieldsTreeCache1 = new TreeView();
        List<string> searchstring = new List<string>();

        #region Scheduled Task

        public void SchTime(String time)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = textBox1.Text;
                if (time == "hourly")
                {
                    DailyTrigger dt = new DailyTrigger();
                    dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(dateTimePicker2.Value.Hour) + TimeSpan.FromMinutes(dateTimePicker2.Value.Minute);
                    dt.DaysInterval = 1;
                    dt.Repetition.Interval = TimeSpan.FromMinutes(60); // Default is TimeSpan.Zero (or never)
                    // Set the time the task will repeat to 1 day.
                    dt.Repetition.Duration = TimeSpan.FromDays(1); // Default is TimeSpan.Zero (or never)
                    td.Triggers.Add(dt);
                }


                if (time == "daily")
                {
                    DailyTrigger dt = new DailyTrigger();
                    dt.StartBoundary = DateTime.Today + TimeSpan.FromHours(dateTimePicker2.Value.Hour) + TimeSpan.FromMinutes(dateTimePicker2.Value.Minute);
                    td.Triggers.Add(dt);
                }

                if (time == "weekly")
                {
                    WeeklyTrigger wt = new WeeklyTrigger();
                    wt.DaysOfWeek = 0;
                    if (checkBox1.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Monday;
                    }
                    if (checkBox2.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Tuesday;
                    }
                    if (checkBox3.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Wednesday;
                    }
                    if (checkBox4.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Thursday;
                    }
                    if (checkBox5.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Friday;
                    }
                    if (checkBox6.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Saturday;
                    }
                    if (checkBox7.Checked)
                    {
                        wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Sunday;
                    }
                    if (wt.DaysOfWeek == 0)
                    {
                        wt.DaysOfWeek = DaysOfTheWeek.Sunday;
                    }
                    td.Triggers.Add(wt);
                }

                if (time == "monthly")
                {
                    MonthlyTrigger mt = new MonthlyTrigger();
                    mt.StartBoundary = DateTime.Today + TimeSpan.FromHours(dateTimePicker2.Value.Hour) + TimeSpan.FromMinutes(dateTimePicker2.Value.Minute);
                    mt.DaysOfMonth = new int[] { int.Parse(comboBox3.SelectedItem.ToString()) };
                    td.Triggers.Add(mt);
                }
                if (time == "logon")
                {
                    td.Triggers.Add(new LogonTrigger());
                }
                String os = System.Environment.OSVersion.Version.Major.ToString();
                int osn = Convert.ToInt32(os);
                if (osn > 5)
                {
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                }
                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(Global.system + "pcsmwin.exe", " /S /taskid " + id, "c:\\windows\\pcsm"));
                ts.RootFolder.RegisterTaskDefinition(@"Performance Maintainer Task " + id, td);


            }
        }
        #endregion
        
        public void FilterCleaners(TreeView treeView1, List<string> searchstring, TextBox textBox1, PictureBox pictureBox1)
        {
            //blocks repainting tree till all objects loaded

            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            if (textBox1.Text != string.Empty)
            {
                pictureBox1.Image = global::pcsm.Properties.Resources.x;
                for (int x = 0; x < DiskCleaner._fieldsTreeCache.Nodes.Count; x++)
                {
                    Match m1 = Regex.Match(DiskCleaner._fieldsTreeCache.Nodes[x].Text, textBox1.Text, RegexOptions.IgnoreCase);
                    if (m1.Success)
                    {
                        for (int y = 0; y < DiskCleaner._fieldsTreeCache.Nodes[x].Nodes.Count; y++)
                        {
                            searchstring.Add(DiskCleaner._fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString());
                        }
                    }
                    else
                    {
                        for (int y = 0; y < DiskCleaner._fieldsTreeCache.Nodes[x].Nodes.Count; y++)
                        {
                            Match m = Regex.Match(DiskCleaner._fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString(), textBox1.Text, RegexOptions.IgnoreCase);
                            Match m2 = Regex.Match(DiskCleaner._fieldsTreeCache.Nodes[x].Nodes[y].Text, textBox1.Text, RegexOptions.IgnoreCase);

                            if (m.Success)
                            {
                                searchstring.Add(DiskCleaner._fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString());
                            }

                            else if (m2.Success)
                            {
                                searchstring.Add(DiskCleaner._fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString());
                            }

                            else
                            {

                            }
                        }
                    }
                }

                DiskCleaner.SearchCleaners(treeView1, Global.blbConf, searchstring);
                searchstring.Clear();
                treeView1.ExpandAll();
            }
            else
            {
                pictureBox1.Image = global::pcsm.Properties.Resources.q;
                DiskCleaner._fieldsTreeCache.Nodes.Clear();
                DiskCleaner.ReadCleaners(DiskCleaner._fieldsTreeCache, Global.blbConf);
                foreach (TreeNode _node in DiskCleaner._fieldsTreeCache.Nodes)
                {
                    treeView1.Nodes.Add((TreeNode)_node.Clone());
                }
                DiskCleaner.SelectionLoad(treeView1, Global.blbConf);
            }
            //enables redrawing tree after all objects have been added
            treeView1.EndUpdate();
        }

        public void RefreshCleaners(TreeView treeView1, string settingsfile)
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Checked = DiskCleaner.SecSelection(treeView1.Nodes[i].Name, Global.settingsfile);
                for (int j = 0; j < treeView1.Nodes[i].Nodes.Count; j++)
                {
                    treeView1.Nodes[i].Nodes[j].Checked = DiskCleaner.Selection(treeView1.Nodes[i].Nodes[j].Tag.ToString(), Global.settingsfile);
                }
            }
        }

        public void ReadDiskClean()
        {

            if (editmode == true)
            {
                treeView1.Dispose();
                triStateTreeView1.Location = new System.Drawing.Point(6, 32);
                triStateTreeView1.Size = new System.Drawing.Size(350, 192);
                DiskCleaner.ReadCleanersFile(triStateTreeView1, Global.system + "settings\\tasks\\" + id + "\\Bleachbit.ini", "tree");
                DiskCleaner.SelectionLoad(triStateTreeView1, Global.system + "settings\\tasks\\" + id + "\\Bleachbit.ini");
                pictureBox1.Visible = false;
                textBox2.Visible = false;
            }
            else
            {
                DiskCleaner.ReadCleaners(treeView1, Global.system + "settings\\tasks\\" + id + "\\Bleachbit.ini");
                DiskCleaner.CheckUncheckTreeNode(treeView1.Nodes, false);
                DiskCleaner._fieldsTreeCache.Nodes.Clear();
                foreach (TreeNode node in treeView1.Nodes)
                {
                    DiskCleaner._fieldsTreeCache.Nodes.Add((TreeNode)node.Clone());
                }
                _fieldsTreeCache1.Nodes.Clear();
                foreach (TreeNode node in treeView1.Nodes)
                {
                    _fieldsTreeCache1.Nodes.Add((TreeNode)node.Clone());
                }
            }
        }

        public void ReadRegSections()
        {

            string[] sectionnames = { "Active X / COM", "Startup", "ScanFonts", "Application Info", "Drivers", "Help Files", "Sounds", "Application Paths", "Application Settings", "Shared DLL", "Recent Files" };

            if (File.Exists(Global.system + "settings\\tasks\\" + id + "\\regsections.ini"))
            {
                string[] strArray = PCS.GetAllKeysInIniFileSection("sections", Global.system + "settings\\tasks\\" + id + "\\regsections.ini");


                for (int i = 0; i < strArray.Length; i++)
                {
                    string boolean = strArray[i].Substring(strArray[i].Length - 1);
                    string value = strArray[i].Substring(0, (strArray[i].Length - 2));
                    TreeNode ParentNode = new TreeNode();
                    ParentNode.Name = value;
                    ParentNode.Text = sectionnames[i];
                    if (boolean == "1")
                    {
                        ParentNode.Checked = true;
                    }
                    else
                    {
                        ParentNode.Checked = false;
                    }
                    treeView2.Nodes.Add(ParentNode);

                }
            }

            else
            {
                string[] sectionnames1 = { "Activex", "Startup", "ScanFonts", "AppInfo", "Drivers", "HelpFiles", "Sounds", "AppPaths", "AppSettings", "SharedDLL", "HistoryList" };

                for (int i = 0; i < sectionnames1.Length; i++)
                {
                    TreeNode ParentNode = new TreeNode();
                    ParentNode.Name = sectionnames1[i];
                    ParentNode.Text = sectionnames[i];
                    ParentNode.Checked = true;
                    treeView2.Nodes.Add(ParentNode);
                }

            }
        }

        public void ListDrives()
        {
            DiskDefragger.ListDrives(dataGridView1, checkBox13, checkBox12, checkBox14, Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini");
            if (editmode == true)
            {
                DiskDefragger.ReadDefragSettings(dataGridView1, checkBox13, checkBox12, checkBox14, Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", true);
                if (PCS.IniReadValue(Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", "main", "pathonly") == "True")
                {
                    radioButton2.Checked = true;
                    DefragCheckChanged();
                }
                textBox4.Text = PCS.IniReadValue(Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", "main", "path");
            }
        }

        public void SaveDefragSettings()
        {
            if (radioButton1.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", "main", "pathonly", "False");
                PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", "main", "path", "");
            }
            if (radioButton2.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", "main", "pathonly", "True");
                PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini", "main", "path", textBox4.Text);
            }
            DiskDefragger.SaveDefragSettings(dataGridView1, checkBox13, checkBox12, checkBox14, Global.system + "settings\\tasks\\" + id + "\\defragsettings.ini");
        }
        
        public void SelectScheduleType(string option)
        {
            comboBox2.Enabled = false;
            if (option == "Registry cleanup, Disk cleanup and Disk Defrag")
            {
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
                ReadDiskClean();
                ListDrives();
            }
            if (option == "Clean Registry")
            {
                TabRemove(tabControl2, "TabPage1");
                TabRemove(tabControl2, "TabPage2");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
            if (option == "Disk Cleanup")
            {
                TabRemove(tabControl2, "TabPage0");
                TabRemove(tabControl2, "TabPage2");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
                ReadDiskClean();
            }
            if (option == "Disk Defragmenation")
            {
                TabRemove(tabControl2, "TabPage0");
                TabRemove(tabControl2, "TabPage1");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
                ListDrives();
            }
            if (option == "Disk Cleanup & Disk Defragmenation")
            {
                TabRemove(tabControl2, "TabPage0");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
                ReadDiskClean();
                ListDrives();
            }
            if (option == "Clean Registry & Disk Defragmenation")
            {
                TabRemove(tabControl2, "TabPage1");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
                ListDrives();
            }
            if (option == "Disk Cleanup & Clean Registry")
            {
                TabRemove(tabControl2, "TabPage2");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
                ReadDiskClean();
            }
        }

        public void SaveSchedule()
        {
            if (comboBox1.Text == "Hourly")
            {
                SchTime("hourly");
                PCS.IniWriteValue("main", "schedule", "hourly");
            }
            else if (comboBox1.Text == "Daily")
            {
                SchTime("daily");
                PCS.IniWriteValue("main", "schedule", "daily");
            }
            else if (comboBox1.Text == "Weekly")
            {
                SchTime("weekly");
                PCS.IniWriteValue("main", "schedule", "weekly");
            }
            else if (comboBox1.Text == "Monthly")
            {
                SchTime("monthly");
                PCS.IniWriteValue(Global.settingsfile, "main", "schedule", "monthly");
            }
            else if (comboBox1.Text == "Log On")
            {
                SchTime("logon");
            }
            else if (comboBox1.Text == "Log Off")
            {
                TaskService ts = new TaskService();
                ts.RootFolder.DeleteTask(@"Performance Maintainer Task " + id);
                PCS.IniWriteValue("main", "schedule", "none");
            }
        }

        public void SaveRegSections()
        {
            for (int i = 0; i < treeView2.Nodes.Count; i++)
            {
                String value = treeView2.Nodes[i].Name;

                if (treeView2.Nodes[i].Checked)
                {
                    PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\regsections.ini", "sections", value, "1");
                }
                else
                {
                    PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\regsections.ini", "sections", value, "0");
                }
            }
            this.Close();
        }

        public void TabRemove(TabControl tb, string tabname)
        {
            for (int i = 0; i < tb.TabPages.Count; i++)
            {
                if (tb.TabPages[i].Name.Equals(tabname, StringComparison.OrdinalIgnoreCase))
                {
                    tb.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        public void DefragCheckChanged()
        {
            if (radioButton1.Checked)
            {
                label10.Enabled = false;
                textBox4.Enabled = false;
                button4.Enabled = false;

                dataGridView1.DefaultCellStyle.BackColor = SystemColors.Window;
                label8.Enabled = true;
                dataGridView1.Enabled = true;
                checkBox12.Enabled = true;
                checkBox13.Enabled = true;
                checkBox14.Enabled = true;
            }

            if (radioButton2.Checked)
            {
                label8.Enabled = false;
                dataGridView1.Enabled = false;
                checkBox12.Enabled = false;
                checkBox13.Enabled = false;
                checkBox14.Enabled = false;
                dataGridView1.DefaultCellStyle.BackColor = SystemColors.Control;

                label10.Enabled = true;
                textBox4.Enabled = true;
                button4.Enabled = true;

            }
        }

        #region Events
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Monthly")
            {
                label11.Visible = true;
                comboBox3.Visible = true;
            }
            else
            {
                label11.Visible = false;
                comboBox3.Visible = false;
            }

            if (comboBox1.Text != "Log On" && comboBox1.Text != "Log Off")
            {
                label4.Visible = true;
                dateTimePicker2.Visible = true;
            }

            else
            {
                label4.Visible = false;
                label11.Visible = false;
                comboBox3.Visible = false;
                dateTimePicker2.Visible = false;
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                checkBox5.Visible = false;
                checkBox6.Visible = false;
                checkBox7.Visible = false;
            }

            if (comboBox1.Text == "Weekly")
            {
                checkBox1.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                checkBox4.Visible = true;
                checkBox5.Visible = true;
                checkBox6.Visible = true;
                checkBox7.Visible = true;
            }

            else
            {
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                checkBox5.Visible = false;
                checkBox6.Visible = false;
                checkBox7.Visible = false;
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectScheduleType(comboBox2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter name of the task.");
            }
            else if (comboBox1.Text == "" && editmode == false)
            {
                MessageBox.Show("Please select a schedule type.");
            }
            else if (comboBox2.Text == "")
            {
                MessageBox.Show("Please select a Action for task.");
            }
            else if (comboBox3.Visible == true && comboBox3.Text == "")
            {
                MessageBox.Show("Please select day of the month.");
            }
            else if (triStateTreeView1.Nodes.Count == 0 && (comboBox2.Text == "Registry cleanup, Disk cleanup and Disk Defrag" || comboBox2.Text == "Disk Cleanup" || comboBox2.Text == "Disk Cleanup & Disk Defragmenation" || comboBox2.Text == "Disk Cleanup & Clean Registry"))
            {
                MessageBox.Show("Please select a Disk Cleaning option");

            }
            else
            {
                PCS.IniWriteValue(taskfile, id, "name", textBox1.Text);
                PCS.IniWriteValue(taskfile, id, "enabled", "1 ");
                if (comboBox2.Text == "Registry cleanup, Disk cleanup and Disk Defrag")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "1");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                if (comboBox2.Text == "Clean Registry")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "2");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                if (comboBox2.Text == "Disk Cleanup")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "3");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                if (comboBox2.Text == "Disk Defragmenation")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "4");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                if (comboBox2.Text == "Disk Cleanup & Disk Defragmenation")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "5");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                if (comboBox2.Text == "Clean Registry & Disk Defragmenation")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "6");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                if (comboBox2.Text == "Disk Cleanup & Clean Registry")
                {
                    PCS.IniWriteValue(taskfile, id, "typeid", "7");
                    PCS.IniWriteValue(taskfile, id, "typename", comboBox2.Text);
                }
                DiskCleaner.SaveCleaners(triStateTreeView1, Global.system + "settings\\tasks\\" + id + "\\Bleachbit.ini");
                PCS.IniWriteValue(Global.system + "settings\\tasks\\" + id + "\\Bleachbit.ini", "bleachbit", "check_online_updates", "False");
                SaveDefragSettings();
                SaveRegSections();
                SaveSchedule();
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
                            if (t.Name == "Performance Maintainer Task " + id)
                            {
                                PCS.IniWriteValue(taskfile, id, "trigger", trg.ToString());
                            }
                        }
                    }
                    catch { }
                }
                treeView1.Nodes.Clear();
                n = 0;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FilterCleaners(treeView1, searchstring, textBox2, pictureBox1);
            DiskCleaner.CheckUncheckTreeNode(treeView1.Nodes, false);
        }

        private void treeView1_MouseLeave(object sender, EventArgs e)
        {
            DiskCleaner.SaveCleaners(triStateTreeView1, Global.system + "settings\\tasks\\" + id + "\\Bleachbit.ini");
            System.Threading.Thread.Sleep(250);
            DiskCleaner._fieldsTreeCache.Nodes.Clear();
            if (DiskCleaner._fieldsTreeCache.Nodes.Count == 0)
            {

                foreach (TreeNode node in treeView1.Nodes)
                {
                    DiskCleaner._fieldsTreeCache.Nodes.Add((TreeNode)node.Clone());
                }
            }
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int x = 0; x < treeView1.Nodes.Count; x++)
            {
                for (int y = 0; y < treeView1.Nodes[x].Nodes.Count; y++)
                {
                    if (treeView1.Nodes[x].Nodes[y].Checked && !triStateTreeView1.Nodes.ContainsKey(treeView1.Nodes[x].Name))
                    {
                        TreeNode ParentNode = new TreeNode();
                        ParentNode.Name = treeView1.Nodes[x].Name;
                        ParentNode.Text = treeView1.Nodes[x].Text;
                        ParentNode.Tag = treeView1.Nodes[x].Tag;
                        ParentNode.Checked = treeView1.Nodes[x].Checked;
                        triStateTreeView1.Nodes.Add(ParentNode);

                        foreach (TreeNode node in treeView1.Nodes[x].Nodes)
                        {

                            triStateTreeView1.Nodes[n].Nodes.Add((TreeNode)node.Clone());

                        }
                        n++;
                        triStateTreeView1.ExpandAll();
                        break;
                    }
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DefragCheckChanged();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DefragCheckChanged();
        }
        #endregion
    }
}