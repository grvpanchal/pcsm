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
            if (checkBox7.Checked)
            {
                PCS.IniWriteValue(Global.system + Global.settingsfile, "schedule", "notification", "true");
            }
            else
            {
                PCS.IniWriteValue(Global.system + Global.settingsfile, "schedule", "notification", "false");
            }
        }

        public void ReadCheckboxCheck()
        {
            string notify = PCS.IniReadValue(Global.system + Global.settingsfile, "schedule", "notification");
            if (notify == "true")
            {
                checkBox7.Checked = true;
            }
            else
            {
                checkBox7.Checked = false;
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