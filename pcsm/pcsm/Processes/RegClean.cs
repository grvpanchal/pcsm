using System;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class RegClean : Form
    {
        public RegClean()
        {
            InitializeComponent();            
        }

        private Maintainer mainForm = null;

        public RegClean(Form callingForm)
        {
            mainForm = callingForm as Maintainer; 
            InitializeComponent();
        }

        public void analyse()
        {
            RegCleaner.analyse(dataGridView1, label1);
        }

        public void repair()
        {
            RegCleaner.repair();
        }
                
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            progressBar1.Visible = true;
            RegCleaner.save_regsections(treeView1);
            this.analyse();
            button1.Enabled = true;
            progressBar1.Visible = false;
        }

        private void RegClean_Load(object sender, EventArgs e)
        {            
            RegCleaner.read_regsections(treeView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            RegCleaner.save_regsections(treeView1);            
            this.Hide();
        }

        private void poweredby_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/lilregdefrag/");
        }
    }
}