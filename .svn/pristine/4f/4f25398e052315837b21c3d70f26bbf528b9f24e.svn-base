using System;
using System.Windows.Forms;

namespace pcsm.Processes
{
    public partial class RegCleanList : Form
    {
        public RegCleanList()
        {
            InitializeComponent();         
        }        

        private void RegClean_Load(object sender, EventArgs e)
        {
            RegCleaner.read_regsections(treeView1);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            RegCleaner.save_regsections(treeView1);
            this.Close();
            
        }
    }
}
