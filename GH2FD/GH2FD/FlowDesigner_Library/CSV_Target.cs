using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class CSV_Target : FD_Setting
    {
        int analysistype;
        public List<CSV_Group> groups;

        public string Analysis_Type
        {
            get
            {
                if (analysistype == 0) { return "fwd"; }
                else if (analysistype == 1) { return "opt"; }
                else { return ""; }
            }
        }

        public CSV_Target(int _analysistype)
            : base()
        {
            analysistype = _analysistype;

            object targetgroupcount = FD_Commander.Excute("plugin ctrlgraplug csv get " + Analysis_Type + " targetgroupcount");

            groups = new List<CSV_Group>();

            for (int i = 0; i < Convert.ToInt32(targetgroupcount); i++)
            {
                groups.Add(new CSV_Group(analysistype, i, 0));
            }
        }

        protected override List<string> Update_Strings
        {
            get
            {
                List<string> upstr = new List<string>();
                foreach (CSV_Group group in groups)
                {
                    upstr.AddRange(group.Get_Up_Strings());
                }
                return upstr;
            }
        }
    }
}