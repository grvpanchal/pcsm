using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;

namespace pcsm
{
    public partial class Download : Form
    {
        PCS pcs = new PCS();
        public class Global
        {
            public static string program = "";
            
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://sourceforge.net"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Download(string section)
        {
            Global.program = section;
            InitializeComponent();
            
        }
               
        private void Extract()
        {
            string link = PCS.IniReadValue(Global.program, "link");
            string filename = PCS.IniReadValue(Global.program, "filename");
            string folder = PCS.IniReadValue(Global.program, "folder");
            string zipToUnpack = filename;
            string unpackDirectory = folder;
            using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
            {                
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            PCS.IniWriteValue(Global.program, "installed", "1");
            File.Delete(filename);
        }

        public void download(string sec)
        {
            string link = PCS.IniReadValue(sec, "link");
            string filename = PCS.IniReadValue(sec, "filename");
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri(link), filename);

        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (Global.program == "sre")
            {
                Thread.Sleep(1000);
                string folder = PCS.IniReadValue(Global.program, "folder");
                Directory.CreateDirectory(folder);
                File.Copy("sre.exe", folder + "\\sre.exe", true);
                PCS.IniWriteValue(Global.program, "installed", "1");
                File.Delete("sre.exe");
               
            }   
            
            else
            {
                Extract();
            }
            this.DialogResult = DialogResult.OK;
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
            progressBar1.Value = (int)e.BytesReceived / 100;
        }

        private void Download_Load(object sender, EventArgs e)
        {
            if (CheckForInternetConnection() == false)
            {
                MessageBox.Show("Sorry cannot install. Reason: No Internet Connection.");
                this.DialogResult = DialogResult.Abort;
                this.Close();

            }
            else
            {
                download(Global.program);
            }
        }
               
    }
}

