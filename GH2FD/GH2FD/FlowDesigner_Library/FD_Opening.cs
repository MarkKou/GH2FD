using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Opening : FD_Group
    {
        protected override string Object_Type { get { return "opening"; } }

        public FD_Opening(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private bool ty_modified = false;
        private bool pr_modified = false;
        private bool pe_modified = false;
        private bool fr_modified = false;
        private bool pd_modified = false;

        private bool te_modified = false;
        private bool rh_modified = false;
        private bool co_modified = false;

        private bool o1_modified = false;
        private bool o2_modified = false;
        private bool o3_modified = false;

        //Valiable
        private int type;
        private double pressure;
        private double perf_ratio;
        private double fri_coef;
        private double pre_drop;

        private double temperature;
        private double r_humidity;
        private double contamination;

        private double other1;
        private double other2;
        private double other3;

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
        public double Pressure
        {
            get { return pressure; }
            set
            {
                pressure = value;
                pr_modified = true;
            }
        }
        public double Perf_ratio
        {
            get { return perf_ratio; }
            set
            {
                perf_ratio = value;
                pe_modified = true;
            }
        }
        public double Fri_coef
        {
            get { return fri_coef; }
            set
            {
                fri_coef = value;
                fr_modified = true;
            }
        }
        public double Pre_drop
        {
            get { return pre_drop; }
            set
            {
                pre_drop = value;
                pd_modified = true;
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

                //Optional
                if (ty_modified)
                {
                    if (type == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set OPTIONAL OPTTYPE \"自由流出入\""); }
                        else { p_str.Add("property set OPTIONAL OPTTYPE \"Free in and out flow\""); }
                    }
                    else if (type == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set OPTIONAL OPTTYPE \"圧力規定\""); }
                        else { p_str.Add("property set OPTIONAL OPTTYPE \"Specified pressure\""); }
                    }
                    else if (type == 2)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set OPTIONAL OPTTYPE \"開口率\""); }
                        else { p_str.Add("property set OPTIONAL OPTTYPE \"Perforated ratio\""); }
                    }
                }

                if (pr_modified) { p_str.Add("property set OPTIONAL PRESSURE " + pressure.ToString()); }

                if (pe_modified || fr_modified || pd_modified)
                {
                    if (!pe_modified) { Perf_ratio = 100; }
                    if (!fr_modified) { Fri_coef = 0; }
                    if (!pd_modified) { Pre_drop = 2; }

                    p_str.Add("property set OPTIONAL COEFF \"" + Perf_ratio.ToString() + " " + Fri_coef.ToString() + " " + Pre_drop.ToString() + "\"");
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