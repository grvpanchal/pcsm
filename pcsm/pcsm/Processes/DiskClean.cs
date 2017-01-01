using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace pcsm.Processes
{
    public partial class DiskClean : Form
    {
        public DiskClean()
        {            
            InitializeComponent();
        }

        List<string> searchString = new List<string>();

        public void Analyse()
        {
            DiskCleaner.Analyse(treeView1, label1, textBox2, textBox1);
        }

        public void Clean()
        {
            DiskCleaner.Clean(treeView1, label1, textBox1);
        }

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

        #region Events
        private void Form1_Load(object sender, EventArgs e)
        {
            DiskCleaner.SelectionLoad(treeView1, Global.blbConf);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            DiskCleaner.CheckUncheckTreeNode(treeView1.Nodes, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DiskCleaner.CheckUncheckTreeNode(treeView1.Nodes, false);
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            DiskCleaner.SaveCleaners(treeView1, Global.blbConf);
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FilterCleaners( treeView1, searchString, textBox1, pictureBox1);
        }

        private void treeView1_MouseLeave(object sender, EventArgs e)
        {
            DiskCleaner.SaveCleaners(treeView1, Global.blbConf);
            RefreshCleaners(DiskCleaner._fieldsTreeCache, Global.blbConf);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {            
                textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            Analyse();
            progressBar1.Visible = false;
            button1.Enabled = true;
        }

        private void poweredby_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://bleachbit.sourceforge.net");
        }
        #endregion
    }
}
