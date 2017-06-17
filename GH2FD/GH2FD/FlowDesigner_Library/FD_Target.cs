using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Target : FD_Group
    {
        protected override string Object_Type { get { return "advtarget"; } }

        public FD_Target(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private bool ty_modified = false;
        private bool sp_modified = false;
        private bool te_modified = false;
        private bool de_modified = false;

        //Valiable
        private int type;
        private double speed;
        private double temperature;
        private double density;

        //Properties
        public int Type
        {
            get { return type; }
            set
            {
                type = value;
                ty_modified = true;
            }
        }
        public double Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                sp_modified = true;
            }
        }
        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                te_modified = true;
            }
        }
        public double Density
        {
            get { return density; }
            set
            {
                density = value;
                de_modified = true;
            }
        }

        protected override List<string> Property_string
        {
            get
            {
                List<string> p_str = new List<string>();

                //Basic
                if (ty_modified)
                {
                    if (type == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set SCOND STYPE \"目標値\""); }
                        else { p_str.Add("property set SCOND STYPE \"Target\""); }
                    }
                    else if (type == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set SCOND STYPE \"均一化\""); }
                        else { p_str.Add("property set SCOND STYPE \"Equalize\""); }
                    }
                    else if (type == 2)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set SCOND STYPE \"使用しない\""); }
                        else { p_str.Add("property set SCOND STYPE \"Ignore\""); }
                    }
                }

                if (sp_modified) { p_str.Add("property set SCOND TSPEED " + speed.ToString()); }
                if (te_modified) { p_str.Add("property set SCOND TTEMP " + temperature.ToString()); }
                if (de_modified) { p_str.Add("property set SCOND TDENSITY " + density.ToString()); }

                return p_str;
            }
        }
    }
}