using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Perf_Panel : FD_Group
    {
        protected override string Object_Type { get { return "punching"; } }

        public FD_Perf_Panel(List<FD_Object> _Members)
            : base(_Members) { }

        private bool pe_modified = false;
        private bool fr_modified = false;
        private bool pr_modified = false;
        private bool ma_modified = false;

        private double perf_ratio;
        private double fri_coef;
        private double pre_drop;
        private int macro_model;

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
                pr_modified = true;
            }
        }

        public int Macro_model
        {
            get { return macro_model; }
            set
            {
                macro_model = value;
                ma_modified = true;
            }
        }

        protected override List<string> Property_string
        {
            get
            {
                List<string> p_str = new List<string>();

                if (pe_modified || fr_modified || pr_modified)
                {
                    if (!pe_modified) { Perf_ratio = 100; }
                    if (!fr_modified) { Fri_coef = 0; }
                    if (!pr_modified) { Pre_drop = 2; }

                    p_str.Add("property set GENERAL COEFF \"" + Perf_ratio.ToString() + " " + Fri_coef.ToString() + " " + Pre_drop.ToString() + "\"");
                }

                if (ma_modified) { p_str.Add("property set GENERAL MACROMODEL " + Macro_model.ToString()); }
                return p_str;
            }
        }
    }
}