using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowDesigner
{
    public class CSV_DL : ComboBox
    {
        int analysistype;
        int group_type;
        int type;

        public string AT_String
        {
            get
            {
                if (analysistype == 0) { return "fwd"; }
                else if (analysistype == 1) { return "opt"; }
                else { return ""; }
            }
        }

        public string GT_String
        {
            get
            {
                if (group_type == 0) { return "atarget"; }
                else if (group_type == 1) { return "starget"; }
                else if (group_type == 2) { return "ctarget"; }
                else { return ""; }
            }
        }

        public string Type_String
        {
            get
            {
                if (type == 0) { return "outputtype"; }
                else if (type == 1) { return "paneldir"; }
                else if (type == 2) { return "outputseparation"; }
                else if (type == 3) { return "range"; }
                else { return ""; }
            }
        }

        public string[] Value_List
        {
            get
            {
                string[] temp = new string[0];

                if (type == 0)
                {
                    temp = new string[2];
                    temp[0] = "byelem";
                    temp[1] = "ave";
                }

                else if (type == 1)
                {
                    temp = new string[2];
                    temp[0] = "plus";
                    temp[1] = "minus";
                }

                else if (type == 2)
                {
                    temp = new string[3];
                    temp[0] = "none";
                    temp[1] = "target";
                    temp[2] = "object";
                }

                else if (type == 3)
                {
                    temp = new string[2];
                    temp[0] = "whole";
                    temp[1] = "center";
                }

                return temp;
            }
        }

        public string[] Item_List
        {
            get
            {
                string[] temp = new string[0];

                if (type == 0)
                {
                    temp = new string[2];
                    temp[0] = "Element unit";
                    temp[1] = "Average";
                }

                else if (type == 1)
                {
                    temp = new string[2];
                    temp[0] = "(+) direction";
                    temp[1] = "(-) direction";
                }

                else if (type == 2)
                {
                    temp = new string[3];
                    temp[0] = "Single file";
                    temp[1] = "Target each";
                    temp[2] = "Object each";
                }

                else if (type == 3)
                {
                    temp = new string[2];
                    temp[0] = "All elements";
                    temp[1] = "Only elements including center";
                }

                return temp;
            }
        }

        public string Name
        {
            get
            {
                if (type == 0) { return "Output type"; }
                else if (type == 1) { return "Panel direction"; }
                else if (type == 2) { return "Output unit"; }
                else if (type == 3) { return "Output range"; }
                else { return ""; }
            }
        }

        string Value
        {
            get
            {
                return Value_List[SelectedIndex];
            }
        }

        //plugin ctrlgraplug csv set { fwd | opt } { atarget | starget | ctarget } { target | range | ... } { byelem | ave }
        public string Set_string
        {
            get
            {
                return "plugin ctrlgraplug csv set " + AT_String + " " + GT_String + " " + Type_String + " " + Value;
            }
        }

        public CSV_DL(int _analysistype, int _grouptype, int _type)
            : base()
        {
            analysistype = _analysistype;
            group_type = _grouptype;
            type = _type;

            Items.Clear();
            foreach (string item in Item_List)
            {
                Items.Add(item);
            }

            SelectedIndex = 0;
        }

        public void Update()
        {
            FD_Commander.Excute(Set_string);
        }
    }
}