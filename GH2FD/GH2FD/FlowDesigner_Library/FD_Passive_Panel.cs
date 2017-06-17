using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Passive_Panel : FD_Group
    {
        protected override string Object_Type { get { return "heatobj"; } }

        public FD_Passive_Panel(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private int attribute;
        private double trans;
        private string matprp;
        private double thickness;
        private double heatbyvolume;
        private double transplus;
        private double transminus;
        private double temp;

        //Valiables
        private bool attribute_modified;
        private bool trans_modified;
        private bool matprp_modified;
        private bool thickness_modified;
        private bool heatbyvolume_modified;
        private bool transplus_modified;
        private bool transminus_modified;
        private bool temp_modified;

        //Properties
        public int Attribute
        {
            set
            {
                attribute = value;
                attribute_modified = true;
            }
            get { return attribute; }
        }
        public double Heat_Transmissivity
        {
            set
            {
                trans = value;
                trans_modified = true;
            }
            get { return trans; }
        }
        public string Material
        {
            set
            {
                matprp = value;
                matprp_modified = true;
            }
            get { return matprp; }
        }
        public double Thickness
        {
            set
            {
                thickness = value;
                thickness_modified = true;
            }
            get { return thickness; }
        }
        public double Heat_Generation
        {
            set
            {
                heatbyvolume = value;
                heatbyvolume_modified = true;
            }
            get { return heatbyvolume; }
        }
        public double Heat_Transfer_Coef_Plus
        {
            set
            {
                transplus = value;
                transplus_modified = true;
            }
            get { return transplus; }
        }
        public double Heat_Transfer_Coef_Minus
        {
            set
            {
                transminus = value;
                transminus_modified = true;
            }
            get { return transminus; }
        }
        public double Initial_Temperature
        {
            set
            {
                temp = value;
                temp_modified = true;
            }
            get { return temp; }
        }

        protected override List<string> Property_string
        {
            get
            {
                List<string> p_str = new List<string>();

                if (attribute_modified)
                {
                    if (attribute == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general attribute \"考慮しない（断熱）\""); }
                        else { p_str.Add("property set general attribute \"Adiabatic (panel)\""); }
                    }
                    else if (attribute == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general attribute \"熱通過率\""); }
                        else { p_str.Add("property set general attribute \"Heat transmissivity\""); }
                    }
                    else if (attribute == 2)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general attribute \"厚みを考慮\""); }
                        else { p_str.Add("property set general attribute \"Pseudo solid\""); }
                    }
                }

                if (trans_modified) { p_str.Add("property set general trans " + trans.ToString()); }

                if (matprp_modified) { p_str.Add("property set general matprp " + matprp); }
                if (thickness_modified) { p_str.Add("property set general thickness " + thickness.ToString()); }
                if (heatbyvolume_modified) { p_str.Add("property set general heatbyvolume " + heatbyvolume.ToString()); }
                if (transplus_modified) { p_str.Add("property set general transplus " + transplus.ToString()); }
                if (transminus_modified) { p_str.Add("property set general transminus " + transminus.ToString()); }
                if (temp_modified) { p_str.Add("property set initial temp " + temp.ToString()); }

                return p_str;
            }
        }
    }
}