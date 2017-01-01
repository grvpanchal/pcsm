using pcsm.Processes;
using ServicesOptimizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace pcsm
{
    public partial class Maintainer : Form
    {
        public Maintainer()
        {
            InitializeComponent();            
        }
                
        RegClean rc = new RegClean();
        DiskClean dc = new DiskClean();
        RegDefrag rd = new RegDefrag();
        DiskDefrag dd = new DiskDefrag();
        FrmDiagnostics so = new FrmDiagnostics();

        Dictionary<string, string> data = new Dictionary<string, string>();
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

        public void CheckSkippedPending(CheckBox c, Label l)
        {
            if (c.Checked)
            {
                l.Visible = true;
                l.Text = "Pending";
 
            }
            else
            {
                l.Visible = true;
                l.Text = "Skipped";
            }
        }

        public void StartAnalysis(DoWorkEventArgs e)
        {   
            Global.currentprocess = "analysis";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("button1.enabled", "false");
            data.Add("button2.enabled", "false");
            data.Add("checkbox1.visible", "false");
            data.Add("checkbox2.visible", "false");
            data.Add("checkbox3.visible", "false");
            data.Add("checkbox4.visible", "false");
            data.Add("checkbox5.visible", "false");
            data.Add("linklabel1.visible", "false");
            data.Add("linklabel2.visible", "false");
            data.Add("linklabel3.visible", "false");
            data.Add("linklabel9.visible", "false");
            data["button1.visible"] = "true";
            data["button2.text"] = "Repair";

            if (checkBox1.Checked)
                data.Add("checkbox1.checked", "true");
            else
                data.Add("checkbox1.checked", "false");

            if (checkBox2.Checked)
                data.Add("checkbox2.checked", "true");
            else
                data.Add("checkbox2.checked", "false");

            if (checkBox3.Checked)
                data.Add("checkbox3.checked", "true");
            else
                data.Add("checkbox3.checked", "false");

            if (checkBox4.Checked)
                data.Add("checkbox4.checked", "true");
            else
                data.Add("checkbox4.checked", "false");

            if (checkBox5.Checked)
                data.Add("checkbox5.checked", "true");
            else
                data.Add("checkbox5.checked", "false");

            data.Add("label4.visible", "true");
            data.Add("label6.visible", "true");
            data.Add("label7.visible", "true");
            data.Add("label8.visible", "true");
            data.Add("label11.visible", "true");

            backgroundWorker1.ReportProgress(5, data);

            if (checkBox1.Checked)
            {
                data["label4.visible"] = "false";
                backgroundWorker1.ReportProgress(10, data);
                data.Add("linklabel5.visible", "false");
                data.Add("picturebox11.visible", "true");
                backgroundWorker1.ReportProgress(10, data);
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();                
                rc.Analyse();
                data.Add("linklabel5.text", Global.registryerrors.ToString() + " Registry Errors\nwere found");
                if (Global.registryerrors == 0)
                {
                    data["checkbox1.checked"] = "false";
                }
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
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();
                dc.Analyse();
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
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();
                dd.Analyse();
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
                        data["checkbox3.checked"] = "false";
                    }
                }

                data["picturebox5.visible"] = "false";
                data["linklabel8.visible"] = "true";
                data["linklabel8.enabled"] = "true";
                backgroundWorker1.ReportProgress(60, data);
                Thread.Sleep(500);
            }

            if (checkBox4.Checked)
            {
                data["label11.visible"] = "false";
                backgroundWorker1.ReportProgress(65, data);
                data.Add("picturebox14.visible", "true");
                backgroundWorker1.ReportProgress(68, data);
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();
                so.diagonise();
                Global.serviceanalysed = true;
                Thread.Sleep(500);
                int j =0;
               for (int i = 0; i < so.LstServices.Items.Count; i++)
                {
                    if (so.LstServices.Items[i].Checked)
                    {
                        j++;
                    }
                }
                data.Add("linklabel4.text", j.ToString() + " Service\nRecommendation(s)");
                if (j == 0)
                {
                    data["checkbox4.visible"] = "false";
                }
                data["picturebox14.visible"] = "false";
                data["linklabel4.visible"] = "true";
                data["linklabel4.enabled"] = "true";
                backgroundWorker1.ReportProgress(70, data);
                Thread.Sleep(500);
            }
            
            if (checkBox5.Checked)
            {
                DialogResult dlogResult = MessageBox.Show("Registry Defragmentation Analysis is a system intensive process. Please close your other windows. Your computer may not resond for several minutes while the scan is being completed. \n\n Do you wish to CONTINUE?", "Contine with Registry Defragmentation!", MessageBoxButtons.YesNo);
                if (dlogResult == DialogResult.Yes)
                {
                    data["label8.visible"] = "false";
                    backgroundWorker1.ReportProgress(70, data);
                    data.Add("linklabel7.Visible", "false");
                    backgroundWorker1.ReportProgress(70, data);
                    Application.DoEvents();
                    rd.Analyse();
                    data["linklabel7.visible"] = "true";
                    data["linklabel7.enabled"] = "true";
                    if ((100 - ((Global.newRegistrySize / Global.oldRegistrySize) * 100)) >= 5)
                    {
                        data.Add("linklabel7.text", "Defragmentation\nRequired");
                    }
                    else
                    {
                        data.Add("linklabel7.text", "Defragmentation\nnot Required");
                        data["checkbox5.checked"] = "false";
                    }
                }
            }

            data["button2.enabled"] = "true";
            if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false)
            {
                data["button2.enabled"] = "false";
            }
            backgroundWorker1.ReportProgress(100, data);
        }
        
        public void StartRepair(DoWorkEventArgs e)
        {
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
            data.Add("linklabel9.visible", "false");
            data["button1.visible"] = "true";
            data["button2.text"] = "Repair";

            if (checkBox1.Checked)
                data.Add("checkbox1.checked", "true");
            else
                data.Add("checkbox1.checked", "false");

            if (checkBox2.Checked)
                data.Add("checkbox2.checked", "true");
            else
                data.Add("checkbox2.checked", "false");

            if (checkBox3.Checked)
                data.Add("checkbox3.checked", "true");
            else
                data.Add("checkbox3.checked", "false");

            if (checkBox4.Checked)
                data.Add("checkbox4.checked", "true");
            else
                data.Add("checkbox4.checked", "false");

            if (checkBox5.Checked)
                data.Add("checkbox5.checked", "true");
            else
                data.Add("checkbox5.checked", "false");


            data.Add("label4.visible", "true");
            data.Add("label6.visible", "true");
            data.Add("label7.visible", "true");
            data.Add("label8.visible", "true");
            data.Add("label11.visible", "true");

            backgroundWorker1.ReportProgress(5, data);

            if (checkBox1.Checked)
            {
                data["label4.visible"] = "false";
                backgroundWorker1.ReportProgress(10, data);
                data.Add("linklabel5.visible", "false");
                data.Add("picturebox11.visible", "true");
                backgroundWorker1.ReportProgress(10, data);
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();
                rc.Repair();
                data.Add("linklabel5.text", Global.registry +" Registry\nProblems Cleared");
                data["picturebox11.visible"] = "false";
                data["linklabel5.visible"] = "true";
                data["linklabel5.enabled"] = "false";
                backgroundWorker1.ReportProgress(20, data);
                Thread.Sleep(500);
            }

            if (checkBox2.Checked)
            {
                data["label6.visible"] = "false";
                backgroundWorker1.ReportProgress(30, data);
                data.Add("picturebox12.visible", "true");
                backgroundWorker1.ReportProgress(30, data);
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();               
                dc.Clean();
                data.Add("linklabel6.text", Global.cleanupsize + " space is\nRecovered");
                data["picturebox12.visible"] = "false";
                data["linklabel6.visible"] = "true";
                data["linklabel6.enabled"] = "false";
                backgroundWorker1.ReportProgress(40, data);
                Thread.Sleep(500);
                

            }

            if (checkBox3.Checked)
            {
                data["label7.visible"] = "false";

                data.Add("linklabel8.visible", "false");
                data.Add("picturebox5.visible", "false");
                data.Add("progressbar2.visible", "true");
                data.Add("picturebox5.enabled", "false");
                data["label9.visible"] = "true";                
                string uncompress = PCS.IniReadValue(Global.defragConf, "main", "uncompress");
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
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();
                this.timer1.Interval = 300;
                timer1.Start();
                dd.Defrag();
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

            if (checkBox4.Checked)
            {
                data["label11.visible"] = "false";
                backgroundWorker1.ReportProgress(65, data);
                data.Add("picturebox14.visible", "true");
                backgroundWorker1.ReportProgress(68, data);
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Application.DoEvents();                
                Thread.Sleep(500);
                if (!Global.serviceanalysed)
                {
                    so.diagonise();
                }
                int j = 0;
                for (int i = 0; i < so.LstServices.Items.Count; i++)
                {
                    if (so.LstServices.Items[i].Checked)
                    {
                        j++;
                    }
                }
                so.Fix();
                data.Add("linklabel4.text", j.ToString() + " Services Optimized");
                data["picturebox14.visible"] = "false";
                data["linklabel4.visible"] = "true";
                data["linklabel4.enabled"] = "false";
                backgroundWorker1.ReportProgress(70, data);
                Thread.Sleep(500);
            }
                                   
            if (checkBox5.Checked)
            {
                DialogResult dlogResult = MessageBox.Show("Registry Defragmentation Analysis is a system intensive process. Please close your other windows. Your computer may not resond for several minutes while the scan is being completed. \n\n Do you wish to CONTINUE?", "Contine with Registry Defragmentation!", MessageBoxButtons.YesNo);
                if (dlogResult == DialogResult.Yes)
                {
                    data["label8.visible"] = "false";
                    backgroundWorker1.ReportProgress(70, data);
                    data.Add("linklabel7.Visible", "false");
                    backgroundWorker1.ReportProgress(70, data);
                    if (this.backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    Application.DoEvents();
                    rd.Defrag();
                    data["linklabel7.visible"] = "true";
                    data.Add("linklabel7.text", "Defragmentation\nComplete");
                    data["linklabel7.enabled"] = "false";
                    backgroundWorker1.ReportProgress(100, data);
                    DialogResult dialogResult = MessageBox.Show("Restart Required to complete Registry Defragmentation. Do you want to restart NOW?", "Restart Required!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        PCS.Process("shutdown.exe", "-r -t 0", true);
                        Thread.Sleep(1000);
                        Application.Exit();
                    }
                }
            }
            data["button2.enabled"] = "true";
            data["button1.visible"] = "false";
            data["button2.text"] = "Close";
            backgroundWorker1.ReportProgress(100, data);
        }

        #region
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiskCleanList dcl = new DiskCleanList();
            dcl.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiskDefragList ddl = new DiskDefragList();
            ddl.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegCleanList rcl = new RegCleanList();
            rcl.ShowDialog();
        }
        
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rc.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            CheckSkippedPending(checkBox1, label4);
            CheckSkippedPending(checkBox2, label6);
            CheckSkippedPending(checkBox3, label7);
            CheckSkippedPending(checkBox4, label11);
            CheckSkippedPending(checkBox5, label8);
            backgroundWorker1.RunWorkerAsync(0);            
            
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
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int type = (int)e.Argument;
            if(type == 0)
                StartAnalysis(e);
            if (type == 1)
                StartRepair(e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dictionary<string, string> results = (Dictionary<string, string>)e.UserState;
            button1.Enabled = value(results, "button1.enabled");
            button2.Enabled = value(results, "button2.enabled");
            button2.Text = valuetext(results, "button2.text");
            button1.Visible = value(results, "button1.visible");

            checkBox1.Visible = value(results, "checkbox1.visible");
            checkBox2.Visible = value(results, "checkbox2.visible");
            checkBox3.Visible = value(results, "checkbox3.visible");
            checkBox4.Visible = value(results, "checkbox4.visible");
            checkBox5.Visible = value(results, "checkbox5.visible");

            linkLabel1.Visible = value(results, "linklabel1.visible");
            linkLabel2.Visible = value(results, "linklabel2.visible");
            linkLabel3.Visible = value(results, "linklabel3.visible");
            linkLabel9.Visible = value(results, "linklabel9.visible");
            
            checkBox1.Checked = value(results, "checkbox1.checked");
            checkBox2.Checked = value(results, "checkbox2.checked");
            checkBox3.Checked = value(results, "checkbox3.checked");
            checkBox4.Checked = value(results, "checkbox4.checked");
            checkBox5.Checked = value(results, "checkbox5.checked");            

            label4.Visible = value(results, "label4.visible");
            linkLabel5.Visible = value(results, "linklabel5.visible");
            pictureBox11.Visible = value(results, "picturebox11.visible");
            linkLabel5.Text = valuetext(results, "linklabel5.text");
            linkLabel5.Enabled = value(results, "linklabel5.enabled");

            label6.Visible = value(results, "label6.visible");            
            pictureBox12.Visible = value(results, "picturebox12.visible");
            linkLabel6.Visible = value(results, "linklabel6.visible");
            linkLabel6.Text = valuetext(results, "linklabel6.text");
            linkLabel6.Enabled = value(results, "linklabel6.enabled");
            
            
            label7.Visible = value(results, "label7.visible");
            label10.Visible = value(results, "label10.visible");
            label9.Visible = value(results, "label9.visible");
            linkLabel8.Visible = value(results, "linklabel8.visible");
            pictureBox5.Visible = value(results, "picturebox5.visible");
            linkLabel8.Text = valuetext(results, "linklabel8.text");
            linkLabel8.Enabled = value(results, "linklabel8.enabled");
            progressBar2.Visible = value(results, "progressbar2.visible");

            label11.Visible = value(results, "label11.visible");
            pictureBox14.Visible = value(results, "picturebox14.visible");
            linkLabel4.Visible = value(results, "linklabel4.visible");
            linkLabel4.Text = valuetext(results, "linklabel4.text");
            linkLabel4.Enabled = value(results, "linklabel4.enabled");

            label8.Visible = value(results, "label8.visible");
            linkLabel7.Visible = value(results, "linklabel7.visible");
            linkLabel7.Text = valuetext(results, "linklabel7.text");
            linkLabel7.Enabled = value(results, "linklabel7.enabled");

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Close")
            {
                this.Close();
            }
            else
            {
                button2.Enabled = false;
                CheckSkippedPending(checkBox1, label4);
                CheckSkippedPending(checkBox2, label6);
                CheckSkippedPending(checkBox3, label7);
                CheckSkippedPending(checkBox4, label11);
                CheckSkippedPending(checkBox5, label8);
                backgroundWorker1.RunWorkerAsync(1);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
 
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

        private void Maintainer_Activated(object sender, EventArgs e)
        {

            label9.Text = Global.diskdefragdrive + Global.diskdefragprogress + "%";
            decimal toint = Math.Round(Convert.ToDecimal(Global.diskdefragprogress), 0);
            progressBar2.Value = Convert.ToInt32(toint);
        }
        
        private void Maintainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            PCS.Process("cmd", "/c taskkill /im lrc.exe /im bleachbit_console.exe /im udefrag.exe /t /f", true);
            backgroundWorker1.CancelAsync();
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmUsrProfile up = new FrmUsrProfile();
            up.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            so.Show();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.smartpcutilities.com/servicesoptimizer.html");
        }

        private void Maintainer_Shown(object sender, EventArgs e)
        {
            if (ServicesOptimizer.My.MySettingsProperty.Settings.MediaCntr == 0)
            {
                DialogResult dialogResult = MessageBox.Show("Service Optimization profile has been not created. Would you like to Create?", "Service Optimization Profile", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    FrmUsrProfile up = new FrmUsrProfile();
                    up.ShowDialog();
                }
                
                if (ServicesOptimizer.My.MySettingsProperty.Settings.MediaCntr == 0)
                {
                    checkBox4.Checked = false;
                    checkBox4.Enabled = false;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Application.ExitThread();
            }
            else
            {
                if (Global.currentprocess == "analysis")
                {
                    checkBox1.Location = new System.Drawing.Point(218, 77);
                    checkBox2.Location = new System.Drawing.Point(218, 127);
                    checkBox3.Location = new System.Drawing.Point(218, 177);
                    checkBox4.Location = new System.Drawing.Point(218, 227);
                    checkBox5.Location = new System.Drawing.Point(218, 277);

                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = true;
                    checkBox4.Visible = true;
                    checkBox5.Visible = true;
                }
 
            }
        }
        #endregion
    }
}
