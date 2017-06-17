using System;
using System.Collections.Generic;
using System.IO;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class EPW2CSV : GH_Component
    {
        public EPW2CSV()
            : base("EPW2CSV", "E→C",
                @"Generate a CSV file for 'Wind environment assessment' from a EPW file'",
                "FlowDesigner", Tools.sub_cate_05)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("File path of EPW file", "E", "File path of EPW file", GH_ParamAccess.item);
            //1
            pManager.AddTextParameter("File path of CSV file", "C", "File path of CSV file", GH_ParamAccess.item);
            //2
            pManager.AddNumberParameter("Setpoint of Heating", "H", "", GH_ParamAccess.item, -100.0);
            //3
            pManager.AddNumberParameter("Setpoint of Cooling", "C", "", GH_ParamAccess.item, 100.0);
            //4
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("Message", "M", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool go = false;
            DA.GetData(4, ref go);

            string message = "Waiting...";

            if (go)
            {
                string epw_path = "";
                string csv_path = "";
                double heat = 0;
                double cool = 0;

                DA.GetData(0, ref epw_path);
                DA.GetData(1, ref csv_path);
                DA.GetData(2, ref heat);
                DA.GetData(3, ref cool);

                List<double>[] wind_speed_all = new List<double>[16];

                for (int i = 0; i < 16; i++) { wind_speed_all[i] = new List<double>(); }

                FileStream fs = new FileStream(epw_path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.ASCII);

                string strLine = "";
                int counter = 0;

                while ((strLine = sr.ReadLine()) != null)
                {
                    if (counter >= 8)
                    {
                        string[] aryLine = strLine.Split(',');
                        double temperature = Convert.ToDouble(aryLine[6]);
                        if (temperature >= heat && temperature <= cool)
                        {
                            double speed = Convert.ToDouble(aryLine[21]);
                            if (speed != 0)
                            {
                                wind_speed_all[Degree2Dire(Convert.ToDouble(aryLine[20]))].Add(speed);
                            }
                        }
                    }
                    counter++;
                }
                sr.Close();
                fs.Close();

                double[] average_speed_all = new double[16];
                double[] frequency_all = new double[16];

                for (int i = 0; i < 16; i++)
                {
                    double sum = 0;
                    foreach (double ws in wind_speed_all[i]) { sum += ws; }
                    average_speed_all[i] = sum / wind_speed_all[i].Count;
                    frequency_all[i] = wind_speed_all[i].Count / 87.6;
                }

                FileInfo fi = new FileInfo(csv_path);
                if (!fi.Directory.Exists) { fi.Directory.Create(); }

                FileStream fs2 = new FileStream(csv_path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs2, System.Text.Encoding.ASCII);

                sw.WriteLine(@"UnitSystem,0,The International System of Units,,,,,,");
                sw.WriteLine(@"Observation point height(m),30,,,,,,,");
                sw.WriteLine(@"Direction # ,Direction,Wind speed (m/s),Wind speed pattern,Terrain type,Exponent power,Generation frequency (A) (%),Weibull coef.(scale) (m/s),Weibull coef.(shape)");
                sw.WriteLine(@"1,N," + average_speed_all[0].ToString() + ",Pattern,Suburban,-," + frequency_all[0].ToString() + ",0,0");
                sw.WriteLine(@"2,NNE," + average_speed_all[1].ToString() + ",Pattern,Suburban,-," + frequency_all[1].ToString() + ",0,0");
                sw.WriteLine(@"3,NE," + average_speed_all[2].ToString() + ",Pattern,Suburban,-," + frequency_all[2].ToString() + ",0,0");
                sw.WriteLine(@"4,ENE," + average_speed_all[3].ToString() + ",Pattern,Suburban,-," + frequency_all[3].ToString() + ",0,0");
                sw.WriteLine(@"5,E," + average_speed_all[4].ToString() + ",Pattern,Suburban,-," + frequency_all[4].ToString() + ",0,0");
                sw.WriteLine(@"6,ESE," + average_speed_all[5].ToString() + ",Pattern,Suburban,-," + frequency_all[5].ToString() + ",0,0");
                sw.WriteLine(@"7,SE," + average_speed_all[6].ToString() + ",Pattern,Suburban,-," + frequency_all[6].ToString() + ",0,0");
                sw.WriteLine(@"8,SSE," + average_speed_all[7].ToString() + ",Pattern,Suburban,-," + frequency_all[7].ToString() + ",0,0");
                sw.WriteLine(@"9,S," + average_speed_all[8].ToString() + ",Pattern,Suburban,-," + frequency_all[8].ToString() + ",0,0");
                sw.WriteLine(@"10,SSW," + average_speed_all[9].ToString() + ",Pattern,Suburban,-," + frequency_all[9].ToString() + ",0,0");
                sw.WriteLine(@"11,SW," + average_speed_all[10].ToString() + ",Pattern,Suburban,-," + frequency_all[10].ToString() + ",0,0");
                sw.WriteLine(@"12,WSW," + average_speed_all[11].ToString() + ",Pattern,Suburban,-," + frequency_all[11].ToString() + ",0,0");
                sw.WriteLine(@"13,W," + average_speed_all[12].ToString() + ",Pattern,Suburban,-," + frequency_all[12].ToString() + ",0,0");
                sw.WriteLine(@"14,WNW," + average_speed_all[13].ToString() + ",Pattern,Suburban,-," + frequency_all[13].ToString() + ",0,0");
                sw.WriteLine(@"15,NW," + average_speed_all[14].ToString() + ",Pattern,Suburban,-," + frequency_all[14].ToString() + ",0,0");
                sw.WriteLine(@"16,NNW," + average_speed_all[15].ToString() + ",Pattern,Suburban,-," + frequency_all[15].ToString() + ",0,0");

                sw.Close();
                fs2.Close();

                message = "Done";
            }

            DA.SetData(0, message);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.EPW_CSV;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{d06c2259-6cd4-4d32-845a-1896649d3d12}"); }
        }

        private int Degree2Dire(double degree)
        {
            degree += 11.25;
            degree = degree % 360.0;
            if (degree < 0) { degree += 360; }
            degree = degree / 22.5;
            int dire = Convert.ToInt32(Math.Floor(degree));
            if (dire > 15) { dire = 0; }
            return dire;
        }
    }
}