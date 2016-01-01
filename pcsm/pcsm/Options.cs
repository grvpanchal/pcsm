using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace pcsm
{
    public partial class Options : Form
    {

        public Options()
        {
            InitializeComponent();
            read_cleaners();
            listdrives();
        }

        #region Read all Key in Section

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
            (
                [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
            // Note that because the key/value pars are returned as null-terminated
            // strings with the last string followed by 2 null-characters, we cannot
            // use StringBuilder.
                [In] IntPtr pReturnedString,
                [In] UInt32 nSize,
                [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
            );

        private static string[] GetAllKeysInIniFileSection(string strSectionName, string strIniFileName)
        {
            // Allocate in unmanaged memory a buffer of suitable size.
            // I have specified here the max size of 32767 as documentated 
            // in MSDN.
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            // Start with an array of 1 string only. 
            // Will embellish as we go along.
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(strSectionName, pBuffer, 1778, strIniFileName);

            // iStartAddress will point to the first character of the buffer,
            int iStartAddress = pBuffer.ToInt32();
            // iEndAddress will point to the last null char in the buffer.
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;
            //int iEndAddress = iStartAddress + 1000;
            // Navigate through pBuffer.
            while (iStartAddress < iEndAddress)
            {
                // Determine the current size of the array.
                int iArrayCurrentSize = strArray.Length;
                // Increment the size of the string array by 1.
                Array.Resize<string>(ref strArray, iArrayCurrentSize + 1);
                // Get the current string which starts at "iStartAddress".
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                // Insert "strCurrent" into the string array.
                strArray[iArrayCurrentSize] = strCurrent;
                // Make "iStartAddress" point to the next string.
                iStartAddress += (strCurrent.Length + 1);
            }

            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;

            return strArray;
        }

        #endregion

        PCS pcs = new PCS();

        public void selectionload()
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Checked = secselection(treeView1.Nodes[i].Name);
                for (int j = 0; j < treeView1.Nodes[i].Nodes.Count; j++)
                {
                    treeView1.Nodes[i].Nodes[j].Checked = selection(treeView1.Nodes[i].Nodes[j].Tag.ToString());
                }

            }

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                for (int j = 0; j < treeView1.Nodes[i].Nodes.Count; j++)
                {
                    if (treeView1.Nodes[i].Nodes[j].Checked == false)
                    {
                        treeView1.Nodes[i].Expand();
                        break;
                    }
                }

            }
            treeView1.Nodes[0].EnsureVisible();
        }
        
        private static bool is_virtual(char volLetter)
        {
            string deviceName = string.Format("{0}:", volLetter);
            string targetPath;
            const int maxSize = 512;
            uint retSize;

            IntPtr ptr = Marshal.AllocHGlobal(maxSize);

            retSize = Native.QueryDosDevice(deviceName, ptr, maxSize);
            targetPath = Marshal.PtrToStringAnsi(ptr, (int)retSize);

            return targetPath.Contains("\\??\\");
        }

        public void listdrives()
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives().Where(x => x.IsReady).ToArray();
            List<string> drives = new List<string>();

            foreach (DriveInfo drive in driveInfo)
            {
                DriveType driveType = Native.GetDriveType(drive.Name);
                if (
                    driveType != DriveType.Fixed
                     &&
                    driveType != DriveType.Removable
                     &&
                    driveType != DriveType.RAMDisk
                ) continue;
                if (!is_virtual(drive.Name[0]))
                    drives.Add(drive.Name);
                double driveSizeGB = Math.Round((double)drive.TotalSize / (1024 * 1024 * 1024), 2);
                double driveFreeSpaceGB = Math.Round((double)drive.AvailableFreeSpace / (1024 * 1024 * 1024), 2);
                dataGridView1.Rows.Add(true, drive.Name, driveFreeSpaceGB + " GB", Math.Round((driveFreeSpaceGB / driveSizeGB) * 100, 2));
            }
            read_defragsettings(true);
        }

        public void read_regsections()
        {
            string[] sectionnames = { "Active X / COM", "Startup", "Fonts", "Application Info", "Drivers", "Help Files", "Sounds", "Application Paths", "Application Settings", "Shared DLL", "Recent Files" };
            string[] strArray = GetAllKeysInIniFileSection("sections", Global.system + "settings\\regsections.ini");

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

        public void save_regsections()
        {
            for (int i = 0; i < treeView2.Nodes.Count; i++)
            {
                String value = treeView2.Nodes[i].Name;

                if (treeView2.Nodes[i].Checked)
                {
                    PCS.IniWriteValue(Global.system + "settings\\regsections.ini", "sections", value, "1");
                }
                else
                {
                    PCS.IniWriteValue(Global.system + "settings\\regsections.ini", "sections", value, "0");
                }

            }
            this.Close();
        }

        public void read_cleaners()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Global.system+"blb\\bleachbit_console.exe");

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = "--list-cleaners";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            List<string> output = new List<string>();
            string lineVal = process.StandardOutput.ReadLine();

            while (lineVal != null)
            {
                if (!lineVal.Contains(" "))
                {
                    output.Add(lineVal);
                }
                lineVal = process.StandardOutput.ReadLine();

            }
            string oldentry;
            int val = output.Count();
            string rpl;
            process.WaitForExit();
            int i = 1;
            while (i < output.Count)
            {
                string s = output[i];
                string[] rootsplit = s.Split('.');
                TreeNode ParentNode = new TreeNode();
                ParentNode.Name = rootsplit[0];
                ParentNode.Checked = true;
                ParentNode.Checked = secselection(rootsplit[0]);
                rpl = rootsplit[0].Replace('_', ' ');
                if (rpl.Length > 7)
                {
                    if (rpl.Substring(0, 7) == "winapp2")
                    {
                        rpl = rpl.Substring(8);
                    }
                }
                ParentNode.Text = char.ToUpper(rpl[0]) + rpl.Substring(1);
                treeView1.Nodes.Add(ParentNode);
                oldentry = rootsplit[0];

                while (rootsplit[0] == oldentry)
                {
                    TreeNode childnode = new TreeNode();
                    childnode.Name = rootsplit[1];
                    childnode.Tag = output[i];
                    childnode.Checked = selection(output[i]);
                    rpl = rootsplit[1].Replace('_', ' ');
                    childnode.Text = char.ToUpper(rpl[0]) + rpl.Substring(1);
                    ParentNode.Nodes.Add(childnode);
                    i++;
                    if (i < output.Count)
                    {
                        s = output[i];
                        rootsplit = s.Split('.');
                    }
                    else
                    {
                        break;
                    }
                }
            }
            treeView1.Refresh();
        }

        public bool secselection(string cleanername)
        {
            string readselection = PCS.IniReadValue(Global.system + "blb\\Bleachbit.ini", "tree", cleanername);

            if (readselection == "False")
            {
                return false;
            }
            else
            {
                return true;

            }

        }

        public void save_Cleaners()
        {
            for (int x = 0; x < treeView1.Nodes.Count; x++)
            {
                if (treeView1.Nodes[x].Checked)
                {
                    PCS.IniWriteValue(Global.system + "blb\\Bleachbit.ini", "tree", treeView1.Nodes[x].Name.ToString(), "True");
                }
                else
                {
                    PCS.IniWriteValue(Global.system + "blb\\Bleachbit.ini", "tree", treeView1.Nodes[x].Name.ToString(), "False");
                }
                for (int y = 0; y < treeView1.Nodes[x].Nodes.Count; y++)
                {
                    if (treeView1.Nodes[x].Nodes[y].Checked)
                    {
                        PCS.IniWriteValue(Global.system + "blb\\Bleachbit.ini", "tree", treeView1.Nodes[x].Nodes[y].Tag.ToString(), "1");
                    }
                    else
                    {
                        PCS.IniWriteValue(Global.system + "blb\\Bleachbit.ini", "tree", treeView1.Nodes[x].Nodes[y].Tag.ToString(), "0");
                    }
                }
            }
        }

        public bool selection(string cleanername)
        {
            string readselection = PCS.IniReadValue(Global.system + "blb\\Bleachbit.ini", "tree", cleanername);

            if (readselection == "0")
            {
                return false;
            }
            else
            {
                return true;

            }

        }

        public void read_defragsettings(bool silent)
        {
            string optimize = PCS.IniReadValue(Global.system + "settings\\defragsettings.ini", "main", "optimize");
            if (optimize == "True")
                checkBox1.CheckState = CheckState.Checked;
            else
                checkBox1.CheckState = CheckState.Unchecked;

            string optimizemft = PCS.IniReadValue(Global.system + "settings\\defragsettings.ini", "main", "optimizemft");
            if (optimizemft == "True")
                checkBox2.CheckState = CheckState.Checked;
            else
                checkBox2.CheckState = CheckState.Unchecked;
           
            string fixeddrives = dataGridView1.Rows.Count.ToString();
            string settingsfixeddrives = PCS.IniReadValue(Global.system + "settings\\defragsettings.ini", "main", "fixeddrives");
            if (fixeddrives == settingsfixeddrives)
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string settingsdrive = PCS.IniReadValue(Global.system + "settings\\defragsettings.ini", j.ToString(), "drivename");
                    string detecteddrive = this.dataGridView1.Rows[j].Cells[1].Value.ToString().Substring(0, 2);
                    if (settingsdrive == detecteddrive)
                    {
                        string drivechecked = PCS.IniReadValue(Global.system + "settings\\defragsettings.ini", j.ToString(), "checked");
                        if (drivechecked == "True")
                        {
                            this.dataGridView1.Rows[j].Cells[0].Value = true;
                        }
                        else
                        {
                            this.dataGridView1.Rows[j].Cells[0].Value = false;
                        }
                    }
                    else
                    {
                        if (silent == false)
                        {
                            MessageBox.Show("Drive Configurations has been changed. Rewriting Configuration.");

                        }
                        save_defragsettings();
                    }
                }
            }
            else
            {
                if (silent == false)
                {
                    MessageBox.Show("Drive Configurations has been changed. Rewriting Configuration.");

                }
                save_defragsettings();
            }

        }

        public void save_defragsettings()
        {

            PCS.IniWriteValue(Global.system + "settings\\defragsettings.ini", "main", "optimize", checkBox1.Checked.ToString());

            PCS.IniWriteValue(Global.system + "settings\\defragsettings.ini", "main", "optimizemft", checkBox2.Checked.ToString());

            PCS.IniWriteValue(Global.system + "settings\\defragsettings.ini", "main", "fixeddrives", dataGridView1.Rows.Count.ToString());

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                PCS.IniWriteValue(Global.system + "settings\\defragsettings.ini", j.ToString(), "drivename", this.dataGridView1.Rows[j].Cells[1].Value.ToString().Substring(0, 2));
                PCS.IniWriteValue(Global.system + "settings\\defragsettings.ini", j.ToString(), "checked", this.dataGridView1.Rows[j].Cells[0].Value.ToString());

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            save_Cleaners();
            save_defragsettings();
            save_regsections();
            save_checkbox_check();
            save_schedule();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            selectionload();
            read_defragsettings(true);
            read_regsections();
            read_checkbox_check();
        }
        
        public void save_checkbox_check()
        {
            if (cfu_cb.Checked)
            {
                PCS.IniWriteValue("settings\\downloadsettings.ini", "main", "checkforupdates", "true");
            }
            else
            {
                PCS.IniWriteValue("settings\\downloadsettings.ini", "main", "checkforupdates", "false");
            }
            if (checkBox3.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "main", "savelog", "true");
                PCS.IniWriteValue("settings\\downloadsettings.ini", "main", "savelog", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "main", "savelog", "false");
                PCS.IniWriteValue("settings\\downloadsettings.ini", "main", "savelog", "false");
            }
            if (checkBox4.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "regclean", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "regclean", "false");
            }
            if (checkBox5.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskclean", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskclean", "false");
            }
            if (checkBox6.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskdefrag", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskdefrag", "false");
            }
            if (checkBox6.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskdefrag", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskdefrag", "false");
            }
            if (checkBox7.Checked)
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "notification", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + "settings\\downloadsettings.ini", "schedule", "notification", "false");
            }

        }

        public void save_schedule()
        {
            string set = PCS.IniReadValue("main", "schedule");
            
            if (hourly_rb.Checked)
            {
                pcs.schtime("hourly");
                PCS.IniWriteValue("main", "schedule", "hourly");
            }
            else if (daily_rb.Checked && set != "daily")
            {
                pcs.schtime("daily");
                PCS.IniWriteValue("main", "schedule", "daily");
            }
            else if (weekly_rb.Checked)
            {
                pcs.schtime("weekly");
                PCS.IniWriteValue("main", "schedule", "weekly");
            }
            else if (monthly_rb.Checked)
            {
                pcs.schtime("monthly");
                PCS.IniWriteValue("settings\\downloadsettings.ini", "main", "schedule", "monthly");
            }
            else if (none_rb.Checked && set != "none")
            {                
                TaskService ts = new TaskService();
                ts.RootFolder.DeleteTask(@"Performance Maintainer");
                PCS.IniWriteValue("main", "schedule", "none");
                

            }
        }

        public void read_checkbox_check()
        {

            string checkforupdates = PCS.IniReadValue("main", "checkforupdates");
            if (checkforupdates == "true")
            {
                cfu_cb.Checked = true;
            }

            string savelog = PCS.IniReadValue("main", "savelog");
            if (savelog == "true")
            {
                checkBox3.Checked = true;
            }

            string regclean = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "regclean");
            if (regclean == "true")
            {
                checkBox4.Checked = true;
            }

            string diskclean = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskclean");
            if (diskclean == "true")
            {
                checkBox5.Checked = true;
            }

            string diskdefrag = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskdefrag");
            if (diskdefrag == "true")
            {
                checkBox6.Checked = true;
            }

            string notify = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "notification");
            if (notify == "true")
            {
                checkBox7.Checked = true;
            }

            string sch = PCS.IniReadValue("main", "schedule");
            if (sch == "hourly")
                hourly_rb.Checked = true;
            else if (sch == "daily")
                daily_rb.Checked = true;
            else if (sch == "weekly")
                weekly_rb.Checked = true;
            else if (sch == "monthly")
                monthly_rb.Checked = true;
            else
                none_rb.Checked = true;
            
        }

        private void CheckUncheckTreeNode(TreeNodeCollection trNodeCollection, bool isCheck)
        {
            foreach (TreeNode trNode in trNodeCollection)
            {
                trNode.Checked = isCheck;
                if (trNode.Nodes.Count > 0)
                    CheckUncheckTreeNode(trNode.Nodes, isCheck);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            CheckUncheckTreeNode(treeView1.Nodes, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckUncheckTreeNode(treeView1.Nodes, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }

    internal enum DriveType : uint
    {
        /// <summary>The drive type cannot be determined.</summary>
        Unknown = 0,    //DRIVE_UNKNOWN
        /// <summary>The root path is invalid, for example, no volume is mounted at the path.</summary>
        Error = 1,        //DRIVE_NO_ROOT_DIR
        /// <summary>The drive is a type that has removable media, for example, a floppy drive or removable hard disk.</summary>
        Removable = 2,    //DRIVE_REMOVABLE
        /// <summary>The drive is a type that cannot be removed, for example, a fixed hard drive.</summary>
        Fixed = 3,        //DRIVE_FIXED
        /// <summary>The drive is a remote (network) drive.</summary>
        Remote = 4,        //DRIVE_REMOTE
        /// <summary>The drive is a CD-ROM drive.</summary>
        CDROM = 5,        //DRIVE_CDROM
        /// <summary>The drive is a RAM disk.</summary>
        RAMDisk = 6        //DRIVE_RAMDISK
    }

    internal static class Native
    {
        /// <summary>
        /// The GetDriveType function determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk,
        /// or network drive.
        /// </summary>
        /// <param name="lpRootPathName">
        /// A pointer to a null-terminated string that specifies the root directory and returns information
        /// about the disk.A trailing backslash is required. If this parameter is NULL, the function uses
        /// the root of the current directory.
        /// </param>
        [DllImport("kernel32.dll")]
        internal static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string lpRootPathName);


        /// <summary>
        /// Adds a windows scheduled task.
        /// </summary>
        /// <param name="Servername">
        /// The name of the machine to add the scheduled task to.
        /// NULL for localhost.
        /// </param>
        /// <param name="Buffer">A pointer to a AT_JobInfo struct.</param>
        /// <param name="JobId">An output parameter.</param>
        /// <returns>0 on success. An Error code otherwise.</returns>
        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int NetScheduleJobAdd
            (string Servername, IntPtr Buffer, out int JobId);


        [DllImport("kernel32.dll")]
        internal static extern uint QueryDosDevice(string lpDeviceName, IntPtr lpTargetPath, uint ucchMax);
    }

}