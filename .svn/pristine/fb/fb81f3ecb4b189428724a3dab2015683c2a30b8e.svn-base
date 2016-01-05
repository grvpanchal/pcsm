using System;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class DiskDefragList : Form
    {
        public DiskDefragList()
        {
            InitializeComponent();
            DiskDefragger.listdrives(dataGridView1, checkBox1, checkBox2, checkBox3, "settings//defragsettings.ini");
        }
               
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void DiskDefragList_Load(object sender, EventArgs e)
        {
            DiskDefragger.read_defragsettings(dataGridView1, checkBox1, checkBox2, checkBox3, "settings//defragsettings.ini", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiskDefragger.save_defragsettings(dataGridView1, checkBox1, checkBox2, checkBox3, "settings//defragsettings.ini");
            this.Close();
        }

    }



      
   

   

}
