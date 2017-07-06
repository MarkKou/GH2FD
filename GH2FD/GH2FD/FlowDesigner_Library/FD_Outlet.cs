using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Outlet : FD_Group
    {
        protected override string Object_Type { get { return "inlet"; } }

        public FD_Outlet(List<FD_Object> _Members)
            : base(_Members) { }

        private bool me_modified = false;
        private bool sp_modified = false;
        private bool vo_modified = false;
        private bool bp_modified = false;

        private int method;
        private double speed;
        private double volume;
        private int balance_prio;

        public int Method
        {
            get { return method; }
            set
            {
                method = value;
                me_modified = true;
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
        public double Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                vo_modified = true;
            }
        }
        public int Balance_Prio
        {
            get { return balance_prio; }
            set
            {
                balance_prio = value;
                bp_modified = true;
            }
        }

        protected override List<string> Property_string
        {
            get
            {
                List<string> p_str = new List<string>();

                if (me_modified)
                {
                    if (Method == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL INPUTTYPE \"流速\""); }
                        else 
                        { 
                        p_str.Add("property set GENERAL INPUTTYPE \"Flow speed\"");
                        }
                    }
                    if (Method == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL INPUTTYPE \"流量\""); }
                        else 
                        { 
                        p_str.Add("property set GENERAL INPUTTYPE \"Flow volume\"");
                        }
                    }
                }

                if (sp_modified) { p_str.Add("property set GENERAL INLETSPEED " + speed.ToString()); }
                if (vo_modified) { p_str.Add("property set GENERAL INLETAMOUNT " + volume.ToString()); }

                if (bp_modified)
                {
                    if (Method == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL BALANCEPRIORITY \"流速一定\""); }
                        else { p_str.Add("property set GENERAL BALANCEPRIORITY \"Keep flow speed\""); }
                    }
                    if (Method == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL BALANCEPRIORITY \"流量一定\""); }
                        else { p_str.Add("property set GENERAL BALANCEPRIORITY \"Keep flow volume\""); }
                    }
                }

                return p_str;
            }
        }
    }
}