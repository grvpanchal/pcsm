using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace pcsm.Processes
{
    class DiskDefragger
    {
        private static bool IsVirtual(char volLetter)
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

        public static void ListDrives(DataGridView dataGridView1, CheckBox optimize, CheckBox optimizemft, CheckBox uncompress, string settingsfile)
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
                if (!IsVirtual(drive.Name[0]))
                    drives.Add(drive.Name);
                double driveSizeGB = Math.Round((double)drive.TotalSize / (1024 * 1024 * 1024), 2);
                double driveFreeSpaceGB = Math.Round((double)drive.AvailableFreeSpace / (1024 * 1024 * 1024), 2);
                double drivefreeSpacep = Math.Round((driveFreeSpaceGB / driveSizeGB) * 100, 2);
                if (drive.Name == "C:\\" && drivefreeSpacep < 35)
                {
                    uncompress.CheckState = CheckState.Unchecked;
                    uncompress.Enabled = false;
                    PCS.IniWriteValue(Global.settingsfile, "main", "uncompress", "False");

                }
                dataGridView1.Rows.Add(true, drive.Name, driveFreeSpaceGB + " GB", drivefreeSpacep);
            }
            DiskDefragger.ReadDefragSettings(dataGridView1, optimize, optimizemft, uncompress, Global.settingsfile, true);
        }
        
        public static void ReadDefragSettings(DataGridView dataGridView1, CheckBox optimize, CheckBox optimizemft, CheckBox uncompress, string settingsfile, bool silent)
        {
            string optimize1 = PCS.IniReadValue(Global.settingsfile, "main", "optimize");
            if (optimize1 == "True")
                optimize.CheckState = CheckState.Checked;
            else
                optimize.CheckState = CheckState.Unchecked;

            string optimizemft1 = PCS.IniReadValue(Global.settingsfile, "main", "optimizemft");
            if (optimizemft1 == "True")
                optimizemft.CheckState = CheckState.Checked;
            else
                optimizemft.CheckState = CheckState.Unchecked;

            string uncompress1 = PCS.IniReadValue(Global.settingsfile, "main", "uncompress");
            if (uncompress1 == "True")
                uncompress.CheckState = CheckState.Checked;
            else
                uncompress.CheckState = CheckState.Unchecked;

            string fixeddrives = dataGridView1.Rows.Count.ToString();
            string settingsfixeddrives = PCS.IniReadValue(Global.settingsfile, "main", "fixeddrives");
            if (fixeddrives == settingsfixeddrives)
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string settingsdrive = PCS.IniReadValue(Global.settingsfile, j.ToString(), "drivename");
                    string detecteddrive = dataGridView1.Rows[j].Cells[1].Value.ToString().Substring(0, 2);
                    if (settingsdrive == detecteddrive)
                    {
                        string drivechecked = PCS.IniReadValue(Global.settingsfile, j.ToString(), "checked");
                        if (drivechecked == "True")
                        {
                            dataGridView1.Rows[j].Cells[0].Value = true;
                        }
                        else
                        {
                            dataGridView1.Rows[j].Cells[0].Value = false;
                        }
                    }
                    else
                    {
                        if (silent == false)
                        {
                            MessageBox.Show("Drive Configurations has been changed. Rewriting Configuration.");

                        }
                        DiskDefragger.SaveDefragSettings(dataGridView1, optimize, optimizemft, uncompress, Global.settingsfile);
                    }
                }
            }
            else
            {
                if (silent == false)
                {
                    MessageBox.Show("Drive Configurations has been changed. Rewriting Configuration.");

                }
                SaveDefragSettings(dataGridView1, optimize, optimizemft, uncompress, Global.settingsfile);
            }

        }

        public static void SaveDefragSettings(DataGridView dataGridView1, CheckBox optimize, CheckBox optimizemft, CheckBox uncompress, string settingsfile)
        {

            PCS.IniWriteValue(Global.settingsfile, "main", "optimize", optimize.Checked.ToString());

            PCS.IniWriteValue(Global.settingsfile, "main", "optimizemft", optimizemft.Checked.ToString());

            PCS.IniWriteValue(Global.settingsfile, "main", "uncompress", uncompress.Checked.ToString());

            PCS.IniWriteValue(Global.settingsfile, "main", "fixeddrives", dataGridView1.Rows.Count.ToString());

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                PCS.IniWriteValue(Global.settingsfile, j.ToString(), "drivename", dataGridView1.Rows[j].Cells[1].Value.ToString().Substring(0, 2));
                PCS.IniWriteValue(Global.settingsfile, j.ToString(), "checked", dataGridView1.Rows[j].Cells[0].Value.ToString());

            }
        }
        
        public static void DriveSelectionChanged(System.Windows.Forms.DataVisualization.Charting.Chart chart1, System.Windows.Forms.DataVisualization.Charting.Series series1, DataGridView dataGridView1, Label label4, GroupBox groupBox1)
        {
            string logf = dataGridView1.CurrentCell.RowIndex.ToString();
            if (File.Exists("ud\\" + logf + ".log"))
            {
                PCS pcs = new PCS();
                string fragmentedf = pcs.FindWord("ud\\" + logf + ".log", "fragmented files", 41);
                string fragmentedp = pcs.FindWord("ud\\" + logf + ".log", "fragmentation is above the threshold:", 61);
                string compf = pcs.FindWord("ud\\" + logf + ".log", "compressed files:", 41);
                string totalf = pcs.FindWord("ud\\" + logf + ".log", "files total:", 41);
                double defragmentedfiles = Convert.ToDouble(totalf) - Convert.ToDouble(compf) + Convert.ToDouble(fragmentedf);

                label4.Text = "Fragmentation: " + fragmentedp.Substring(0, 7);
                groupBox1.Text = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString().Substring(0, 2);
                series1.Points.Clear();
                series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
                series1.ChartArea = "ChartArea1";
                series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                series1.Legend = "Legend1";
                series1.Name = "Series1";
                series1.Points.AddXY("Defragmented Files: " + defragmentedfiles, defragmentedfiles);
                series1.Points.AddXY("Fragmented Files: " + fragmentedf, Convert.ToDouble(fragmentedf));
                series1.Points.AddXY("Compressed Files: " + compf, Convert.ToDouble(compf));
                double fs = Convert.ToDouble(dataGridView1[3, dataGridView1.CurrentCell.RowIndex].Value);
                double rs = 100 - fs;
                double fsp = Math.Round((fs * Convert.ToDouble(totalf)) / rs, 0);
                series1.Points.AddXY("Free Space: " + fs + "%", fsp);
            }
            else
            {
                label4.Text = "Drive Not Analysed";
                groupBox1.Text = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString().Substring(0, 2);
                series1.Points.Clear();
            }
        }

        public static void Analyse(System.Windows.Forms.DataVisualization.Charting.Chart chart1, System.Windows.Forms.DataVisualization.Charting.Series series1, DataGridView dataGridView1, CheckBox optimize, CheckBox optimizemft, CheckBox uncompress, Label label4, string settingsfile)
        {
            PCS pcs = new PCS();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string driveselection = dataGridView1[0, i].Value.ToString();

                if (driveselection == "True")
                {
                    string path = dataGridView1[1, i].Value.ToString().Substring(0, 2);
                    PCS.Process("cmd", "/C set UD_TIME_LIMIT=0h 30m && set UD_LOG_FILE_PATH=ud\\" + i.ToString() + ".log && ud\\udefrag.exe -a " + path, true);
                    string fragmentedpercent = pcs.FindWord("ud\\" + i + ".log", "fragmentation is above the threshold:", 61);
                    System.Threading.Thread.Sleep(1000);
                    Global.fragmentedp[i] = fragmentedpercent.Substring(0, 5);
                }
            }
            string fragmentedf = pcs.FindWord("ud\\0.log", "fragmented files", 41);
            string fragmentedp = pcs.FindWord("ud\\0.log", "fragmentation is above the threshold:", 61);

            string compf = pcs.FindWord("ud\\0.log", "compressed files:", 41);
            string totalf = pcs.FindWord("ud\\0.log", "files total:", 41);
            double defragmentedfiles = Convert.ToDouble(totalf) - Convert.ToDouble(compf) + Convert.ToDouble(fragmentedf);

            label4.Text = "Fragmentation: " + fragmentedp.Substring(0, 7);


            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Points.AddXY("Defragmented Files: " + defragmentedfiles, defragmentedfiles);
            series1.Points.AddXY("Fragmented Files: " + fragmentedf, Convert.ToDouble(fragmentedf));
            series1.Points.AddXY("Compressed Files: " + compf, Convert.ToDouble(compf));
            double fs = Convert.ToDouble(dataGridView1[3, 0].Value);
            double rs = 100 - fs;
            double fsp = Math.Round((fs * Convert.ToDouble(totalf)) / rs, 0);
            series1.Points.AddXY("Free Space: " + fs + "%", fsp);


            chart1.Series.Add(series1);
            chart1.Series[0]["PieLabelStyle"] = "Disabled";

        }

        public static void Defrag(DataGridView dataGridView1, string settingsfile)
        {
            PCS pcs = new PCS();
            string series = " ";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string driveselection = dataGridView1[0, i].Value.ToString();

                if (driveselection == "True")
                {
                    series = series + dataGridView1[1, i].Value.ToString().Substring(0, 2) + " ";
                }
            }
            string argoptions = " ";
            string optimize = PCS.IniReadValue(Global.settingsfile, "main", "optimize");
            if (optimize == "True")

                argoptions = argoptions + "-o ";


            string uncompress = PCS.IniReadValue(Global.settingsfile, "main", "uncompress");
            if (uncompress == "True")
            {
                Global.compactstatus = true;
                try
                {

                    ProcessStartInfo startInfo1 = new ProcessStartInfo("compact.exe");

                    startInfo1.UseShellExecute = false;
                    startInfo1.RedirectStandardInput = true;
                    startInfo1.RedirectStandardOutput = true;
                    startInfo1.Arguments = "/u /s /a /q /i *.*";
                    startInfo1.WorkingDirectory = "c:\\";
                    startInfo1.CreateNoWindow = true;
                    startInfo1.WindowStyle = ProcessWindowStyle.Hidden;

                    Process process1 = new Process();
                    process1.StartInfo = startInfo1;
                    process1.Start();


                    string lineVal = process1.StandardOutput.ReadLine();
                    while (lineVal != null)
                    {
                        if (lineVal != "")
                        {
                            Global.compactprocess = lineVal;
                        }
                        Application.DoEvents();
                        lineVal = process1.StandardOutput.ReadLine();

                    }
                }
                catch
                {

                }

            }
            Global.compactstatus = false;

            PCS.Process("cmd", "/C set UD_TIME_LIMIT=1h 30m && set UD_LOG_FILE_PATH=ud\\d.log && ud\\udefrag.exe -a C:", true);
            Global.defragmented = pcs.FindWord("ud\\d.log", "fragmented files", 41);
            try
            {
                ProcessStartInfo startInfo2 = new ProcessStartInfo(Global.defragExec);
                startInfo2.UseShellExecute = false;
                startInfo2.RedirectStandardInput = true;
                startInfo2.RedirectStandardOutput = true;
                startInfo2.Arguments = argoptions + series;
                startInfo2.CreateNoWindow = true;
                startInfo2.WindowStyle = ProcessWindowStyle.Hidden;

                Process process2 = new Process();
                process2.StartInfo = startInfo2;
                process2.Start();


                string lineVal2 = process2.StandardOutput.ReadLine();
                while (lineVal2 != null)
                {
                    if (lineVal2.Length > 14)
                    {
                        string checkanalyse = lineVal2.Substring(3, 8);
                        if (checkanalyse != "analyse:")
                        {
                            Global.diskdefragdrive = lineVal2.Substring(0, 2);
                            Global.diskdefragprogress = lineVal2.Substring(14, 2);

                        }
                    }
                    Application.DoEvents();
                    lineVal2 = process2.StandardOutput.ReadLine();
                }
            }
            catch
            {
            }

            string optimizemft = PCS.IniReadValue(Global.settingsfile, "main", "optimizemft");
            if (optimizemft == "True")
            {
                try
                {
                    ProcessStartInfo startInfo3 = new ProcessStartInfo(Global.defragExec);
                    startInfo3.UseShellExecute = false;
                    startInfo3.RedirectStandardInput = true;
                    startInfo3.RedirectStandardOutput = true;
                    startInfo3.Arguments = " --optimize-mft " + series;
                    startInfo3.CreateNoWindow = true;
                    startInfo3.WindowStyle = ProcessWindowStyle.Hidden;

                    Process process3 = new Process();
                    process3.StartInfo = startInfo3;
                    process3.Start();


                    string lineVal1 = process3.StandardOutput.ReadLine();
                    while (lineVal1 != null)
                    {
                        if (lineVal1.Length > 14)
                        {
                            string checkanalyse = lineVal1.Substring(3, 8);
                            if (checkanalyse != "analyse:")
                            {
                                Global.diskdefragdrive = lineVal1.Substring(0, 2);
                                Global.diskdefragprogress = lineVal1.Substring(14, 2);

                            }
                        }
                        Application.DoEvents();
                        lineVal1 = process3.StandardOutput.ReadLine();
                    }
                }
                catch
                {
                }
            }
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
