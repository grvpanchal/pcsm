using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using pcsm.Processes;
using pcsm.Scheduler;

namespace pcsm
{
    public partial class Main : Form
    {
        private bool m_bLayoutCalled = false;
        private DateTime m_dt;


        #region GlobalMemoryStatusEx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(this);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
        #endregion
       

        PCS pcs = new PCS();
        private Control _currentToolTipControl = null;      
        public Main()
        {            
            InitializeComponent();                                         
        }
              

        #region Shadow, Movement and Single Instance
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                // add the drop shadow flag for automatically drawing
                // a drop shadow around the form
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion
             
        #region Close and Show Window
        public void CloseWindow()
        {
            try
            {
                Process SomeProgram = new Process();
                SomeProgram.StartInfo.FileName = Global.system + "cmd";
                SomeProgram.StartInfo.Arguments = " /c taskkill /im lrc.exe /im bleachbit_console.exe /im udefrag.exe /t /f";
                SomeProgram.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                SomeProgram.StartInfo.CreateNoWindow = true;
                SomeProgram.StartInfo.UseShellExecute = false;

                SomeProgram.StartInfo.RedirectStandardOutput = false;
                SomeProgram.Start();                                
                string SomeProgramOutput = SomeProgram.StandardOutput.ReadToEnd();
            }
            catch (Exception)
            {
                // Log the exception
            }
            this.Close();
        }

        public void ShowWindow()
        {
            // Insert code here to make your form show itself.
            WinApi.ShowToFront(this.Handle);
        }
#endregion
                              
        #region Program  Open or Download

