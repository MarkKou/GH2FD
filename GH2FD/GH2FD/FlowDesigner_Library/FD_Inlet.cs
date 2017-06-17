using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Inlet : FD_Group
    {
        protected override string Object_Type { get { return "outlet"; } }

        public FD_Inlet(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private bool me_modified = false;
        private bool sp_modified = false;
        private bool vo_modified = false;
        private bool bp_modified = false;

        private bool te_modified = false;
        private bool rh_modified = false;
        private bool co_modified = false;

        private bool o1_modified = false;
        private bool o2_modified = false;
        private bool o3_modified = false;

        //Valiable
        private int method;
        private double speed;
        private double volume;
        private int balance_prio;

        private double temperature;
        private double r_humidity;
        private double contamination;

        private double other1;
        private double other2;
        private double other3;

        //Properties
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

        public double Temperature
        {
            get { return temperature; ;}
            set
            {
                temperature = value;
                te_modified = true;
            }
        }
        public double R_Humidity
        {
            get { return r_humidity; }
            set
            {
                r_humidity = value;
                rh_modified = true;
            }
        }
        public double Contamination
        {
            get { return contamination; }
            set
            {
                contamination = value;
                co_modified = true;
            }
        }

        public double Other1
        {
            get { return other1; }
            set
            {
                other1 = value;
                o1_modified = true;
            }
        }
        public double Other2
        {
            get { return other2; }
            set
            {
                other2 = value;
                o2_modified = true;
            }
        }
        public double Other3
        {
            get { return other3; }
            set
            {
                other3 = value;
                o3_modified = true;
            }
        }

        protected override List<string> Property_string
        {
            get
            {
                List<string> p_str = new List<string>();

                //Basic
                if (me_modified)
                {
                    if (method == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL INPUTTYPE \"流速\""); }
                        else { p_str.Add("property set GENERAL INPUTTYPE \"Flow speed\""); }
                    }
                    else if (method == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL INPUTTYPE \"流量\""); }
                        else { p_str.Add("property set GENERAL INPUTTYPE \"Flow volume\""); }
                    }
                }

                if (sp_modified) { p_str.Add("property set GENERAL OUTLETSPEED " + speed.ToString()); }
                if (vo_modified) { p_str.Add("property set GENERAL OUTLETAMOUNT " + volume.ToString()); }

                if (bp_modified)
                {
                    if (balance_prio == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL BALANCEPRIORITY \"流速一定\""); }
                        else { p_str.Add("property set GENERAL BALANCEPRIORITY \"Keep flow speed\""); }
                    }
                    else if (balance_prio == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL BALANCEPRIORITY \"流量一定\""); }
                        else { p_str.Add("property set GENERAL BALANCEPRIORITY \"Keep flow volume\""); }
                    }
                }

                //Air properties
                if (te_modified) { p_str.Add("property set GENERAL TEMP " + temperature.ToString()); }
                if (rh_modified) { p_str.Add("property set GENERAL HUMIDITY " + r_humidity.ToString()); }
                if (co_modified) { p_str.Add("property set GENERAL POLLUTION " + contamination.ToString()); }

                //Other pollutions
                if (o1_modified) { p_str.Add("property set GENERAL OTHER1 " + other1.ToString()); }
                if (o2_modified) { p_str.Add("property set GENERAL OTHER2 " + other2.ToString()); }
                if (o3_modified) { p_str.Add("property set GENERAL OTHER3 " + other3.ToString()); }

                return p_str;
            }
        }
    }
}