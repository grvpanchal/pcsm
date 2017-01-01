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
        
        public RegClean(Form callingForm)
        {
            mainForm = callingForm as Maintainer; 
            InitializeComponent();
        }

        private Maintainer mainForm = null;

        public void Analyse()
        {
            RegCleaner.Analyse(dataGridView1, label1);
        }

        public void Repair()
        {
            RegCleaner.Repair();
        }

        #region Events
        private void RegClean_Load(object sender, EventArgs e)
        {            
            RegCleaner.ReadRegSections(treeView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            progressBar1.Visible = true;
            RegCleaner.SaveRegSections(treeView1);
            this.Analyse();
            button1.Enabled = true;
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            RegCleaner.SaveRegSections(treeView1);            
            this.Hide();
        }

        private void poweredby_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/lilregdefrag/");
        }
        #endregion
    }
}