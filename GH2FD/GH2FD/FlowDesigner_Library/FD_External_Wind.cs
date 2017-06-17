using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_External_Wind : FD_Setting
    {
        public bool useouterairflow = false;
        public bool useouterflowcontamination = false;
        public bool useouterflowotherdensity1 = false;
        public bool useouterflowotherdensity2 = false;
        public bool useouterflowotherdensity3 = false;
        public bool useouterflowdrag = false;

        public int outerairflowsettype = 0;
        public int outerairflowtype = 0;
        public int outerairflowdirtype = 0;
        public int outerairflowdir = 0;

        public double outerairflowmanualdir = 0;
        public double outerairflowspeed = 0;
        public double outerairflowheight = 10;
        public double outerairflowtemperature = 25;
        public double outerairflowhumidity = 60;
        public double outerflowcontamination = 0;
        public double outerflowotherdensity1 = 0;
        public double outerflowotherdensity2 = 0;
        public double outerflowotherdensity3 = 0;

        protected override List<string> Update_Strings
        {
            get
            {
                List<string> s_string = new List<string>();

                if (useouterairflow)
                {
                    s_string.Add("plugin analyzeplug setraw useouterairflow true");
                    s_string.Add("plugin analyzeplug setraw outerairflowtype " + outerairflowtype.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowdirtype " + outerairflowdirtype.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowdir " + outerairflowdir.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowmanualdir " + outerairflowmanualdir.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowspeed " + outerairflowspeed.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowheight " + outerairflowheight.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowtemperature " + outerairflowtemperature.ToString());
                    s_string.Add("plugin analyzeplug setraw outerairflowhumidity " + outerairflowhumidity.ToString());
                }
                else
                { s_string.Add("plugin analyzeplug setraw useouterairflow false"); }

                if (useouterflowcontamination)
                {
                    s_string.Add("plugin analyzeplug setraw useouterflowcontamination true");
                    s_string.Add("plugin analyzeplug setraw outerflowcontamination " + outerflowcontamination.ToString());
                }
                else
                { s_string.Add("plugin analyzeplug setraw useouterflowcontamination false"); }

                if (useouterflowotherdensity1)
                {
                    s_string.Add("plugin analyzeplug setraw useouterflowotherdensity1 true");
                    s_string.Add("plugin analyzeplug setraw outerflowotherdensity1 " + outerflowotherdensity1.ToString());
                }
                else
                { s_string.Add("plugin analyzeplug setraw useouterflowotherdensity1 false"); }

                if (useouterflowotherdensity2)
                {
                    s_string.Add("plugin analyzeplug setraw useouterflowotherdensity2 true");
                    s_string.Add("plugin analyzeplug setraw outerflowotherdensity2 " + outerflowotherdensity2.ToString());
                }
                else
                { s_string.Add("plugin analyzeplug setraw useouterflowotherdensity2 false"); }

                if (useouterflowotherdensity3)
                {
                    s_string.Add("plugin analyzeplug setraw useouterflowotherdensity3 true");
                    s_string.Add("plugin analyzeplug setraw outerflowotherdensity3 " + outerflowotherdensity3.ToString());
                }
                else
                { s_string.Add("plugin analyzeplug setraw useouterflowotherdensity3 false"); }

                if (useouterflowdrag)
                {
                    s_string.Add("plugin analyzeplug setraw useouterflowdrag true");
                }
                else
                { s_string.Add("plugin analyzeplug setraw useouterflowdrag false"); }

                return s_string;
            }
        }

        public FD_External_Wind() : base() { }

        public FD_External_Wind(int Wind_Direction, double Wind_Speed)
            : base()
        {
            useouterairflow = true;
            outerairflowdirtype = 0;
            outerairflowdir = Wind_Direction;
            outerairflowspeed = Wind_Speed;
        }

        public FD_External_Wind(double Wind_Direction, double Wind_Speed)
            : base()
        {
            useouterairflow = true;
            outerairflowdirtype = 1;
            outerairflowmanualdir = Wind_Direction;
            outerairflowspeed = Wind_Speed;
        }
    }
}