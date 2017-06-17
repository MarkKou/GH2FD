using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Panel_Source : FD_Group
    {
        protected override string Object_Type { get { return "heatpanel"; } }

        public FD_Panel_Source(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private bool tt_modified = false;
        private bool st_modified = false;
        private bool h_modified = false;
        private bool t_modified = false;
        private bool ht_modified = false;
        private bool oh_modified = false;
        private bool sh_modified = false;

        //Valiable
        private int temptype;
        private double stemp;
        private double heat;
        private double temp;
        private double htrans;
        private double ohtrans;
        private double shtrans;

        //Properties
        public int Temp_Type
        {
            get { return temptype; }
            set
            {
                temptype = value;
                tt_modified = true;
            }
        }
        public double Surface_Temp
        {
            get { return stemp; }
            set
            {
                stemp = value;
                st_modified = true;
            }
        }
        public double Heat_Generation
        {
            get { return heat; }
            set
            {
                heat = value;
                h_modified = true;
            }
        }
        public double External_Temp
        {
            get { return temp; }
            set
            {
                temp = value;
                t_modified = true;
            }
        }
        public double Heat_Trans
        {
            get { return htrans; }
            set
            {
                htrans = value;
                ht_modified = true;
            }
        }
        public double External_HTrans
        {
            get { return ohtrans; }
            set
            {
                ohtrans = value;
                oh_modified = true;
            }
        }
        public double Internal_HTrans
        {
            get { return shtrans; }
            set
            {
                shtrans = value;
                sh_modified = true;
            }
        }

        //Modified?
        private bool ps_modified = false;
        private bool mt_modified = false;
        private bool th_modified = false;
        private bool hv_modified = false;
        private bool it_modified = false;

        //Valiable
        private int pseudosolid;
        private string material;
        private double thickness;
        private double heat_volumn;
        private double ini_temp;

        //Properties
        public int Pseudo_Solid
        {
            get { return pseudosolid; }
            set
            {
                pseudosolid = value;
                ps_modified = true;
            }
        }
        public string Material
        {
            get { return material; }
            set
            {
                material = value;
                mt_modified = true;
            }
        }
        public double Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                th_modified = true;
            }
        }
        public double HG_Volumn
        {
            get { return heat_volumn; }
            set
            {
                heat_volumn = value;
                hv_modified = true;
            }
        }
        public double Init_Temp
        {
            get { return ini_temp; }
            set
            {
                ini_temp = value;
                it_modified = true;
            }
        }

        //Modified
        private bool dt_modified = false;
        private bool hg_modified = false;
        private bool ct_modified = false;
        private bool o1_modified = false;
        private bool o2_modified = false;
        private bool o3_modified = false;

        //Valiable
        private int diff_type;
        private double humi_gene;
        private double conta_gene;
        private double other1;
        private double other2;
        private double other3;

        //Properties
        public int Diff_Type
        {
            get { return diff_type; }
            set
            {
                diff_type = value;
                dt_modified = true;
            }
        }
        public double Humi_Gene
        {
            get { return humi_gene; }
            set
            {
                humi_gene = value;
                hg_modified = true;
            }
        }
        public double Conta_Genen
        {
            get { return conta_gene; }
            set
            {
                conta_gene = value;
                ct_modified = true;
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

                //Temperature conditions
                if (tt_modified)
                {
                    if (temptype == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set TEMPCOND TEMPTYPE \"表面温度固定\""); }
                        else { p_str.Add("property set TEMPCOND TEMPTYPE \"Fixed surface temperature\""); }
                    }
                    else if (temptype == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set TEMPCOND TEMPTYPE \"表面総発熱量\""); }
                        else { p_str.Add("property set TEMPCOND TEMPTYPE \"Total surface heat generated\""); }
                    }
                    else if (temptype == 2)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set TEMPCOND TEMPTYPE \"単位面積あたり発熱量\""); }
                        else { p_str.Add("property set TEMPCOND TEMPTYPE \"Heat generated per unit area\""); }
                    }
                    else if (temptype == 3)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set TEMPCOND TEMPTYPE \"外気温と固体表面までの熱伝達率\""); }
                        else { p_str.Add("property set TEMPCOND TEMPTYPE \"Heat transfer coef between external air and solid surface\""); }
                    }
                    else if (temptype == 4)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set TEMPCOND TEMPTYPE \"外気温と内側表面までの熱通過率\""); }
                        else { p_str.Add("property set TEMPCOND TEMPTYPE \"External air temperature and overall heat transfer coef to internal surface\""); }
                    }
                    else if (temptype == 5)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set TEMPCOND TEMPTYPE \"外気温と熱通過率\""); }
                        else { p_str.Add("property set TEMPCOND TEMPTYPE \"External air temperature and overall heat transfer coef\""); }
                    }
                }

                if (st_modified) { p_str.Add("property set TEMPCOND STEMP " + stemp.ToString()); }
                if (h_modified) { p_str.Add("property set TEMPCOND HEAT " + heat.ToString()); }
                if (t_modified) { p_str.Add("property set TEMPCOND TEMP " + temp.ToString()); }
                if (ht_modified) { p_str.Add("property set TEMPCOND HTRANS " + htrans.ToString()); }
                if (oh_modified) { p_str.Add("property set TEMPCOND OHTRANS " + ohtrans.ToString()); }
                if (sh_modified) { p_str.Add("property set TEMPCOND SHTRANS " + shtrans.ToString()); }

                //Pseudo solid condition
                if (ps_modified)
                {
                    if (pseudosolid == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set PSEUDOSOLIDCATEGORY PSEUDOSOLID \"いいえ\""); }
                        else { p_str.Add("property set PSEUDOSOLIDCATEGORY PSEUDOSOLID \"No\""); }
                    }
                    else if (pseudosolid == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set PSEUDOSOLIDCATEGORY PSEUDOSOLID \"はい\""); }
                        else { p_str.Add("property set PSEUDOSOLIDCATEGORY PSEUDOSOLID \"Yes\""); }
                    }
                }

                if (mt_modified) { p_str.Add("property set PSEUDOSOLIDCATEGORY MATPRP  \"" + material + "\""); }
                if (th_modified) { p_str.Add("property set PSEUDOSOLIDCATEGORY THICKNESS" + thickness.ToString()); }
                if (hv_modified) { p_str.Add("property set PSEUDOSOLIDCATEGORY HEATBYVOLUME" + heat_volumn.ToString()); }
                if (it_modified) { p_str.Add("property set PSEUDOSOLIDCATEGORY INITTEMP" + ini_temp.ToString()); }

                //Diffusion material
                if (dt_modified)
                {
                    if (diff_type == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set POLLUCOND DIFFTYPE \"表面総発熱量\""); }
                        else { p_str.Add("property set POLLUCOND DIFFTYPE \"Total surface mass generated\""); }
                    }
                    else if (diff_type == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set POLLUCOND DIFFTYPE \"表面量固定\""); }
                        else { p_str.Add("property set POLLUCOND DIFFTYPE \"Fixed surface value\""); }
                    }
                }

                if (hg_modified) { p_str.Add("property set POLLUCOND HUMIDITY " + humi_gene.ToString()); }

                if (ct_modified)
                {
                    p_str.Add("property set POLLUCOND POLLUTION " + conta_gene.ToString());
                    p_str.Add("property set POLLUCOND FIXEDPOLLUTION " + conta_gene.ToString());
                }

                if (o1_modified) 
                {
                    p_str.Add("property set POLLUCOND OTHER1 " + other1.ToString());
                    p_str.Add("property set POLLUCOND FIXEDOTHER1 " + other1.ToString());
                }

                if (o2_modified)
                {
                    p_str.Add("property set POLLUCOND OTHER2 " + other2.ToString());
                    p_str.Add("property set POLLUCOND FIXEDOTHER2 " + other2.ToString());
                }

                if (o3_modified)
                {
                    p_str.Add("property set POLLUCOND OTHER3 " + other3.ToString());
                    p_str.Add("property set POLLUCOND FIXEDOTHER3 " + other3.ToString());
                }

                return p_str;
            }
        }
    }
}