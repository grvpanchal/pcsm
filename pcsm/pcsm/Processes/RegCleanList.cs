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

        #region Events
        private void RegClean_Load(object sender, EventArgs e)
        {
            RegCleaner.ReadRegSections(treeView1);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            RegCleaner.SaveRegSections(treeView1);
            this.Close();
        }
        #endregion
    }
}