        public static string Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return String.Format("{0}.{1}", fvi.ProductMajorPart, fvi.ProductMinorPart);
            }
        }

        public void OpenProgram(string section)
        {
            String folder = PCS.IniReadValue(section, "folder");
            String subfolder = PCS.IniReadValue(section, "subfolder");
            String exefile = PCS.IniReadValue(section, "exefile");
            String exearg = PCS.IniReadValue(section, "exearg");
            if (File.Exists(folder + "\\" + subfolder + "\\" + exefile))
            {
                Process.Start(folder + "\\" + subfolder + "\\" + exefile, exearg);
            }
            else
            {
                Download myForm = new Download(section);
                DialogResult dlg = myForm.ShowDialog(this);
                if (dlg == DialogResult.OK)
                {
                    Process.Start(folder + "\\" + subfolder + "\\" + exefile, exearg);
                }
            }
        }

        public void CheckProgram(string section, Label ll, Label install_ll)
        {
            string name = PCS.IniReadValue(section, "name");

            ll.Text = name;

            string Installed = PCS.IniReadValue(section, "installed");
            
            if (Installed == "1")
            {                
                ll.Enabled = true;
                install_ll.Visible = false;
                if (File.Exists("settings\\downloadsettingsnew.ini"))
                {
                    string oldversion = PCS.IniReadValue(section, "link");
                    string newversion = PCS.IniReadValue(section, "link");
                    if (newversion != null && newversion != oldversion)
                    {
                        PCS.IniWriteValue(section, "link", newversion);
                        string folder = PCS.IniReadValue(section, "folder");
                        PCS.IniWriteValue(section, "folder", folder);
                        string subfolder = PCS.IniReadValue("settings\\downloadsettingsnew.ini", section, "subfolder");
                        PCS.IniWriteValue(section, "subfolder", subfolder);
                        string newname = PCS.IniReadValue("settings\\downloadsettingsnew.ini", section, "name");
                        PCS.IniWriteValue(section, "name", newname);
                        string exefile = PCS.IniReadValue("settings\\downloadsettingsnew.ini", section, "exefile");
                        PCS.IniWriteValue(section, "exefile", exefile);
                        install_ll.Visible = true;
                        install_ll.Text = "Update";
                    }
                }
            }
            else
            {

                if (File.Exists("settings\\downloadsettingsnew.ini"))
                {
                    string oldversion = PCS.IniReadValue(section, "link");
                    string newversion = PCS.IniReadValue("settings\\downloadsettingsnew.ini", section, "link");
                    if (newversion != null && newversion != oldversion)
                    {
                        PCS.IniWriteValue(section, "link", newversion);
                        string folder = PCS.IniReadValue("settings\\downloadsettingsnew.ini", section, "folder");
                        PCS.IniWriteValue(section, "folder", folder);
                        string subfolder = PCS.IniReadValue("settings\\downloadsettingsnew.ini", section, "subfolder");
                        PCS.IniWriteValue(section, "subfolder", subfolder);
                        
                    }
                }
            }
            

        }
        
        #endregion

        private void Main_Load(object sender, EventArgs e)
        {
            if (m_bLayoutCalled == false)
            {
                m_bLayoutCalled = true;
                m_dt = DateTime.Now;
                this.Activate();
                SplashScreen.CloseForm();
            }
            log.WriteLog("Application Started");       

        }
           
                
        #region System Info
        public void get_system_info()
        {
            using (Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0"))
            {
                if (regKey != null)
                {
                    string procName = regKey.GetValue("ProcessorNameString") as string;

                    if (!string.IsNullOrEmpty(procName))
                    {
                        string pros = System.Text.RegularExpressions.Regex.Replace(procName, @"\s+", " ");
                        string processor = pros.Replace("(R)", "\u00AE");
                        this.label2.Text = processor;
                    }
                    else
                    {
                        this.label2.Text = "Unknown";
                    }
                }
                else
                    this.label2.Text = "Unknown";
            }

            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();

            if (GlobalMemoryStatusEx(memStatus))
                this.label3.Text = string.Format("{0} RAM", PCS.ConvertSizeToString(Convert.ToInt64(memStatus.ullTotalPhys)));
            else
                this.label3.Text = "Unknown";

            this.label1.Text = OSVersion.GetOSFriendlyName();

            OperatingSystem OS = Environment.OSVersion;

            if (OS.Version.Major >= 6)
            {
                if(OS.Version.Minor == 0)
                    this.pictureBox1.Image = global::pcsm.Properties.Resources.vista;
                if (OS.Version.Minor >= 1)
                    this.pictureBox1.Image = global::pcsm.Properties.Resources._7;
                if (OS.Version.Minor >= 2)
                    this.pictureBox1.Image = global::pcsm.Properties.Resources._8;
            }
            
            }

            #endregion
        
        #region Main Panel Start

        private void Start_b_Click(object sender, EventArgs e)
        {
            OperatingSystem OS = Environment.OSVersion;
            if (OS.Version.Major > 5)
            {
                Maintainer m = new Maintainer();
                this.Hide();
                Start_b.Enabled = false;
                m.ShowDialog();
                Start_b.Enabled = true;
                this.Show();
            }
            else
            {
                Maintainer_xp m = new Maintainer_xp();
                this.Hide();
                Start_b.Enabled = false;
                m.ShowDialog();
                Start_b.Enabled = true;
                this.Show();
 
            }
            
        }
        
        #endregion

        #region Notify Icon Click

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

        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.pcstarters.net");
        } 

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            CloseWindow();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.Show();
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

       

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {

            }
        }

        #endregion
             
        #region Control Box Pictures

        private void minimize_p_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void close_p_Click(object sender, EventArgs e)
        {
            CloseWindow();
        }

        #endregion

        #region Additional Tools Link Labels

        private void SystemInformation_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("msinfo32");
        }

        private void uncompressdrive_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("cmd", "/C compact /? && pause");
        }

        private void services_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("services.msc");
        }

        private void lrc_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("lrc");            
        }

        private void lro_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("lro");
        }

        private void sre_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("sre"); 
        }

        private void dclean_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("dclean");
        }

        private void undelete_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("undelete");
        }

        private void fsinspect_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("fsinspect");
        }

        private void diskcleanup_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("scf");
        }              

        private void chkdskgui_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("chkdskgui");
        }

        private void stm_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("stm");
        }

        private void lum_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("lum");
        }

        private void ph_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void ultradefrag_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void eraser_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("eraser");
        }

        private void diskinfo_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgram("diskinfo");
        }

        #endregion
        
        #region Application Information Link Labels

        private void license_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Authors a = new Authors();
            a.ShowDialog();
        }

        private void about_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }

        private void develop_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://sf.net/projects/pcsm/");
        }

        private void blog_ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://blog.pcstarters.net");
        }

        private void pcstarterslogo_p_Click(object sender, EventArgs e)
        {
            Process.Start("http://blog.pcstarters.net");
        }

        #endregion           
       
        #region IntroWiz
        private void Main_Shown(object sender, EventArgs e)
        {

            string firstrun = PCS.IniReadValue("main", "firstrun");
            if (firstrun == "true")
            {
                IntroWiz a = new IntroWiz();
                a.Show();
                PCS.IniWriteValue("main", "firstrun", "false");
            }
            backgroundWorker1.RunWorkerAsync();

        }
        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Loading a = new Loading();
            a.Show();
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = global::pcsm.Properties.Resources.optionsglow;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = global::pcsm.Properties.Resources.options;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        
        private bool IsForegroundWindow
        {
            get
            {
                var foreWnd = GetForegroundWindow();
                return ((from f in this.MdiChildren select f.Handle)
                    .Union(from f in this.OwnedForms select f.Handle)
                    .Union(new IntPtr[] { this.Handle })).Contains(foreWnd);
            }
        }

        private void close_p_MouseHover(object sender, EventArgs e)
        {
            close_p.Image = global::pcsm.Properties.Resources.closehover;
        }

        private void close_p_MouseLeave(object sender, EventArgs e)
        {
            close_p.Image = global::pcsm.Properties.Resources.close;
            if(!IsForegroundWindow)
                close_p.Image = global::pcsm.Properties.Resources.closeinv;
        }

        private void minimize_p_MouseHover(object sender, EventArgs e)
        {
            minimize_p.Image = global::pcsm.Properties.Resources.minhover;
        }

        private void minimize_p_MouseLeave(object sender, EventArgs e)
        {
            minimize_p.Image = global::pcsm.Properties.Resources.min;
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            close_p.Image = global::pcsm.Properties.Resources.close;
            minimize_p.Image = global::pcsm.Properties.Resources.min;
        }

        private void Main_Deactivate(object sender, EventArgs e)
        {
            close_p.Image = global::pcsm.Properties.Resources.closeinv;
            minimize_p.Image = global::pcsm.Properties.Resources.mininv;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            PCS.process("blb\\bleachbit_console.exe", " --update-winapp2", true);
            PCS.process(Global.system + "blb\\bleachbit_console.exe", " --update-winapp2", true);
            if (PCS.CheckForInternetConnection() == true)
            {
                using (System.Net.WebClient myWebClient = new System.Net.WebClient())
                {
                    myWebClient.DownloadFile("http://www.pcstarters.net/pcsm/update/downloadsettings.ini", "settings\\downloadsettingsnew.ini");
                }
            }            
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Schedule sct = new Schedule();
            sct.ShowDialog();
        }

        private void pictureBox12_MouseHover(object sender, EventArgs e)
        {
            pictureBox12.Image = global::pcsm.Properties.Resources.schtglow;
        }

        private void pictureBox12_MouseLeave(object sender, EventArgs e)
        {
            pictureBox12.Image = global::pcsm.Properties.Resources.scht;
        }

        private void stm_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("stm");
        }

        private void lrc_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("lrc");
        }

        private void lro_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("lro");
        }

        private void sre_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("sre");
        }

        private void dclean_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("dclean");
        }

        private void eraser_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("eraser");
        }

        private void undelete_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("undelete");
        }

        private void fsinspect_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("fsinspect");
        }

        private void scf_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("scf");
        }

        private void ultradefrag_ll_Click(object sender, EventArgs e)
        {
            String Architecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            if (Architecture == "x86")
            {
                OpenProgram("ultradefrag86");
            }
            if (Architecture == "x64")
            {
                OpenProgram("ultradefrag86");
            }
        }

        private void chkdskgui_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("chkdskgui");
        }

        private void diskinfo_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("diskinfo");
        }

        private void lum_ll_Click(object sender, EventArgs e)
        {
            OpenProgram("lum");
        }

        private void ph_ll_Click(object sender, EventArgs e)
        {
            String Architecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            if (Architecture == "x86")
            {
                OpenProgram("ph86");
            }
            if (Architecture == "x64")
            {
                OpenProgram("ph64");
            }
        }

        private void stm_ll_MouseHover(object sender, EventArgs e)
        {
            stm_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            stm_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void stm_ll_MouseLeave(object sender, EventArgs e)
        {
            stm_ll.BackColor = System.Drawing.SystemColors.Window;
            stm_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void lrc_ll_MouseHover(object sender, EventArgs e)
        {
            lrc_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            lrc_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void lrc_ll_MouseLeave(object sender, EventArgs e)
        {
            lrc_ll.BackColor = System.Drawing.SystemColors.Window;
            lrc_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void lro_ll_MouseHover(object sender, EventArgs e)
        {
            lro_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            lro_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void lro_ll_MouseLeave(object sender, EventArgs e)
        {
            lro_ll.BackColor = System.Drawing.SystemColors.Window;
            lro_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void sre_ll_MouseHover(object sender, EventArgs e)
        {
            sre_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            sre_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void sre_ll_MouseLeave(object sender, EventArgs e)
        {
            sre_ll.BackColor = System.Drawing.SystemColors.Window;
            sre_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void dclean_ll_MouseHover(object sender, EventArgs e)
        {
            dclean_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            dclean_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void dclean_ll_MouseLeave(object sender, EventArgs e)
        {
            dclean_ll.BackColor = System.Drawing.SystemColors.Window;
            dclean_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void eraser_ll_MouseHover(object sender, EventArgs e)
        {
            eraser_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            eraser_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void eraser_ll_MouseLeave(object sender, EventArgs e)
        {
            eraser_ll.BackColor = System.Drawing.SystemColors.Window;
            eraser_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void undelete_ll_MouseHover(object sender, EventArgs e)
        {
            undelete_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            undelete_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void undelete_ll_MouseLeave(object sender, EventArgs e)
        {
            undelete_ll.BackColor = System.Drawing.SystemColors.Window;
            undelete_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void fsinspect_ll_MouseHover(object sender, EventArgs e)
        {
            fsinspect_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            fsinspect_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void fsinspect_ll_MouseLeave(object sender, EventArgs e)
        {
            fsinspect_ll.BackColor = System.Drawing.SystemColors.Window;
            fsinspect_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void scf_ll_MouseHover(object sender, EventArgs e)
        {
            scf_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            scf_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void scf_ll_MouseLeave(object sender, EventArgs e)
        {
            scf_ll.BackColor = System.Drawing.SystemColors.Window;
            scf_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void ultradefrag_ll_MouseHover(object sender, EventArgs e)
        {
            ultradefrag_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            ultradefrag_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void ultradefrag_ll_MouseLeave(object sender, EventArgs e)
        {
            ultradefrag_ll.BackColor = System.Drawing.SystemColors.Window;
            ultradefrag_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void chkdskgui_ll_MouseHover(object sender, EventArgs e)
        {
            chkdskgui_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            chkdskgui_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void chkdskgui_ll_MouseLeave(object sender, EventArgs e)
        {
            chkdskgui_ll.BackColor = System.Drawing.SystemColors.Window;
            chkdskgui_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void diskinfo_ll_MouseHover(object sender, EventArgs e)
        {
            diskinfo_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            diskinfo_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void diskinfo_ll_MouseLeave(object sender, EventArgs e)
        {
            diskinfo_ll.BackColor = System.Drawing.SystemColors.Window;
            diskinfo_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void lum_ll_MouseHover(object sender, EventArgs e)
        {
            lum_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            lum_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void lum_ll_MouseLeave(object sender, EventArgs e)
        {
            lum_ll.BackColor = System.Drawing.SystemColors.Window;
            lum_p.BackColor = System.Drawing.SystemColors.Window;
        }

        private void ph_ll_MouseHover(object sender, EventArgs e)
        {
            ph_ll.BackColor = System.Drawing.SystemColors.MenuBar;
            ph_p.BackColor = System.Drawing.SystemColors.MenuBar;
        }

        private void ph_ll_MouseLeave(object sender, EventArgs e)
        {
            ph_ll.BackColor = System.Drawing.SystemColors.Window;
            ph_p.BackColor = System.Drawing.SystemColors.Window;
        }
    }
}