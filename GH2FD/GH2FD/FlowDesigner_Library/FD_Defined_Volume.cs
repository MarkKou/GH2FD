using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Defined_Volume : FD_Group
    {
        protected override string Object_Type { get { return "anatarget"; } }

        public FD_Defined_Volume(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified
        private bool ty_modified = false;
        private bool xp_modified = false;
        private bool xm_modified = false;
        private bool yp_modified = false;
        private bool ym_modified = false;
        private bool zp_modified = false;
        private bool zm_modified = false;

        //Valiable
        private int type = 0;
        private int xplus = 1;
        private int xminus = 1;
        private int yplus = 1;
        private int yminus = 1;
        private int zplus = 1;
        private int zminus = 0;

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

        public bool X_Plus
        {
            set
            {
                xp_modified = true;
                if (value) { xplus = 1; }
                else { xplus = 0; }
            }
            get { return (xplus == 1); }
        }

        public bool X_Minus
        {
            set
            {
                xm_modified = true;
                if (value) { xminus = 1; }
                else { xminus = 0; }
            }
            get { return (xminus == 1); }
        }

        public bool Y_Plus
        {
            set
            {
                yp_modified = true;
                if (value) { yplus = 1; }
                else { yplus = 0; }
            }
            get { return (yplus == 1); }
        }

        public bool Y_Minus
        {
            set
            {
                ym_modified = true;
                if (value) { yminus = 1; }
                else { yminus = 0; }
            }
            get { return (yminus == 1); }
        }

        public bool Z_Plus
        {
            set
            {
                zp_modified = true;
                if (value) { zplus = 1; }
                else { zplus = 0; }
            }
            get { return (zplus == 1); }
        }

        public bool Z_Minus
        {
            set
            {
                zm_modified = true;
                if (value) { zminus = 1; }
                else { zminus = 0; }
            }
            get { return (zminus == 1); }
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
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL ATYPE \"単純領域\""); }
                        else { p_str.Add("property set GENERAL ATYPE \"Simple volume\""); }
                    }
                    else if (type == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL ATYPE \"ネスティング領域\""); }
                        else { p_str.Add("property set GENERAL ATYPE \"Nesting volume\""); }
                    }
                }

                if (type == 1 && (xp_modified || xm_modified || yp_modified || ym_modified || zp_modified || zm_modified))
                {
                    p_str.Add("property set GENERAL APPLYSURFACE \"" + xminus.ToString() + " " + xplus.ToString() + " " + yminus.ToString() + " " + yplus.ToString() + " " + zminus.ToString() + " " + zplus.ToString() + "\"");
                }

                return p_str;
            }
        }
    }
}