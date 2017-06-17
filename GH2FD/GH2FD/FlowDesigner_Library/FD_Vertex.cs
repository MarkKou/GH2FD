using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Vertex
    {
        private double x;
        private double y;
        private double z;

        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double Z { get { return z; } }

        public FD_Vertex(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public FD_Vertex(double _x, double _y)
        {
            x = _x;
            y = _y;
            z = 0;
        }

        public string CreateString
        {
            get
            {
                return x.ToString() + "," + y.ToString() + "," + z.ToString() + " ";
            }
        }
    }
}