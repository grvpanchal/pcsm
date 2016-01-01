using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pcsm.Scheduler
{
    public partial class NewTask : Form
    {
        public NewTask()
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void TabRemove(TabControl tb, string tabname)
        {
            for (int i = 0; i < tb.TabPages.Count; i++)
            {
                if (tb.TabPages[i].Name.Equals(tabname, StringComparison.OrdinalIgnoreCase))
                {
                    tb.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "Log On" && comboBox1.Text != "Log Off")
            {
                label4.Visible = true;
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = true;

            }

            else
            {
                label4.Visible = false;
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                checkBox5.Visible = false;
                checkBox6.Visible = false;
                checkBox7.Visible = false;
            }

            if (comboBox1.Text == "Weekly")
            {
                checkBox1.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                checkBox4.Visible = true;
                checkBox5.Visible = true;
                checkBox6.Visible = true;
                checkBox7.Visible = true;
            }

            else
            {
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
                checkBox5.Visible = false;
                checkBox6.Visible = false;
                checkBox7.Visible = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {            
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = false;
            if (comboBox2.Text == "All Three")
            {
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
 
            }
            if (comboBox2.Text == "Clean Registry")
            {
                TabRemove(tabControl2, "TabPage1");
                TabRemove(tabControl2, "TabPage2");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
            if (comboBox2.Text == "Disk Cleanup")
            {
                TabRemove(tabControl2, "TabPage0");
                TabRemove(tabControl2, "TabPage2");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
            if (comboBox2.Text == "Disk Defragmenation")
            {
                TabRemove(tabControl2, "TabPage0");
                TabRemove(tabControl2, "TabPage1");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
            if (comboBox2.Text == "Disk Cleanup & Disk Defragmenation")
            {
                TabRemove(tabControl2, "TabPage0");
                
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
            if (comboBox2.Text == "Clean Registry & Disk Defragmenation")
            {
                TabRemove(tabControl2, "TabPage1");                
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
            if (comboBox2.Text == "Disk Cleanup & Clean Registry")
            {                
                TabRemove(tabControl2, "TabPage2");
                TabRemove(tabControl2, "TabPage3");
                tabControl2.Visible = true;
            }
        }
    }
}
