using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pcsm
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }
        
        PCS pcs = new PCS();

        public void SaveCheckboxCheck()
        {
            if (cfu_cb.Checked)
            {
                PCS.IniWriteValue(Global.settingsfile, "main", "checkforupdates", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.settingsfile, "main", "checkforupdates", "false");
            }
            if (checkBox3.Checked)
            {
                PCS.IniWriteValue(Global.system + Global.settingsfile, "main", "savelog", "true");
                PCS.IniWriteValue(Global.settingsfile, "main", "savelog", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + Global.settingsfile, "main", "savelog", "false");
                PCS.IniWriteValue(Global.settingsfile, "main", "savelog", "false");
            }
        }

        public void ReadCheckboxCheck()
        {
            string checkforupdates = PCS.IniReadValue("main", "checkforupdates");
            if (checkforupdates == "true")
            {
                cfu_cb.Checked = true;
            }

            string notify = PCS.IniReadValue(Global.system + Global.settingsfile, "schedule", "notification");
            if (notify == "true")
            {
                checkBox7.Checked = true;
            }
        }

        #region Events
        private void button2_Click(object sender, EventArgs e)
        {
            SaveCheckboxCheck();
            this.Close();
        }

        private void Options_Load(object sender, EventArgs e)
        {   
            ReadCheckboxCheck();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}