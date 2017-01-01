using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace pcsm.Processes
{
    class RegCleaner
    {
        public static string Problem
        {
            get;
            set;
        }

        public static string ValueName
        {
            get;
            set;
        }

        public static string RegKey
        {
            get;
            set;
        }

        public static void Analyse(DataGridView dataGridView1, Label label1)
        {   
            dataGridView1.Rows.Clear();
            PCS.Process(Global.regCleanExec, " /analyse", true);

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
                             Problem = (string)bdky[1];
                             RegKey = (string)bdky[4];
                             ValueName = (string)bdky[5];
                            dataGridView1.Rows.Add(Problem, RegKey, ValueName);
                        }
                    }
                }
            }            
        }

        public static void Repair()
        {
            PCS pcs = new PCS();
            PCS.Process(Global.regCleanExec, " /repair", true);      
            if(File.Exists("settings\\reglog.txt"))
                Global.registry = pcs.FindWord("settings\\reglog.txt", "Total problems found", 21);
            
        }

        public static void ReadRegSections(TreeView treeView1)
        {   
            string[] sectionnames = { "Active X / COM", "Startup", "Fonts", "Application Info", "Drivers", "Help Files", "Sounds", "Application Paths", "Application Settings", "Shared DLL", "Recent Files" };
            string[] strArray = PCS.GetAllKeysInIniFileSection("sections", Global.regCleanConf);

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

        public static void SaveRegSections(TreeView treeView1)
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                String value = treeView1.Nodes[i].Name;

                if (treeView1.Nodes[i].Checked)
                {
                    PCS.IniWriteValue(Global.regCleanConf, "sections", value, "1");
                }
                else
                {
                    PCS.IniWriteValue(Global.regCleanConf, "sections", value, "0");
                }

            }
        }
    }
}
