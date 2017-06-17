using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Mesh : FD_Setting
    {
        //Constructor
        public FD_Mesh(int count)
            : base()
        {
            Mode = 0;
            Autocount = count;
        }

        public FD_Mesh(double distance)
            : base()
        {
            Mode = 1;
            Distance = distance;
        }

        public FD_Mesh(List<double> x, List<double> y, List<double> z)
            : base()
        {
            Mode = 2;
            X_Mesh = x;
            Y_Mesh = y;
            Z_Mesh = z;
        }

        //Variables
        public int Mode;
        public int Autocount;
        public double Distance;
        public List<double> X_Mesh;
        public List<double> Y_Mesh;
        public List<double> Z_Mesh;
        public bool Add_Model_Edge = true;
        public List<FD_Sub_Division> Sub_Divisions = new List<FD_Sub_Division>();
        //string something;

        //Properties
        public List<string> Creat_string
        {
            get
            {
                List<string> cs = new List<string>();

                if (Mode == 0)
                {
                    cs.Add("mesh mode autocount");
                    cs.Add("mesh autocount " + Autocount.ToString());
                }
                else if (Mode == 1)
                {
                    cs.Add("mesh mode autodistance");
                    cs.Add("mesh autodistance " + Distance.ToString());
                }
                else if (Mode == 2)
                {
                    cs.Add("mesh mode manual");
                    foreach (double item in X_Mesh) { cs.Add("mesh add x " + item.ToString()); }
                    foreach (double item in Y_Mesh) { cs.Add("mesh add y " + item.ToString()); }
                    foreach (double item in Z_Mesh) { cs.Add("mesh add z " + item.ToString()); }
                }

                return cs;
            }
        }

        public string Mesh_Mode
        {
            get
            {
                if (Mode == 0) { return "Auto Count"; }
                else if (Mode == 1) { return "Auto Distance"; }
                else if (Mode == 2) { return "Manual"; }
                else { return "Error"; }
            }
        }

        protected override List<string> Update_Strings
        {
            get
            {
                List<string> upstr = new List<string>();

                upstr.Add("mesh mode manual");
                upstr.Add("mesh select all");
                upstr.Add("mesh clear");

                upstr.AddRange(Creat_string);

                foreach(FD_Sub_Division sb in Sub_Divisions)
                {
                    upstr.AddRange(sb.Sub_Div_Strings);
                }

                if (Add_Model_Edge) { upstr.Add("mesh modeledge"); }

                return upstr;
            }
        }

        //Methods
        public void Creat_Mesh()
        {
            Clear_Meah();
            foreach (string item in Creat_string) { FD_Commander.Excute(item); }
            foreach (FD_Sub_Division sb in Sub_Divisions) { sb.Update(); }
            if (Add_Model_Edge) { FD_Commander.Excute("mesh modeledge"); }
        }

        public void Clear_Meah()
        {
            FD_Commander.Excute("mesh mode manual");
            FD_Commander.Excute("mesh select all");
            FD_Commander.Excute("mesh clear");
        }

        public class FD_Sub_Division
        {
            public double X0 = 0;
            public double X1 = 0;
            public double Y0 = 0;
            public double Y1 = 0;
            public double Z0 = 0;
            public double Z1 = 0;

            public int Scheme = 0;
            public int Mode = 0;

            public double Ratio = 1;

            double xcd = 0;
            double ycd = 0;
            double zcd = 0;

            public FD_Sub_Division(int scheme = 0, int mode = 0)
            {
                Scheme = scheme;
                Mode = mode;
            }

            public double X_Count_or_Distance
            {
                set { xcd = value; sub_x = true; }
                get { return xcd; }
            }

            public double Y_Count_or_Distance
            {
                set { ycd = value; sub_y = true; }
                get { return ycd; }
            }

            public double Z_Count_or_Distance
            {
                set { zcd = value; sub_z = true; }
                get { return zcd; }
            }

            public bool sub_x = false;
            public bool sub_y = false;
            public bool sub_z = false;

            public List<string> Sub_Div_Strings
            {
                get
                {
                    List<string> sdss = new List<string>();

                    string M_S = "";

                    if (Scheme == 0)
                    {
                        if (Mode == 0) { M_S = "split "; }
                        else if (Mode == 1) { M_S = "spliti "; }
                    }
                    else
                    {
                        if (Mode == 0) { sdss.Add("mesh setting div 2"); }
                        else if (Mode == 1) { sdss.Add("mesh setting div 1"); }

                        sdss.Add("mesh setting ratio " + Ratio.ToString());

                        if (Scheme == 1) { M_S = "splitr lower "; }
                        if (Scheme == 2) { M_S = "splitr both "; }
                        if (Scheme == 3) { M_S = "splitr higher "; }
                    }

                    if (sub_x)
                    {
                        sdss.Add("mesh select x " + X0.ToString() + " " + X1.ToString());
                        sdss.Add("mesh " + M_S + X_Count_or_Distance.ToString() + " 2");
                    }

                    if (sub_y)
                    {
                        sdss.Add("mesh select y " + Y0.ToString() + " " + Y1.ToString());
                        sdss.Add("mesh " + M_S + Y_Count_or_Distance.ToString() + " 2");
                    }

                    if (sub_z)
                    {
                        sdss.Add("mesh select z " + Z0.ToString() + " " + Z1.ToString());
                        sdss.Add("mesh " + M_S + Z_Count_or_Distance.ToString() + " 2");
                    }

                    return sdss;
                }
            }

            public void Update() { foreach (string item in Sub_Div_Strings) { FD_Commander.Excute(item); } }
        }
    }
}