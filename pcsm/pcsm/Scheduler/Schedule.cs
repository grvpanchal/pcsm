using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32.TaskScheduler;

namespace pcsm.Scheduler
{
    public partial class Schedule : Form
    {
        public static string taskfile = Global.system + "settings\\TaskSettings.ini";

        public void ReadTasks()
        {
            string name, schedule, action;
            for (int i = 1; i <= 11; i++)
            {
                if (PCS.IniReadValue(taskfile, i.ToString(), "name") != "")
                {
                    name = PCS.IniReadValue(taskfile, i.ToString(), "name");
                    schedule = PCS.IniReadValue(taskfile, i.ToString(), "trigger");
                    action = PCS.IniReadValue(taskfile, i.ToString(), "typename");
                    ListViewItem item = new ListViewItem(new[] { name, schedule, action });
                    if (PCS.IniReadValue(taskfile, i.ToString(), "enabled") == "1")
                    {
                        item.Checked = true;
                    }
                    listView1.Items.Add(item);
                }
            }
        }

        public Schedule()
        {
            InitializeComponent();

            ReadTasks();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string taskfile = Global.system + "settings\\TaskSettings.ini";
            string taskid = "";
            for (int i = 1; i <= 11; i++)
            {
                if (PCS.IniReadValue(taskfile, i.ToString(), "name") == "")
                {
                    taskid = i.ToString();
                    break;
                }
            }
            if (int.Parse(taskid) <= 10)
            {
                NewTask nt = new NewTask(taskid);
                nt.ShowDialog();
                listView1.Items.Clear();
                ReadTasks();
            }
            else
            {
                MessageBox.Show("Sorry, task cannot be created.\n You have reached the limit of 10 tasks.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                var confirmation = MessageBox.Show("Confirm task Deletion", "Are you Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {

                    for (int i = listView1.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem itm = listView1.SelectedItems[i];
                        listView1.Items[itm.Index].Remove();


                        for (int a = 1; a <= 10; a++)
                        {
                            if (PCS.IniReadValue(taskfile, a.ToString(), "name") == itm.SubItems[0].Text)
                            {
                                if (Directory.Exists(Global.system + "settings\\tasks\\" + a.ToString()))
                                    Directory.Delete(Global.system + "settings\\tasks\\" + a.ToString(), true);
                                Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService();
                                ts.RootFolder.DeleteTask(@"Performance Maintainer Task " + a.ToString());
                                PCS.IniDeleteSection(taskfile, a.ToString());
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("No Task selected");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                for (int i = listView1.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem itm = listView1.SelectedItems[i];
                    for (int a = 1; a <= 10; a++)
                    {
                        if (PCS.IniReadValue(taskfile, a.ToString(), "name") == itm.SubItems[0].Text)
                        {
                            NewTask nt = new NewTask(a.ToString());
                            nt.ShowDialog();
                            listView1.Items.Clear();
                            ReadTasks();
                        }
                    }
                }
            }

            else
                MessageBox.Show("No Task selected");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //using (TaskService ts = new TaskService())
            //{
            //    MessageBox.Show(ts.HighestSupportedVersion.ToString());
            //    Task t = ts.GetTask("Performance Maintainer Task 1");
            //    if (t != null)
            //    {

            //        MessageBox.Show(t.Enabled.ToString());
            //        if (t.Enabled)
            //        {                        
            //            t.Enabled = false;
            //            MessageBox.Show(t.Enabled.ToString());
            //            t.RegisterChanges();
            //            MessageBox.Show(t.Enabled.ToString());
            //        }
            //    }
            //}
            this.Close();
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                for (int i = listView1.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem itm = listView1.SelectedItems[i];
                    for (int a = 1; a <= 10; a++)
                    {
                        if (PCS.IniReadValue(taskfile, a.ToString(), "name") == itm.SubItems[0].Text)
                        {
                            NewTask nt = new NewTask(a.ToString());
                            nt.ShowDialog();
                            listView1.Items.Clear();
                            ReadTasks();
                        }
                    }
                }
            }

            else
                MessageBox.Show("No Task selected");
        }
    }

}
