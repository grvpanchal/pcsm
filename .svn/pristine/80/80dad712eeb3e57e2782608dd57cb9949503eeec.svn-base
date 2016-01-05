using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace pcsm.Processes
{
    class RegCleaner
    {
        public static string problem
        {
            get;
            set;
        }

        public static string valueName
        {
            get;
            set;
        }

        public static string regKey
        {
            get;
            set;
        }

        public static void analyse(DataGridView dataGridView1, Label label1)
        {
            log.WriteLog("Maintainer Registry Cleaner analysis start");
            dataGridView1.Rows.Clear();
            PCS.process("lrc\\lrc.exe", " /analyse", true);

            if (File.Exists("settings\\regcleanresult.txt"))
            {
                System.Threading.Thread.Sleep(1000);
                string json = File.ReadAllText("settings\\regcleanresult.txt");
                JObject o = JObject.Parse(json);

                int count = (int)o["problems"];
                Global.registryerrors = count;
                label1.Text = "Registry Errors Found: " + count.ToString();
                if (count > 0)
                {
                    for (int i = 1; i <= count; i++)
                    {
                        JArray bdky = (JArray)o[i.ToString()];
                        if ((int)bdky[0] == 1)
                        {
                             problem = (string)bdky[1];
                             regKey = (string)bdky[4];
                             valueName = (string)bdky[5];
                            dataGridView1.Rows.Add(problem, regKey, valueName);
                        }
                    }
                }
                log.WriteLog("Maintainer Registry Cleaner End");
            }            
        }

        public static void repair()
        {
            PCS pcs = new PCS();
            log.WriteLog("Maintainer Registry Cleaner repair start");
            PCS.process("lrc\\lrc.exe", " /repair", true);      
            if(File.Exists("settings\\reglog.txt"))
                Global.registry = pcs.findword("settings\\reglog.txt", "Total problems found", 21);
            log.WriteLog("Maintainer Registry Cleaner repair end");
        }

        #region Read all Key in Section

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
            (
                [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
            // Note that because the key/value pars are returned as null-terminated
            // strings with the last string followed by 2 null-characters, we cannot
            // use StringBuilder.
                [In] IntPtr pReturnedString,
                [In] UInt32 nSize,
                [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
            );

        private static string[] GetAllKeysInIniFileSection(string strSectionName, string strIniFileName)
        {
            // Allocate in unmanaged memory a buffer of suitable size.
            // I have specified here the max size of 32767 as documentated 
            // in MSDN.
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            // Start with an array of 1 string only. 
            // Will embellish as we go along.
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(strSectionName, pBuffer, 1778, strIniFileName);

            // iStartAddress will point to the first character of the buffer,
            int iStartAddress = pBuffer.ToInt32();
            // iEndAddress will point to the last null char in the buffer.
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;
            //int iEndAddress = iStartAddress + 1000;
            // Navigate through pBuffer.
            while (iStartAddress < iEndAddress)
            {
                // Determine the current size of the array.
                int iArrayCurrentSize = strArray.Length;
                // Increment the size of the string array by 1.
                Array.Resize<string>(ref strArray, iArrayCurrentSize + 1);
                // Get the current string which starts at "iStartAddress".
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                // Insert "strCurrent" into the string array.
                strArray[iArrayCurrentSize] = strCurrent;
                // Make "iStartAddress" point to the next string.
                iStartAddress += (strCurrent.Length + 1);
            }

            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;

            return strArray;
        }

        #endregion

        public static void read_regsections(TreeView treeView1)
        {
            string[] sectionnames = { "Active X / COM", "Startup", "Fonts", "Application Info", "Drivers", "Help Files", "Sounds", "Application Paths", "Application Settings", "Shared DLL", "Recent Files" };
            string[] strArray = GetAllKeysInIniFileSection("sections", "settings\\regsections.ini");

            for (int i = 0; i < strArray.Length; i++)
            {
                string boolean = strArray[i].Substring(strArray[i].Length - 1);
                string value = strArray[i].Substring(0, (strArray[i].Length - 2));
                TreeNode ParentNode = new TreeNode();
                ParentNode.Name = value;
                ParentNode.Text = sectionnames[i];
                if (boolean == "1")
                {
                    ParentNode.Checked = true;

                }
                else
                {
                    ParentNode.Checked = false;
                }
                treeView1.Nodes.Add(ParentNode);

            }
        }

        public static void save_regsections(TreeView treeView1)
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                String value = treeView1.Nodes[i].Name;

                if (treeView1.Nodes[i].Checked)
                {
                    PCS.IniWriteValue("settings\\regsections.ini", "sections", value, "1");
                }
                else
                {
                    PCS.IniWriteValue("settings\\regsections.ini", "sections", value, "0");
                }

            }
        }
    }
}
