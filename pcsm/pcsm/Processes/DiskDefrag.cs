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
            DiskDefragger.listdrives(dataGridView1, checkBox1, checkBox2, checkBox3, "settings//defragsettings.ini");           
        }

        System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();

        public void analyse()
        {
            DiskDefragger.analyse(chart1, series1, dataGridView1, checkBox1, checkBox2, checkBox3, label4, "settings//defragsettings.ini");
        }

        public void defrag()
        {
            DiskDefragger.defrag(dataGridView1, "settings//defragsettings.ini");
        }
               
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DiskDefragger.drive_selection_changed(chart1, series1, dataGridView1, label4, groupBox1);
        }

        private void DiskDefrag_Load(object sender, EventArgs e)
        {
           DiskDefragger.read_defragsettings(dataGridView1, checkBox1, checkBox2, checkBox3, "settings//defragsettings.ini", false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiskDefragger.save_defragsettings(dataGridView1, checkBox1, checkBox2, checkBox3, "settings//defragsettings.ini");
            this.Hide();
        }

        private void poweredby_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ultradefrag.sourceforge.net/");
        }
    }
}
