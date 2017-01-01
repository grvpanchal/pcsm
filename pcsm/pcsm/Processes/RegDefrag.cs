using System;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class RegDefrag : Form
    {
        public RegDefrag()
        {
            InitializeComponent();
        }

        public void Analyse()
        {
            RegDefragger.Analyse(chart1);
        }

        public void Defrag()
        {
            RegDefragger.Defrag();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            this.Hide();
        }

        private void poweredby_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/lilregdefrag/");
        }
    }
}
