using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Passive_Cube : FD_Group
    {
        protected override string Object_Type { get { return "heatobj"; } }

        public FD_Passive_Cube(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private bool attribute_modified = false;

        private bool shetobj_modified = false;
        private bool stemp_modified = false;

        private bool matprp_modified = false;
        private bool heatvalue_modified = false;
        private bool trans_modified = false;

        private bool fldprp_modified = false;
        private bool fldapl_modified = false;
        private bool humidity_modified = false;
        private bool pollution_modified = false;
        private bool other1_modified = false;
        private bool other2_modified = false;
        private bool other3_modified = false;
        private bool ventarea_modified = false;

        private bool inittemp_modified = false;
        private bool inithumidity_modified = false;
        private bool initpollution_modified = false;
        private bool initother1_modified = false;
        private bool initother2_modified = false;
        private bool initother3_modified = false;

        //Valiable
        private int attribute;

        private int shetobj;
        private double stemp;

        private string matprp;
        private double heatvalue;
        private double trans;

        private string fldprp;
        private int fldapl;
        private double humidity;
        private double pollution;
        private double other1;
        private double other2;
        private double other3;
        private int ventarea;

        private double inittemp;
        private double inithumidity;
        private double initpollution;
        private double initother1;
        private double initother2;
        private double initother3;

        //Properties
        public int Attribute
        {
            get { return attribute; }
            set
            {
                attribute = value;
                attribute_modified = true;
            }
        }

        public int Surface_Heat_Generation
        {
            get { return shetobj; }
            set
            {
                shetobj = value;
                shetobj_modified = true;
            }
        }
        public double Surface_Temperature
        {
            get { return stemp; }
            set
            {
                stemp = value;
                stemp_modified = true;
            }
        }

        public string Material
        {
            get { return matprp; }
            set
            {
                matprp = value;
                matprp_modified = true;
            }
        }
        public double Heat_Generation
        {
            get { return heatvalue; }
            set
            {
                heatvalue = value;
                heatvalue_modified = true;
            }
        }
        public double Heat_Trans_Coef
        {
            get { return trans; }
            set
            {
                trans = value;
                trans_modified = true;
            }
        }

        public string Fluid_Type
        {
            get { return fldprp; }
            set
            {
                fldprp = value;
                fldprp_modified = true;
            }
        }
        public int Apply_Method
        {
            get { return fldapl; }
            set
            {
                fldapl = value;
                fldapl_modified = true;
            }
        }
        public double Humidity_Generation
        {
            get { return humidity; }
            set
            {
                humidity = value;
                humidity_modified = true;
            }
        }
        public double Contaminat_Generation
        {
            get { return pollution; }
            set
            {
                pollution = value;
                pollution_modified = true;
            }
        }
        public double Other1_Generation
        {
            get { return other1; }
            set
            {
                other1 = value;
                other1_modified = true;
            }
        }
        public double Other2_Generation
        {
            get { return other2; }
            set
            {
                other2 = value;
                other2_modified = true;
            }
        }
        public double Other3_Generation
        {
            get { return other3; }
            set
            {
                other3 = value;
                other3_modified = true;
            }
        }
        public int Air_Change
        {
            get { return ventarea; }
            set
            {
                ventarea = value;
                ventarea_modified = true;
            }
        }

        public double Initial_Temperature
        {
            get { return ventarea; }
            set
            {
                inittemp = value;
                inittemp_modified = true;
            }
        }

        public double Initial_Humidity
        {
            get { return inithumidity; }
            set
            {
                inithumidity = value;
                inithumidity_modified = true;
            }
        }

        public double Initial_Contaminat
        {
            get { return initpollution; }
            set
            {
                initpollution = value;
                initpollution_modified = true;
            }
        }

        public double Initial_Other1
        {
            get { return initother1; }
            set
            {
                initother1 = value;
                initother1_modified = true;
            }
        }

        public double Initial_Other2
        {
            get { return initother2; }
            set
            {
                initother2 = value;
                initother2_modified = true;
            }
        }

        public double Initial_Other3
        {
            get { return initother3; }
            set
            {
                initother3 = value;
                initother3_modified = true;
            }
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
                        if (FD_Commander.jp_lan) { p_str.Add("property set general attribute \"考慮しない（障害物）\""); }
                        else { p_str.Add("property set general attribute \"N/A(Obstacle)\""); }
                    }
                    else if (attribute == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general attribute \"流体\""); }
                        else { p_str.Add("property set general attribute \"Fluid\""); }
                    }
                    else if (attribute == 2)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general attribute \"固体（物性）\""); }
                        else { p_str.Add("property set general attribute \"Thermal object\""); }
                    }
                }

                if (shetobj_modified)
                {
                    if (shetobj == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general shetobj \"設定しない\""); }
                        else { p_str.Add("property set general shetobj \"NA\""); }
                    }
                    else if (shetobj == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set general shetobj \"表面温度\""); }
                        else { p_str.Add("property set general shetobj \"Surface temperature\""); }
                    }
                }
                if (stemp_modified) { p_str.Add("property set general stemp " + stemp.ToString()); }

                if (matprp_modified) { p_str.Add("property set general matprp " + matprp); }
                if (heatvalue_modified) { p_str.Add("property set general heatvalue " + heatvalue.ToString()); }
                if (trans_modified) { p_str.Add("property set general trans " + trans.ToString()); }

                if (fldprp_modified) { p_str.Add("property set general fldprp " + fldprp); }
                if (fldapl_modified)
                {
                    if (fldapl == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL FLDAPL \"くりぬき\""); }
                        else { p_str.Add("property set GENERAL FLDAPL \"Hollow\""); }
                    }
                    else if (fldapl == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL FLDAPL \"領域\""); }
                        else { p_str.Add("property set GENERAL FLDAPL \"Volume\""); }
                    }
                }
                if (humidity_modified) { p_str.Add("property set general humidity " + humidity.ToString()); }
                if (pollution_modified) { p_str.Add("property set general pollution " + pollution.ToString()); }
                if (other1_modified) { p_str.Add("property set general other1 " + other1.ToString()); }
                if (other2_modified) { p_str.Add("property set general other2 " + other2.ToString()); }
                if (other3_modified) { p_str.Add("property set general other3 " + other3.ToString()); }
                if (ventarea_modified) 
                {
                    if (ventarea == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set OPTION VENTAREA \"算出しない\""); }
                        else { p_str.Add("property set OPTION VENTAREA \"Not calculated\""); }
                    }
                    else if (ventarea == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set OPTION VENTAREA \"算出する\""); }
                        else { p_str.Add("property set OPTION VENTAREA \"Calculate\""); }
                    }
                }

                if (inittemp_modified) { p_str.Add("property set initial temp " + inittemp.ToString()); }
                if (inithumidity_modified) { p_str.Add("property set initial inithumidity " + inithumidity.ToString()); }
                if (initpollution_modified) { p_str.Add("property set initial initpollution " + initpollution.ToString()); }
                if (initother1_modified) { p_str.Add("property set initial initother1 " + initother1.ToString()); }
                if (initother2_modified) { p_str.Add("property set initial initother2 " + initother2.ToString()); }
                if (initother3_modified) { p_str.Add("property set initial initother3 " + initother3.ToString()); }

                return p_str;
            }
        }
    }
}