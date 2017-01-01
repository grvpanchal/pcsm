using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class DiskDefrag : Form
    {        
        public DiskDefrag()
        {
            InitializeComponent();
            DiskDefragger.ListDrives(dataGridView1, checkBox1, checkBox2, checkBox3, Global.defragConf);           
        }

        System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();

        public void Analyse()
        {
            DiskDefragger.Analyse(chart1, series1, dataGridView1, checkBox1, checkBox2, checkBox3, label4, Global.defragConf);
        }

        public void Defrag()
        {
            DiskDefragger.Defrag(dataGridView1, Global.defragConf);
        }

        #region Events
        private void DiskDefrag_Load(object sender, EventArgs e)
        {
            DiskDefragger.ReadDefragSettings(dataGridView1, checkBox1, checkBox2, checkBox3, Global.defragConf, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiskDefragger.SaveDefragSettings(dataGridView1, checkBox1, checkBox2, checkBox3, Global.defragConf);
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DiskDefragger.DriveSelectionChanged(chart1, series1, dataGridView1, label4, groupBox1);
        }
        
        private void poweredby_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ultradefrag.sourceforge.net/");
        }
        #endregion
    }
}
