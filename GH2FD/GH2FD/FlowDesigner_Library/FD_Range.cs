using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Range:FD_Setting
    {
        //Valiables
        int unit;

        double width;
        double deepth;
        double height;

        double offset_x;
        double offset_y;
        double offset_z;

        //Properties
        public int Unit
        {
            get { return unit; }
            set { unit = value;  }
        }

        public double Width
        {
            get { return width; }
            set { width = value;  }
        }

        public double Deepth
        {
            get { return deepth; }
            set { deepth = value;  }
        }

        public double Height
        {
            get { return height; }
            set { height = value;  }
        }

        public double Offset_X
        {
            get { return offset_x; }
            set { offset_x = value;  }
        }

        public double Offset_Y
        {
            get { return offset_y; }
            set { offset_y = value;  }
        }

        public double Offset_Z
        {
            get { return offset_z; }
            set { offset_z = value;  }
        }

        protected override List<string> Update_Strings
        {
            get
            {
                List<string> upstrs = new List<string>();

                upstrs.Add("range set unit " + unit.ToString());
                upstrs.Add("range set size " + width.ToString() + "," + deepth.ToString() + "," + height.ToString());
                upstrs.Add("range set offset " + offset_x.ToString() + "," + offset_y.ToString() + "," + offset_z.ToString());

                return upstrs;
            }
        }

        //Constructor
        public FD_Range(double w, double d, double h)
            : base()
        {
            unit = Convert.ToInt32(FD_Commander.Excute("range get unit"));

            width = w;
            deepth = d;
            height = h;

            offset_x = 0;
            offset_y = 0;
            offset_z = 0;
        }

        public FD_Range(double w, double d, double h,double ox, double oy, double oz)
            : base()
        {
            unit = Convert.ToInt32(FD_Commander.Excute("range get unit"));

            width = w;
            deepth = d;
            height = h;

            offset_x = ox;
            offset_y = oy;
            offset_z = oz;
        }

        public FD_Range(int u, double w, double d, double h)
            : base()
        {
            unit = u;

            width = w;
            deepth = d;
            height = h;

            offset_x = 0;
            offset_y = 0;
            offset_z = 0;
        }

        public FD_Range(int u, double w, double d, double h, double ox, double oy, double oz)
            : base()
        {
            unit = u;

            width = w;
            deepth = d;
            height = h;

            offset_x = ox;
            offset_y = oy;
            offset_z = oz;
        }

        public FD_Range()
            : base()
        {
            unit = Convert.ToInt32(FD_Commander.Excute("range get unit"));

            width = Convert.ToDouble(FD_Commander.Excute("range get size x"));
            deepth = Convert.ToDouble(FD_Commander.Excute("range get size y"));
            height = Convert.ToDouble(FD_Commander.Excute("range get size z"));

            offset_x = Convert.ToDouble(FD_Commander.Excute("range get offset x"));
            offset_y = Convert.ToDouble(FD_Commander.Excute("range get offset y"));
            offset_z = Convert.ToDouble(FD_Commander.Excute("range get offset z"));
        }
    }
}