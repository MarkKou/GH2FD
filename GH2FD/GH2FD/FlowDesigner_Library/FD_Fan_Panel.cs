using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Fan_Panel : FD_Group
    {
        protected override string Object_Type { get { return "fanpanel"; } }

        public FD_Fan_Panel(List<FD_Object> _Members)
            : base(_Members) { }

        //Modified?
        private bool me_modified = false;
        private bool sp_modified = false;
        private bool vo_modified = false;
        private bool di_modified = false;

        //Valiable
        private int method;
        private double speed;
        private double volume;
        private int direction;

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
        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                di_modified = true;
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
                    else if (method == 2)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL INPUTTYPE \"P-Qカーブ\""); }
                        else { p_str.Add("property set GENERAL INPUTTYPE \"P-Q curve\""); }
                    }
                }

                if (sp_modified) { p_str.Add("property set GENERAL SPEED " + speed.ToString()); }
                if (vo_modified) { p_str.Add("property set GENERAL AMOUNT " + volume.ToString()); }

                if (di_modified)
                {
                    if (direction == 0)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL BALANCEPRIORITY \"流速一定\""); }
                        else { p_str.Add("property set GENERAL DIRECTION \"Plus direction\""); }
                    }
                    else if (direction == 1)
                    {
                        if (FD_Commander.jp_lan) { p_str.Add("property set GENERAL BALANCEPRIORITY \"流量一定\""); }
                        else { p_str.Add("property set GENERAL DIRECTION \"Minus direction\""); }
                    }
                }
                return p_str;
            }
        }
    }
}