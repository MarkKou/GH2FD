using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class CSV_Group : FD_Setting
    {
        int analysistype;
        int type;
        int index;
        string name;
        public List<CSV_CB> Items = new List<CSV_CB>();

        public string Name
        {
            get { return name; }
        }

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
                if (type == 0) { return "target"; }
                else if (type == 1) { return "outputcycle"; }
                else { return ""; }
            }
        }

        public string Index
        {
            get
            {
                if (type == 0) { return index.ToString(); }
                else { return ""; }
            }
        }

        public CSV_Group(int _Analysis_Type, int _Index, int _Type)
            : base()
        {
            analysistype = _Analysis_Type;
            index = _Index;
            type = _Type;

            name = (FD_Commander.Excute("plugin ctrlgraplug csv get " + AT_String + " " + GT_String + "groupname " + index.ToString())).ToString();
            int itemcount = Convert.ToInt32(FD_Commander.Excute("plugin ctrlgraplug csv get " + AT_String + " " + GT_String + "count " + Index));

            for (int i = 0; i < itemcount; i++)
            {
                object itemname = FD_Commander.Excute("plugin ctrlgraplug csv get " + AT_String + " " + GT_String + "name " + Index + " " + i.ToString());
                Items.Add(new CSV_CB(analysistype, type, index, i, itemname.ToString()));
            }
        }

        protected override List<string> Update_Strings
        {
            get
            {
                List<string> upstr = new List<string>();
                foreach (CSV_CB item in Items)
                {
                    upstr.Add(item.Set_string);
                }
                return upstr;
            }
        }

        public List<string> Get_Up_Strings()
        {
            return Update_Strings;
        }
    }
}