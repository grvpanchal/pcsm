﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace pcsm.Processes
{
    class DiskCleaner
    {
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public static TreeView _fieldsTreeCache = new TreeView();

        public static void SaveCleaners(TreeView treeView1, string settingsfile)
        {
            for (int x = 0; x < treeView1.Nodes.Count; x++)
            {
                if (treeView1.Nodes[x].Checked)
                {
                    PCS.IniWriteValue(settingsfile, "tree", treeView1.Nodes[x].Name.ToString(), "True");
                }
                else
                {
                    PCS.IniWriteValue(settingsfile, "tree", treeView1.Nodes[x].Name.ToString(), "False");
                }
                for (int y = 0; y < treeView1.Nodes[x].Nodes.Count; y++)
                {
                    if (treeView1.Nodes[x].Nodes[y].Checked)
                    {
                        PCS.IniWriteValue(settingsfile, "tree", treeView1.Nodes[x].Nodes[y].Tag.ToString(), "1");
                    }
                    else
                    {
                        PCS.IniWriteValue(settingsfile, "tree", treeView1.Nodes[x].Nodes[y].Tag.ToString(), "0");
                    }
                }
            }
        }
        
        public static void ReadCleaners(TreeView treeView1, string settingsfile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Global.blbExec);

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
                    if (!lineVal.Contains("free_disk_space"))
                    {
                        output.Add(lineVal);
                        
                    }
                }
                lineVal = process.StandardOutput.ReadLine();

            }
            Global.cleanerslist = output;
            string oldentry;
            int val = output.Count();
            string rpl;
            process.WaitForExit();
            int i = 0;
            while (i < output.Count)
            {
                string s = output[i];
                string[] rootsplit = s.Split('.');
                TreeNode ParentNode = new TreeNode();
                ParentNode.Name = rootsplit[0];
                ParentNode.Checked = SecSelection(rootsplit[0], settingsfile);
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
                    childnode.Checked = Selection(output[i], settingsfile);
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

        public static void ReadCleanersFile(TreeView treeView1, string settingsfile, string section)
        {

            byte[] buffer = new byte[2048];

            GetPrivateProfileSection(section, buffer, 2048, settingsfile);
            String[] tmp = Encoding.Default.GetString(buffer).Trim('\0').Split('\0');

            List<string> output = new List<string>();

            foreach (string temp in tmp)
            {
                if (temp.Contains("="))
                {
                    output.Add(temp.Substring(0, temp.IndexOf("=")));
                }
            }

            string oldentry;
            string rpl;
            int i = 0;
            while (i < output.Count)
            {

                string s = output[i];
                if (s.ToLowerInvariant().Contains("."))
                {
                    string[] rootsplit = s.Split('.');
                    TreeNode ParentNode = new TreeNode();
                    ParentNode.Name = rootsplit[0];
                    ParentNode.Checked = SecSelection(rootsplit[0], settingsfile);
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
                        childnode.Checked = Selection(output[i], settingsfile);
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
                else
                {
                    i++;
                }
            }

            treeView1.Refresh();
        }
        
        public static void SearchCleaners(TreeView treeView1, string settingsfile, List<string> output)
        {
            string oldentry;
            int val = output.Count();
            string rpl;            
            int i = 0;
            while (i < output.Count)
            {
                string s = output[i];
                string[] rootsplit = s.Split('.');
                TreeNode ParentNode = new TreeNode();
                ParentNode.Name = rootsplit[0];
                ParentNode.Checked = SecSelection(rootsplit[0], settingsfile);
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
                    childnode.Checked = Selection(output[i], settingsfile);
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

        public static bool Selection(string cleanername, string settingsfile)
        {
            string readselection = PCS.IniReadValue(settingsfile, "tree", cleanername);

            if (readselection == "0")
            {
                return false;
            }
            else
            {
                return true;

            }

        }

        public static bool SecSelection(string cleanername, string settingsfile)
        {
            string readselection = PCS.IniReadValue(settingsfile, "tree", cleanername);

            if (readselection == "False")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static void CheckUncheckTreeNode(TreeNodeCollection trNodeCollection, bool isCheck)
        {
            foreach (TreeNode trNode in trNodeCollection)
            {
                trNode.Checked = isCheck;
                if (trNode.Nodes.Count > 0)
                    CheckUncheckTreeNode(trNode.Nodes, isCheck);
            }
        }
        
        public static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively. 
                    CheckAllChildNodes(node, nodeChecked);
                }
            }
        }
        
        public static void SelectionLoad(TreeView treeView1, string settingsfile)
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Checked = DiskCleaner.SecSelection(treeView1.Nodes[i].Name, settingsfile);
                for (int j = 0; j < treeView1.Nodes[i].Nodes.Count; j++)
                {
                    treeView1.Nodes[i].Nodes[j].Checked = DiskCleaner.Selection(treeView1.Nodes[i].Nodes[j].Tag.ToString(), settingsfile);
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
        
        public static void Analyse(TreeView treeView1, Label label1, TextBox textBox1, TextBox textBox2)
        {   
            string series = null;
            if (treeView1.Nodes.Count == 0)
            {
                DiskCleaner.ReadCleaners(treeView1, Global.blbConf);
                
                List<string> searchstring = new List<string>();
                if (_fieldsTreeCache.Nodes.Count == 0)
                {
                    
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        _fieldsTreeCache.Nodes.Add((TreeNode)node.Clone());
                    }
                }

                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                textBox2.Text = Global.cleanertext;
                Global.cleanertext = string.Empty;
                if (textBox2.Text != string.Empty)
                {

                    for (int x = 0; x < _fieldsTreeCache.Nodes.Count; x++)
                    {
                        Match m1 = Regex.Match(_fieldsTreeCache.Nodes[x].Text, textBox2.Text, RegexOptions.IgnoreCase);
                        if (m1.Success)
                        {
                            for (int y = 0; y < _fieldsTreeCache.Nodes[x].Nodes.Count; y++)
                            {
                                searchstring.Add(_fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString());
                            }
                        }
                        else
                        {
                            for (int y = 0; y < _fieldsTreeCache.Nodes[x].Nodes.Count; y++)
                            {
                                Match m = Regex.Match(_fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString(), textBox2.Text, RegexOptions.IgnoreCase);
                                Match m2 = Regex.Match(_fieldsTreeCache.Nodes[x].Nodes[y].Text, textBox2.Text, RegexOptions.IgnoreCase);

                                if (m.Success)
                                {
                                    searchstring.Add(_fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString());
                                }

                                else if (m2.Success)
                                {
                                    searchstring.Add(_fieldsTreeCache.Nodes[x].Nodes[y].Tag.ToString());
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
                    foreach (TreeNode _node in _fieldsTreeCache.Nodes)
                    {
                        treeView1.Nodes.Add((TreeNode)_node.Clone());
                        
                    }
                    treeView1.CheckBoxes = true;
                }
                //enables redrawing tree after all objects have been added
                treeView1.EndUpdate();
            }
            for (int x = 0; x < treeView1.Nodes.Count; x++)
            {
                for (int y = 0; y < treeView1.Nodes[x].Nodes.Count; y++)
                {
                    if (treeView1.Nodes[x].Nodes[y].Checked)
                    {
                        series = series + " " + treeView1.Nodes[x].Nodes[y].Tag;
                    }
                }
            }

            try
            {
                ProcessStartInfo startInfo1 = new ProcessStartInfo(Global.blbExec);
                startInfo1.UseShellExecute = false;
                startInfo1.RedirectStandardInput = true;
                startInfo1.RedirectStandardOutput = true;
                startInfo1.Arguments = "--preview " + series;
                startInfo1.CreateNoWindow = true;
                startInfo1.WindowStyle = ProcessWindowStyle.Hidden;
                Process process1 = new Process();
                process1.StartInfo = startInfo1;
                process1.Start();

                List<string> output1 = new List<string>();
                string lineVal1 = process1.StandardOutput.ReadLine();
                string rem = null;
                string saved = "";
                while (lineVal1 != null)
                {
                    rem = rem + "\r\n" + lineVal1;

                    output1.Add(lineVal1);
                    lineVal1 = process1.StandardOutput.ReadLine();
                    Application.DoEvents();
                }

                if (output1.Count > 4)
                {
                    for (int s = 4; s > 1; s--)
                    {
                        if (output1[output1.Count - s].Length > 28)
                            if (output1[output1.Count - s].Substring(0, 28) == "Disk space to be recovered: ")
                            {
                                saved = output1[output1.Count - s].Substring(28);
                            }
                    }
                }
                label1.Text = "Recoverable Size: " + saved;
                Global.cleanupsize = saved;
                textBox1.Text = rem;
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
            catch
            {
            }
        }

        public static void Clean(TreeView treeView1, Label label1, TextBox textBox1)
        {
            string series = null;
            if (treeView1.Nodes.Count == 0)
            {
                DiskCleaner.ReadCleaners(treeView1, Global.blbConf);
            }
            for (int x = 0; x < treeView1.Nodes.Count; x++)
            {
                for (int y = 0; y < treeView1.Nodes[x].Nodes.Count; y++)
                {
                    if (treeView1.Nodes[x].Nodes[y].Checked)
                    {
                        series = series + " " + treeView1.Nodes[x].Nodes[y].Tag;
                    }
                }
            }


            ProcessStartInfo startInfo1 = new ProcessStartInfo(Global.blbExec);

            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardInput = true;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.Arguments = "--clean " + series;
            startInfo1.CreateNoWindow = true;
            startInfo1.WindowStyle = ProcessWindowStyle.Hidden;

            Process process1 = new Process();
            process1.StartInfo = startInfo1;
            process1.Start();

            List<string> output1 = new List<string>();
            string lineVal1 = process1.StandardOutput.ReadLine();
            string rem = null;
            string saved = "";
            while (lineVal1 != null)
            {
                rem = rem + "\r\n" + lineVal1;

                output1.Add(lineVal1);
                lineVal1 = process1.StandardOutput.ReadLine();
                Application.DoEvents();


            }
            if (output1.Count > 4)
            {
                for (int s = 4; s > 1; s--)
                {
                    if (output1[output1.Count - s].Length > 22)
                        if (output1[output1.Count - s].Substring(0, 22) == "Disk space recovered: ")
                        {
                            saved = output1[output1.Count - s].Substring(22);
                        }
                }
            }
            Global.cleanupsize = saved;
        }
    }
}
