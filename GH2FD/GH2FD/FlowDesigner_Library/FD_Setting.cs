using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Setting
    {
        public FD_Setting() { }
        protected virtual List<string> Update_Strings
        {
            get { return new List<string>(); }
        }

        public List<string> Update()
        {
            List<string> results = new List<string>();

            if (Update_Strings.Count != 0)
            {
                foreach (string upstr in Update_Strings)
                {
                    results.Add(FD_Commander.Excute(upstr).ToString());
                }
            }

            return results;
        }
    }
}