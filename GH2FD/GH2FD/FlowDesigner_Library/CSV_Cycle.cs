using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class CSV_Cycle : CSV_Group
    {
        public CSV_Cycle(int _analysistype)
            : base(_analysistype, 0, 1) { }
    }
}