using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace pcsm
{
    [Serializable()]
    public class log
    {        
        public static string GetFilenameYYYMMDD(string suffix, string extension)
        {
            return System.DateTime.Now.ToString("yyyy_MM_dd") + suffix + extension;
        }
        
        public static void WriteLog(String message)
        {
            PCS pcs = new PCS();
            string savelog = PCS.IniReadValue("main", "savelog");
            if (savelog == "true")
            {
                //just in case: we protect code with try.
                try
                {

                    string filename = "log\\" + GetFilenameYYYMMDD("_LOG", ".log");
                    if (!Directory.Exists("log"))
                    {
                        Directory.CreateDirectory("log");
                    }
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
                    string line = System.DateTime.Now.ToString() + "\t" + message;
                    sw.WriteLine(line);
                    sw.Close();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                return;
            }
        }
    }
}
