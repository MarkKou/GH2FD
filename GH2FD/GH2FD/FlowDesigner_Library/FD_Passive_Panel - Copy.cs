using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Passive_Panel__ : FD_Group
    {
        protected override string Object_Type { get { return "heatobj"; } }

        public FD_Passive_Panel__(List<FD_Object> _Members)
            : base(_Members) { }
    }
}