using pcsm;
using pcsm.Processes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace pcsmwin
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        
        RegClean rc = new RegClean();
        DiskClean dc = new DiskClean();
        RegDefrag rd = new RegDefrag();
        DiskDefrag dd = new DiskDefrag();
        public void check_skipped_pending(CheckBox c, Label l)
        {
            if (c.Checked)
            {
                l.Visible = true;
                l.Text = "pending";
 
            }
            else
            {
                l.Visible = true;
                l.Text = "skipped";
            }
        }

        

        public void start_analysis()
        {
            log.WriteLog("Maintainer Analysis Start");
            Global.currentprocess = "analysis";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("button1.enabled", "false");
            data.Add("button2.enabled", "false");
            data.Add("checkbox1.visible", "false");
            data.Add("checkbox2.visible", "false");
            data.Add("checkbox3.visible", "false");
            data.Add("checkbox5.visible", "false");
            data.Add("linklabel1.visible", "false");
            data.Add("linklabel2.visible", "false");
            data.Add("linklabel3.visible", "false");

            data.Add("label4.visible", "true");
            data.Add("label6.visible", "true");
            data.Add("label7.visible", "true");
            data.Add("label8.visible", "true");

            backgroundWorker1.ReportProgress(5, data);

            if (checkBox1.Checked)
            {
                
                data["label4.visible"] = "false";
                backgroundWorker1.ReportProgress(10, data);
                data.Add("linklabel5.visible", "false");
                data.Add("picturebox11.visible", "true");
                backgroundWorker1.ReportProgress(10, data);
                Application.DoEvents();                
                rc.analyse();
                data.Add("linklabel5.text", Global.registryerrors.ToString() + " Registry Errors\nwere found");
                data["picturebox11.visible"] = "false";
                data["linklabel5.visible"] = "true";
                data["linklabel5.enabled"] = "true";
                backgroundWorker1.ReportProgress(20, data);
                Thread.Sleep(500);
            }

            if (checkBox2.Checked)
            {
                data["label6.visible"] = "false";
                backgroundWorker1.ReportProgress(30, data);
                data.Add("picturebox12.visible", "true");
                backgroundWorker1.ReportProgress(30, data);
                Application.DoEvents();
                dc.analyse();
                data.Add("linklabel6.text", Global.cleanupsize + " space can\nbe freed");
                data["picturebox12.visible"] = "false";
                data["linklabel6.visible"] = "true";
                data["linklabel6.enabled"] = "true";
                backgroundWorker1.ReportProgress(40, data);
                Thread.Sleep(500);

            }

            if (checkBox3.Checked)
            {
                data["label7.visible"] = "false";

                data.Add("linklabel8.visible", "false");
                data.Add("picturebox5.visible", "true");
                backgroundWorker1.ReportProgress(50, data);
                Application.DoEvents();
                dd.analyse();
                data.Add("linklabel8.text", "");
                for (int k = 0; k < Global.fragmentedp.Count(); k++)
                {
                    double fragp = Convert.ToDouble(Global.fragmentedp[k]);
                    if (fragp > 5)
                    {
                        data["linklabel8.text"] = "Defragmentation\nRequired";
                        Global.fragmentedp[k] = "0." + k.ToString();
                        break;
                    }
                    else
                    {
                        data["linklabel8.text"] = "Defragmentation\nnot Required";
                    }
                }

                data["picturebox5.visible"] = "false";
                data["linklabel8.visible"] = "true";
                data["linklabel8.enabled"] = "true";
                backgroundWorker1.ReportProgress(60, data);
                Thread.Sleep(500);
            }

            data["button2.enabled"] = "true";
            backgroundWorker1.ReportProgress(100, data);
            log.WriteLog("Maintainer Analysis end");
            
        }

        Dictionary<string, string> data = new Dictionary<string, string>();
        public void start_repair()
        {
            log.WriteLog("Maintainer Repair start");
            Global.currentprocess = "repair";
            
            data.Add("button1.enabled", "false");
            data.Add("button2.enabled", "false");
            data.Add("checkbox1.visible", "false");
            data.Add("checkbox2.visible", "false");
            data.Add("checkbox3.visible", "false");
            data.Add("checkbox5.visible", "false");
            data.Add("linklabel1.visible", "false");
            data.Add("linklabel2.visible", "false");
            data.Add("linklabel3.visible", "false");

            data.Add("label4.visible", "true");
            data.Add("label6.visible", "true");
            data.Add("label7.visible", "true");
            data.Add("label8.visible", "true");

            backgroundWorker1.ReportProgress(5, data);
            string notify = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "notification");

            if (checkBox1.Checked)
            {
                if (notify == "true")
                {
                    this.notifyIcon1.ShowBalloonTip(6000, "Performance Maintainer", "Cleaning Registry Problems...", ToolTipIcon.Info);
                }
                notifyIcon1.Text = "Cleaning Registry Problems...";
                data["label4.visible"] = "false";
                backgroundWorker1.ReportProgress(10, data);
                data.Add("linklabel5.visible", "false");
                data.Add("picturebox11.visible", "true");
                backgroundWorker1.ReportProgress(10, data);
                Application.DoEvents();
                rc.repair();
                data.Add("linklabel5.text", Global.registry + " Registry\nProblems Cleared");                
                data["picturebox11.visible"] = "false";
                data["linklabel5.visible"] = "true";
                data["linklabel5.enabled"] = "false";
                backgroundWorker1.ReportProgress(20, data);
                Thread.Sleep(500);
            }

            if (checkBox2.Checked)
            {
                if (notify == "true")
                {
                    this.notifyIcon1.ShowBalloonTip(6000, "Performance Maintainer", "Performing Disk Cleanup...", ToolTipIcon.Info);
                }
                notifyIcon1.Text = "Performing Disk Cleanup...";
                data["label6.visible"] = "false";
                backgroundWorker1.ReportProgress(30, data);
                data.Add("picturebox12.visible", "true");
                backgroundWorker1.ReportProgress(30, data);
                Application.DoEvents();               
                dc.clean();
                data.Add("linklabel6.text", Global.cleanupsize + " space is\nRecovered");
                data["picturebox12.visible"] = "false";
                data["linklabel6.visible"] = "true";
                data["linklabel6.enabled"] = "false";
                backgroundWorker1.ReportProgress(40, data);
                Thread.Sleep(500);
                

            }

            if (checkBox3.Checked)
            {
                if (notify == "true")
                {
                    this.notifyIcon1.ShowBalloonTip(6000, "Performance Maintainer", "Defragmenting Drive...", ToolTipIcon.Info);
                }
                notifyIcon1.Text = "Defragmenting Drive...";
                data["label7.visible"] = "false";
                data.Add("linklabel8.visible", "false");
                data.Add("picturebox5.visible", "false");
                data.Add("progressbar2.visible", "true");
                data.Add("picturebox5.enabled", "false");
                data["label9.visible"] = "true";
                
                string uncompress = PCS.IniReadValue("settings//defragsettings.ini", "main", "uncompress");
                if (uncompress == "True")
                {
                    if (data.ContainsKey("progressbar2.visible"))
                    {
                        
                        data["progressbar2.visible"] = "true";
                    }
                    else
                    {
                        data.Add("progressbar2.visible", "true");
                    }
                }
                backgroundWorker1.ReportProgress(50, data);
                Application.DoEvents();
                this.timer1.Interval = 300;
                timer1.Start();
                dd.defrag();
                timer1.Stop();
                this.timer1.Interval = 3000;
                data.Add("linklabel8.text", "");
                data["linklabel8.text"] = "Defragmentation\nComplete";
                data["linklabel8.enabled"] = "false";
                data["progressbar2.visible"] = "false";
                data["linklabel8.visible"] = "true";
                data["label9.visible"] = "false";
                backgroundWorker1.ReportProgress(60, data);
                Thread.Sleep(500);
            }

            
            backgroundWorker1.ReportProgress(100, data);
            log.WriteLog("Maintainer Analysis end");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiskCleanList dcl = new DiskCleanList();
            dcl.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiskDefragList ddl = new DiskDefragList();
            ddl.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegCleanList rcl = new RegCleanList();
            rcl.Show();
        }
        
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rc.Show();
        }


        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dc.Show();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rd.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dd.Show();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Process.Start("http://sourceforge.net/projects/lilregdefrag/");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("http://bleachbit.sourceforge.net/");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Process.Start("http://ultradefrag.sourceforge.net/");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Process.Start("http://sourceforge.net/projects/lilregdefrag/");
        }

        public bool value(Dictionary<string, string> a, string b)
        {
            if (a.ContainsKey(b))
            {
                if (a[b] == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public string valuetext(Dictionary<string, string> a, string b)
        {
            if (a.ContainsKey(b))
            {
                return a[b];
            }
            else
            {
                return null;
            }

        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int type = (int)e.Argument;
            if(type == 0)
                start_analysis();
            if (type == 1)
                start_repair();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dictionary<string, string> results = (Dictionary<string, string>)e.UserState;
            checkBox1.Visible = value(results, "checkbox1.visible");
            checkBox2.Visible = value(results, "checkbox2.visible");
            checkBox3.Visible = value(results, "checkbox3.visible");
            label4.Visible = value(results, "label4.visible");
            linkLabel5.Visible = value(results, "linklabel5.visible");
            pictureBox11.Visible = value(results, "picturebox11.visible");
            linkLabel5.Text = valuetext(results, "linklabel5.text");
            linkLabel5.Enabled = value(results, "linklabel5.enabled");

            label6.Visible = value(results, "label6.visible");
            linkLabel6.Visible = value(results, "linklabel6.visible");
            pictureBox12.Visible = value(results, "picturebox12.visible");
            progressBar2.Visible = value(results, "progressbar2.visible");
            linkLabel6.Text = valuetext(results, "linklabel6.text");
            linkLabel6.Enabled = value(results, "linklabel6.enabled");
            label9.Visible = value(results, "label9.visible");
            
            label7.Visible = value(results, "label7.visible");
            label10.Visible = value(results, "label10.visible"); 
            linkLabel8.Visible = value(results, "linklabel8.visible");
            pictureBox5.Visible = value(results, "picturebox5.visible");
            linkLabel8.Text = valuetext(results, "linklabel8.text");
            linkLabel8.Enabled = value(results, "linklabel8.enabled");
            

            progressBar1.Value = e.ProgressPercentage;
            decimal toint = Math.Round(Convert.ToDecimal(valuetext(results, "progressbar2.value")), 0);
            progressBar2.Value = Convert.ToInt32(toint);
            if (valuetext(results, "progressbar2.style") == "marquee")
            {
                progressBar2.Style = ProgressBarStyle.Marquee;
            }
            else 
            {
                progressBar2.Style = ProgressBarStyle.Continuous; 
            }
            label9.Text = valuetext(results, "label9.text");
            label10.Text = valuetext(results, "label10.text");
            
        }

        private void Maintainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (Global.compactstatus == true)
            {
                progressBar2.Style = ProgressBarStyle.Marquee;
                label10.Text = Global.compactprocess;
                label10.Visible = true;
                
            }
            else
            {
                label10.Visible = false;
                progressBar2.Style = ProgressBarStyle.Continuous;
                if (Global.currentprocess == "analysis")
                {
                    linkLabel5.Text = Global.registryerrors.ToString() + " Registry Errors\nwere found";
                    linkLabel6.Text = Global.cleanupsize + " space can\nbe freed";
                }
                label9.Text = Global.diskdefragdrive + Global.diskdefragprogress + "%";
                decimal toint = Math.Round(Convert.ToDecimal(Global.diskdefragprogress), 0);
                progressBar2.Value = Convert.ToInt32(toint);
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Global.compactstatus == true)
            {
                if (data.ContainsKey("label10.visible"))
                {
                    data["label10.visible"] = "true";
                }
                if (data.ContainsKey("progressbar2.style"))
                {
                    data["progressbar2.style"] = "marquee";
                }
                if (data.ContainsKey("label10.text"))
                {
                    data["label10.text"] = Global.compactprocess;
                }
                else
                {
                    data.Add("label10.visible", "true");
                    data.Add("progressbar2.style", "marquee");
                    data.Add("label10.text", Global.compactprocess);
                    data["label9.visible"] = "false";

                } 
            }
            else
            {
                if (data.ContainsKey("label10.visible"))
                {
                    data["label10.visible"] = "false";
                }
                if (data.ContainsKey("progressbar2.style"))
                {
                    data["progressbar2.style"] = "continous";
                }
                if (data.ContainsKey("progressbar2.value"))
                {
                    data["progressbar2.value"] = Global.diskdefragprogress;
                }
                if (data.ContainsKey("label9.text"))
                {
                    data["label9.text"] = Global.diskdefragdrive + Global.diskdefragprogress + "%";
                }
                else
                {
                    data["progressbar2.style"]= "continous";
                    data.Add("progressbar2.value", Global.diskdefragprogress);
                    data.Add("label9.text", Global.diskdefragdrive + Global.diskdefragprogress + "%");
                    data["label9.visible"] = "true";
                }
            }
            backgroundWorker1.ReportProgress(50, data);
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            PCS.process("cmd", "/c taskkill /im lrc.exe /im bleachbit_console.exe /im udefrag.exe /t /f", true);
            log.WriteLog("Maintainer Closed");
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //this.Hide();
            //this.WindowState = FormWindowState.Minimized;
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg == "/S")
                {
                    this.Hide();
                }
            }
            log.WriteLog("Maintainer Started");

            PCS pcs = new PCS();

            string regclean = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "regclean");
            if (regclean == "true")
            {
                checkBox1.Checked = true;
            }

            string diskclean = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskclean");
            if (diskclean == "true")
            {
                checkBox2.Checked = true;
            }

            string diskdefrag = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "diskdefrag");
            if (diskdefrag == "true")
            {
                checkBox3.Checked = true;
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            check_skipped_pending(checkBox1, label4);
            check_skipped_pending(checkBox2, label6);
            check_skipped_pending(checkBox3, label7);
            backgroundWorker1.RunWorkerAsync(1);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string notify = PCS.IniReadValue(Global.system + "settings\\downloadsettings.ini", "schedule", "notification");
            if (notify == "true")
            {
                this.notifyIcon1.ShowBalloonTip(6000, "Performance Maintainer", "Maintenance Complete! \n\n Files Defragmented:" + Global.defragmented + "\n Disk Space Recovered: " + Global.cleanupsize + "\n Registry Problems Cleared: " + Global.registry, ToolTipIcon.Info);
            }
            notifyIcon1.Text = "Maintenance Complete!";

            FormCollection fc = Application.OpenForms;
            bool closesignal = true;
            foreach (Form frm in fc)
            {
                if (frm.Name == "Options")
                {
                    closesignal = false;
                    break;
                }
            }
            if (closesignal)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(10000);
                this.Close();
            }
            else
            {
                button2.Text = "Close";
            }
            
        }
        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = global::pcsmwin.Properties.Resources.optionsglow;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = global::pcsmwin.Properties.Resources.options;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Options op = new Options();
            op.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hideShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;

            }
            else
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;

            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {

            }
        }
    }


    }

