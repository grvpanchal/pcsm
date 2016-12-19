using System;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class DiskDefragList : Form
    {
        public DiskDefragList()
        {
            InitializeComponent();
            DiskDefragger.ListDrives(dataGridView1, checkBox1, checkBox2, checkBox3, Global.defragConf);
        }
               
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void DiskDefragList_Load(object sender, EventArgs e)
        {
            DiskDefragger.ReadDefragSettings(dataGridView1, checkBox1, checkBox2, checkBox3, Global.defragConf, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiskDefragger.SaveDefragSettings(dataGridView1, checkBox1, checkBox2, checkBox3, Global.defragConf);
            this.Close();
        }

    }



      
   

   

}
