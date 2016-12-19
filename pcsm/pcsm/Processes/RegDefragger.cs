using System;

namespace pcsm.Processes
{
    class RegDefragger
    {        
        public static void analyse(System.Windows.Forms.DataVisualization.Charting.Chart chart1)
        {            
            chart1.Series.Clear();
            PCS.Process("lro\\lro.exe", " /analyse", false);
            string oldregistrysize = PCS.IniReadValue("settings\\regdefragresult.ini", "main", "oldregistrysize");
            string newregistrysize = PCS.IniReadValue("settings\\regdefragresult.ini", "main", "newregistrysize");
            string diffregistrysize = PCS.IniReadValue("settings\\regdefragresult.ini", "main", "diffregistrysize");
            Global.newRegistrySize = Convert.ToDouble(newregistrysize);
            Global.oldRegistrySize = Convert.ToDouble(oldregistrysize);
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";

            series1.Points.AddXY("Registry Size: " + newregistrysize + " MB", Convert.ToDouble(newregistrysize));
            series1.Points.AddXY("Size Saving: " + diffregistrysize + " MB", Convert.ToDouble(diffregistrysize));
            chart1.Series.Add(series1);
            chart1.Series[0]["PieLabelStyle"] = "Disabled";
        }

        public static void defrag()
        {   
            PCS.Process("lro\\lro.exe", " /optimize", false);
        }
    }
}
