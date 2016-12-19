using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class DiskCleanList : Form
    {
        public DiskCleanList()
        {
            InitializeComponent();
            DiskCleaner.ReadCleaners(treeView1, Global.blbConf);
        }

        private TreeView _fieldsTreeCache1 = new TreeView();

        private void Form1_Load(object sender, EventArgs e)
        {
            DiskCleaner.SelectionLoad(treeView1, Global.blbConf);
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
            if (this.textBox1.Text != string.Empty)
            {
                Global.cleanertext = this.textBox1.Text;
            }
            DiskCleaner.SaveCleaners(treeView1, Global.blbConf);
            this.Close();
        }

        List<string> searchstring = new List<string>();

        public void filter_cleaners(TreeView treeView1, List<string> searchstring, TextBox textBox1, PictureBox pictureBox1)
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


        public void refresh_cleaners(TreeView treeView1, string settingsfile)
        {           
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Checked = DiskCleaner.SecSelection(treeView1.Nodes[i].Name, settingsfile);
                for (int j = 0; j < treeView1.Nodes[i].Nodes.Count; j++)
                {
                    if (treeView1.Nodes[i].Nodes[j].Tag.ToString() == "chromium.cache")
                    {
                        MessageBox.Show(DiskCleaner.Selection(treeView1.Nodes[i].Nodes[j].Tag.ToString(), settingsfile).ToString());
                    }
                    treeView1.Nodes[i].Nodes[j].Checked = DiskCleaner.Selection(treeView1.Nodes[i].Nodes[j].Tag.ToString(), settingsfile);
                }

            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filter_cleaners(treeView1, searchstring, textBox1, pictureBox1);
        }

        private void treeView1_MouseLeave(object sender, EventArgs e)
        {
            DiskCleaner.SaveCleaners(treeView1, Global.blbConf);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            filter_cleaners(treeView1, searchstring, textBox1, pictureBox1);
        }

        private void DiskCleanList_Shown(object sender, EventArgs e)
        {
            DiskCleaner.SelectionLoad(treeView1, Global.blbConf);
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
}
