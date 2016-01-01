using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using bscf;

namespace WindowsFormsApplication1
{
    public partial class ShortCutFix : Form
    {
        public ShortCutFix()
        {
            InitializeComponent();
        }

        public static bool CanRead(string path)
        {
            var readAllow = false;
            var readDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Read & rule.FileSystemRights) != FileSystemRights.Read) continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    readAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    readDeny = true;
            }

            return readAllow && !readDeny;
        }

        public  void search(string sDir, Dictionary<string, string> dic, string extention)
        {
            try
            {

                if (CanRead(sDir) && (System.IO.File.GetAttributes(sDir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                {
                    foreach (string f in Directory.GetFiles(sDir, "*." + extention))
                    {                        
                        Application.DoEvents();
                        string file = Path.GetFileName(f);
                        dic.Add(file, f);
                    }

                    foreach (string d in Directory.GetDirectories(sDir))
                    {                        
                        search(d, dic, extention);
                    }
                }
            }
            catch
            { }
        }

        public static void analyse(string sDir, ListView lsv, Label label1)
        {
            try
            {
                if (CanRead(sDir) && (System.IO.File.GetAttributes(sDir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint && sDir != @"C:\Windows")
                {
                    foreach (string f in Directory.GetFiles(sDir, "*.lnk"))
                    {
                        Application.DoEvents();
                        label1.Text = sDir;
                        string extension = Path.GetExtension(f);
                        if (extension != null && (extension.Equals(".lnk")))
                        {
                            if (System.IO.File.Exists(f))
                            {
                                WshShell shell = new WshShell();
                                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(f);
                                string target = link.TargetPath;

                                if (!System.IO.File.Exists(target) || !Directory.Exists(target))
                                {
                                    if (target.Length >= 20)
                                    {
                                        if (Directory.Exists(target))
                                        {

                                        }

                                        else if (!System.IO.File.Exists(target) && target != "" && target.Substring(0, 20) != @"C:\Windows\system32\")
                                        {
                                            if (target.Substring(0, 22) == @"C:\Program Files (x86)")
                                            {
                                                string try64 = @"C:\Program Files" + target.Substring(22);
                                                if (!System.IO.File.Exists(try64))
                                                {
                                                    //files.Add(f, try64);
                                                    lsv.Items.Add(f).SubItems.Add(try64);
                                                    Path.GetFileName(try64);
                                                }
                                            }

                                            else
                                            {
                                                if (!System.IO.File.Exists(target) || !Directory.Exists(target))
                                                    //files.Add(f, link.TargetPath);
                                                    lsv.Items.Add(f).SubItems.Add(link.TargetPath);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Directory.Exists(target))
                                        {

                                        }

                                        else if (!System.IO.File.Exists(target) && target != "")
                                        {

                                            if (!System.IO.File.Exists(target) || !Directory.Exists(target))
                                                lsv.Items.Add(f).SubItems.Add(link.TargetPath);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!System.IO.File.Exists(target) || !System.IO.Directory.Exists(target))
                                    {
                                        //files.Add(f, link.TargetPath);
                                        lsv.Items.Add(f).SubItems.Add(link.TargetPath);
                                    }
                                }
                            }
                        }
                    }
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        analyse(d, lsv, label1);

                    }
                    
                }
                else
                {
                    
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        public static void CreateLink(string shortcutFullPath, string target)
        {
            WshShell wshShell = new WshShell();
            IWshRuntimeLibrary.IWshShortcut newShortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(shortcutFullPath);
            newShortcut.TargetPath = target;
            newShortcut.Save();
        }

        public static void fix(ListView lsv)
        {
            try
            {
                if (lsv.Items.Count == 0)
                {
                    MessageBox.Show("Analyse first please");
                }
                for (int i = 0; i < lsv.Items.Count; i++)
                {
                    if (lsv.Items[i].Checked)
                    {
                        if (lsv.Items[i].SubItems.Count.Equals(3))
                        {
                            if (System.IO.File.Exists(lsv.Items[i].SubItems[0].Text))
                            {
                                System.IO.File.Delete(lsv.Items[i].SubItems[0].Text);
                                CreateLink(lsv.Items[i].SubItems[0].Text, lsv.Items[i].SubItems[2].Text);

                            }

                        }
                        else
                        {
                            if (System.IO.File.Exists(lsv.Items[i].SubItems[0].Text))
                            {
                                System.IO.File.Delete(lsv.Items[i].SubItems[0].Text);
                            }
                        }

                    }
                }
            }
            catch
            {

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            
            progressBar1.Visible = true;
            label2.Text = "Indexing files";
backgroundWorker1.RunWorkerAsync();
            

        }
Dictionary<string, string> exes = new Dictionary<string, string>();
DriveInfo[] allDrives = DriveInfo.GetDrives();
        private void button2_Click(object sender, EventArgs e)
        {
            fix(listView1);
            listView1.Clear();
            MessageBox.Show("Broken Shortcuts Cleared");           
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
                     
            
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Fixed)
                {
                    search(d.Name, exes, "exe");
                }
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Fixed)
                {
                    analyse(d.Name, listView1, label2);
                }
            }
            label2.Text = "";
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
                string lk = Path.GetFileName(listView1.Items[i].SubItems[1].Text);
                if (exes.ContainsKey(lk))
                {
                    listView1.Items[i].SubItems.Add(exes[lk]);
                }
            }
            exes.Clear();
            progressBar1.Visible = false;
            label1.Text = "Broken Shortcuts Found: " + listView1.Items.Count.ToString();
            if (listView1.Items.Count != 0)
            {
                button2.Enabled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.Show();
        }
    }
}
