using System;
using System.Collections.Generic;
using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
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
            pManager.AddTextParameter("File path of EPW file", "EP", "File path of EPW file", GH_ParamAccess.item);
            //1
            pManager.AddTextParameter("File path of CSV file", "CP", "File path of CSV file", GH_ParamAccess.item);
            //2
            pManager.AddIntegerParameter("Begining Month", "BM", "", GH_ParamAccess.item, 1);
            //3
            pManager.AddIntegerParameter("Begining Day", "BD", "", GH_ParamAccess.item, 1);
            //4
            pManager.AddIntegerParameter("End Month", "EM", "", GH_ParamAccess.item, 12);
            //5
            pManager.AddIntegerParameter("End Day", "ED", "", GH_ParamAccess.item, 31);
            //6
            pManager.AddIntegerParameter("Begining Hour", "BH", "0 ~ 23", GH_ParamAccess.item, 0);
            //7
            pManager.AddIntegerParameter("End Hour", "EH", "1 ~ 24", GH_ParamAccess.item, 24);
            //8
            pManager.AddNumberParameter("Reference Height", "RH", "", GH_ParamAccess.item, 10);
            //9
            pManager.AddIntegerParameter("Terrain Type", "TT", "Terrain Type\r\n" +
                "    1: Elevation independent\r\n" +
            "    2: Major city center\r\n" +
            "    3: Major city suburban\r\n" +
            "    4: Suburban\r\n" +
            "    5: Plane\r\n", GH_ParamAccess.item, 3);
            Param_Integer p9 = (Param_Integer)pManager[9];
            p9.AddNamedValue("Elevation independent", 1);
            p9.AddNamedValue("Major city center", 2);
            p9.AddNamedValue("Major city suburban", 3);
            p9.AddNamedValue("Suburban", 4);
            p9.AddNamedValue("Plane", 5);
            //10
            pManager.AddBooleanParameter("English or Japanses", "EJ", "True: English\r\nFlase: Japanese", GH_ParamAccess.item, true);
            //11
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
            DA.GetData(11, ref go);

            string message = "Waiting...";

            if (go)
            {
                //Get parameters

                string epw_path = "";
                string csv_path = "";
                DA.GetData(0, ref epw_path);
                DA.GetData(1, ref csv_path);

                int bm = 0;
                int bd = 0;
                int em = 0;
                int ed = 0;
                DA.GetData(2, ref bm);
                DA.GetData(3, ref bd);
                DA.GetData(4, ref em);
                DA.GetData(5, ref ed);

                int day_from = Day_of_Year(bm - 1, bd - 1);
                int day_to = Day_of_Year(em - 1, ed - 1);

                int hour_from = 0;
                int hour_to = 0;
                DA.GetData(6, ref hour_from);
                DA.GetData(7, ref hour_to);
                hour_to -= 1;

                double rheight = 0;
                DA.GetData(8, ref rheight);

                int ttype = 0;
                DA.GetData(9, ref ttype);

                bool ej = true;
                DA.GetData(10, ref ej);

                int[,] wind_directions = new int[365, 24];
                double[,] wind_speeds = new double[365, 24];


                //Convert epw file to data table

                FileStream fs = new FileStream(epw_path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.ASCII);

                string strLine = "";
                int counter = 0;

                List<int> directions = new List<int>();
                List<double> speeds = new List<double>();

                while ((strLine = sr.ReadLine()) != null)
                {
                    if (counter >= 8)
                    {
                        string[] aryLine = strLine.Split(',');
                        double degree = Convert.ToDouble(aryLine[20]);
                        directions.Add(Degree2Dire(degree));
                        double speed = Convert.ToDouble(aryLine[21]);
                        speeds.Add(speed);
                    }
                    counter++;
                }

                sr.Close();
                fs.Close();

                counter = 0;

                for (int d = 0; d < 365; d++)
                {
                    for (int h = 0; h < 24; h++)
                    {
                        wind_directions[d, h] = directions[counter];
                        wind_speeds[d, h] = speeds[counter];
                        counter++;
                    }
                }

                List<double>[] wind_speed_all = new List<double>[16];
                for (int i = 0; i < 16; i++) { wind_speed_all[i] = new List<double>(); }

                double[] average_speed_all = new double[16];
                double[] frequency_all = new double[16];

                day_loop();

                void day_loop()
                {
                    if (day_from <= day_to)
                    {
                        for (int d = day_from; d <= day_to; d++) { hour_loop(d); }
                    }
                    else
                    {
                        for (int d = 0; d <= day_to; d++) { hour_loop(d); }
                        for (int d = day_from; d < 365; d++) { hour_loop(d); }
                    }
                }

                void hour_loop(int d)
                {
                    if (hour_from <= hour_to)
                    {
                        for (int h = hour_from; h <= hour_to; h++) { wind_speed_all[wind_directions[d, h]].Add(wind_speeds[d, h]); }
                    }
                    else
                    {
                        for (int h = 0; h <= hour_to; h++) { wind_speed_all[wind_directions[d, h]].Add(wind_speeds[d, h]); }
                        for (int h = hour_from; h < 24; h++) { wind_speed_all[wind_directions[d, h]].Add(wind_speeds[d, h]); }
                    }
                }

                double all_count = 0;

                for (int i = 0; i < 16; i++)
                { all_count += wind_speed_all[i].Count; }

                for (int i = 0; i < 16; i++)
                {
                    double sum = 0;

                    if (wind_speed_all[i].Count == 0) { average_speed_all[i] = 0; }
                    else
                    {
                        foreach (double ws in wind_speed_all[i]) { sum += ws; }
                        average_speed_all[i] = sum / wind_speed_all[i].Count;
                    }

                    frequency_all[i] = 100 * wind_speed_all[i].Count / all_count;
                }

                FileInfo fi = new FileInfo(csv_path);
                if (!fi.Directory.Exists) { fi.Directory.Create(); }

                FileStream fs2 = new FileStream(csv_path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw;

                if (ej)
                {
                    sw = new StreamWriter(fs2, System.Text.Encoding.ASCII);

                    string[] ttlist = new string[] { "Elevation independent", "Major city center", "Major city suburban", "Suburban", "Plane" };

                    sw.WriteLine(@"UnitSystem,0,The International System of Units,,,,,,");
                    sw.WriteLine(@"Observation point height(m)," + rheight.ToString() + ",,,,,,,");
                    sw.WriteLine(@"Direction # ,Direction,Wind speed (m/s),Wind speed pattern,Terrain type,Exponent power,Generation frequency (A) (%),Weibull coef.(scale) (m/s),Weibull coef.(shape)");
                    sw.WriteLine(@"1,N," + average_speed_all[0].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[0].ToString() + ",0,0");
                    sw.WriteLine(@"2,NNE," + average_speed_all[1].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[1].ToString() + ",0,0");
                    sw.WriteLine(@"3,NE," + average_speed_all[2].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[2].ToString() + ",0,0");
                    sw.WriteLine(@"4,ENE," + average_speed_all[3].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[3].ToString() + ",0,0");
                    sw.WriteLine(@"5,E," + average_speed_all[4].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[4].ToString() + ",0,0");
                    sw.WriteLine(@"6,ESE," + average_speed_all[5].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[5].ToString() + ",0,0");
                    sw.WriteLine(@"7,SE," + average_speed_all[6].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[6].ToString() + ",0,0");
                    sw.WriteLine(@"8,SSE," + average_speed_all[7].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[7].ToString() + ",0,0");
                    sw.WriteLine(@"9,S," + average_speed_all[8].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[8].ToString() + ",0,0");
                    sw.WriteLine(@"10,SSW," + average_speed_all[9].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[9].ToString() + ",0,0");
                    sw.WriteLine(@"11,SW," + average_speed_all[10].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[10].ToString() + ",0,0");
                    sw.WriteLine(@"12,WSW," + average_speed_all[11].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[11].ToString() + ",0,0");
                    sw.WriteLine(@"13,W," + average_speed_all[12].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[12].ToString() + ",0,0");
                    sw.WriteLine(@"14,WNW," + average_speed_all[13].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[13].ToString() + ",0,0");
                    sw.WriteLine(@"15,NW," + average_speed_all[14].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[14].ToString() + ",0,0");
                    sw.WriteLine(@"16,NNW," + average_speed_all[15].ToString() + ",Pattern," + ttlist[ttype - 1] + ",-," + frequency_all[15].ToString() + ",0,0");
                }
                else
                {
                    sw = new StreamWriter(fs2, System.Text.Encoding.GetEncoding(932));

                    string[] ttlist = new string[] { "高さ方向一定", "大都市中心部", "大都市周辺市街地", "郊外住宅地", "平原" };

                    sw.WriteLine(@"UnitSystem,0,The International System of Units,,,,,,");
                    sw.WriteLine(@"観測点高さ(m)," + rheight.ToString() + ",,,,,,,");
                    sw.WriteLine(@"方位No.,方位,風速 (m/s),風速パターン,パターン選択,べき数,発生頻度(A) (%),ワイブル係数(C) (m/s),ワイブル係数(K)");
                    sw.WriteLine(@"1,北," + average_speed_all[0].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[0].ToString() + ",0,0");
                    sw.WriteLine(@"2,北北東," + average_speed_all[1].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[1].ToString() + ",0,0");
                    sw.WriteLine(@"3,北東," + average_speed_all[2].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[2].ToString() + ",0,0");
                    sw.WriteLine(@"4,東北東," + average_speed_all[3].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[3].ToString() + ",0,0");
                    sw.WriteLine(@"5,東," + average_speed_all[4].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[4].ToString() + ",0,0");
                    sw.WriteLine(@"6,東南東," + average_speed_all[5].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[5].ToString() + ",0,0");
                    sw.WriteLine(@"7,南東," + average_speed_all[6].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[6].ToString() + ",0,0");
                    sw.WriteLine(@"8,南南東," + average_speed_all[7].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[7].ToString() + ",0,0");
                    sw.WriteLine(@"9,南," + average_speed_all[8].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[8].ToString() + ",0,0");
                    sw.WriteLine(@"10,南南西," + average_speed_all[9].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[9].ToString() + ",0,0");
                    sw.WriteLine(@"11,南西," + average_speed_all[10].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[10].ToString() + ",0,0");
                    sw.WriteLine(@"12,西南西," + average_speed_all[11].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[11].ToString() + ",0,0");
                    sw.WriteLine(@"13,西," + average_speed_all[12].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[12].ToString() + ",0,0");
                    sw.WriteLine(@"14,西北西," + average_speed_all[13].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[13].ToString() + ",0,0");
                    sw.WriteLine(@"15,北西," + average_speed_all[14].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[14].ToString() + ",0,0");
                    sw.WriteLine(@"16,北北西," + average_speed_all[15].ToString() + ",パターン選択," + ttlist[ttype - 1] + ",-," + frequency_all[15].ToString() + ",0,0");
                }

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

        private int Day_of_Year(int month, int day)
        {
            int dy = day;

            if (month > 0) { dy += 31; }
            if (month > 1) { dy += 28; }
            if (month > 2) { dy += 31; }
            if (month > 3) { dy += 30; }
            if (month > 4) { dy += 31; }
            if (month > 5) { dy += 30; }
            if (month > 6) { dy += 31; }
            if (month > 7) { dy += 31; }
            if (month > 8) { dy += 30; }
            if (month > 9) { dy += 31; }
            if (month > 10) { dy += 30; }

            return dy;
        }
    }
}