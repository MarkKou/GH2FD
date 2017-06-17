using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Vol_Source : FD_Group
    {
        protected override string Object_Type { get { return "heatarea"; } }

        public FD_Vol_Source(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified
        private bool gt_modified = false;
        private bool he_modified = false;
        private bool hu_modified = false;
        private bool ct_modified = false;
        private bool o1_modified = false;
        private bool o2_modified = false;
        private bool o3_modified = false;

        //Valiable
        private int gene_type;
        private double heat_gene;
        private double humi_gene;
        private double conta_gene;
        private double other1; 
        private double other2;
        private double other3;

        //Properties
        public int Gene_Type
        {
            get { return gene_type; }
            set
            {
                gene_type = value;
                gt_modified = true;
            }
        }
        public double Heat_Gene
        {
            get { return heat_gene; }
            set
            {
                heat_gene = value;
                he_modified = true;
            }
        }
        public double Humi_Gene
        {
            get { return humi_gene; }
            set
            {
                humi_gene = value;
                hu_modified = true;
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

        //Modified
        private bool it_modified = false;
        private bool ih_modified = false;
        private bool ic_modified = false;
        private bool io1_modified = false;
        private bool io2_modified = false;
        private bool io3_modified = false;

        //Valiable
        private double init_temp;
        private double init_humi;
        private double init_conta;
        private double init_other1;
        private double init_other2;
        private double init_other3;

        //Properties
        public double Init_Temp
        {
            get { return init_temp; }
            set
            {
                init_temp = value;
                it_modified = true;
            }
        }
        public double Init_Humi
        {
            get { return init_humi; }
            set
            {
                init_humi = value;
                ih_modified = true;
            }
        }
        public double Init_Conta
        {
            get { return init_conta; }
            set
            {
                init_conta = value;
                ic_modified = true;
            }
        }
        public double Init_Other1
        {
            get { return init_other1; }
            set
            {
                init_other1 = value;
                io1_modified = true;
            }
        }
        public double Init_Other2
        {
            get { return init_other2; }
            set
            {
                init_other2 = value;
                io2_modified = true;
            }
        }
        public double Init_Other3
        {
            get { return init_other3; }
            set
            {
                init_other3 = value;
                io3_modified = true;
            }
        }

        protected override List<string> Property_string
        {
            get
            {
                List<string> p_str = new List<string>();

                //Diffusion material
                if (gt_modified)
                {
                    if (gene_type == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENCOND GENTYPE \"全体\""); }
                        else { p_str.Add("property set GENCOND GENTYPE \"Total\""); }
                    }
                    else if (gene_type == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENCOND GENTYPE \"単位体積あたり\""); }
                        else { p_str.Add("property set GENCOND GENTYPE \"Per unit volume\""); }
                    }
                }

                if (he_modified) { p_str.Add("property set GENCOND CALORIE " + heat_gene.ToString()); }
                if (hu_modified) { p_str.Add("property set GENCOND HUMIDITY " + humi_gene.ToString()); }
                if (ct_modified) { p_str.Add("property set GENCOND POLLUTION " + conta_gene.ToString()); }
                if (o1_modified) { p_str.Add("property set GENCOND OTHER1 " + other1.ToString()); }
                if (o2_modified) { p_str.Add("property set GENCOND OTHER2 " + other2.ToString()); }
                if (o3_modified) { p_str.Add("property set GENCOND OTHER3 " + other3.ToString()); }

                if (it_modified) { p_str.Add("property set INITIAL INITTEMP " + init_temp.ToString()); }
                if (ih_modified) { p_str.Add("property set INITIAL INITHUMIDITY " + init_humi.ToString()); }
                if (ic_modified) { p_str.Add("property set INITIAL INITPOLLUTION " + init_conta.ToString()); }
                if (io1_modified) { p_str.Add("property set INITIAL INITOTHER1 " + init_other1.ToString()); }
                if (io2_modified) { p_str.Add("property set INITIAL INITOTHER2 " + init_other2.ToString()); }
                if (io3_modified) { p_str.Add("property set INITIAL INITOTHER3 " + init_other3.ToString()); }


                return p_str;
            }
        }
    }
}