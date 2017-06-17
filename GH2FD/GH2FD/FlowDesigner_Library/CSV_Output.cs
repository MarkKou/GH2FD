using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class CSV_Output
    {
        public CSV_Target Targets;
        public CSV_Cycle Cycles;
        public CSV_Config Analysis_Target;
        public CSV_Config Surface_Target;
        public CSV_Config Comfort_Target;
        public List<string> IDs;

        public CSV_Output(int _analysistype)
        {
            Targets = new CSV_Target(_analysistype);
            Cycles = new CSV_Cycle(_analysistype);
            Analysis_Target = new CSV_Config(_analysistype, 0);
            //Surface_Target = new CSV_Config(_analysistype, 1);
            //Comfort_Target = new CSV_Config(_analysistype, 2);
        }

        public CSV_Output() { }

        public CSV_Output(CSV_Target _Targets, CSV_Cycle _Cycles, CSV_Config _Analysis_Target)
        {
            Targets = _Targets;
            Cycles = _Cycles;
            Analysis_Target = _Analysis_Target;
        }

        public void Update()
        {
            try { Targets.Update(); }
            catch { }

            try { Cycles.Update(); }
            catch { }

            try { Analysis_Target.Update(); }
            catch { }

            //try { Surface_Target.Update(); }
            //catch { }

            //try { Comfort_Target.Update(); }
            //catch { }
        }

        public string Output()
        {
            FD_Commander.Unselect();
            foreach (string id in IDs) { FD_Commander.Select(id); }
            return FD_Commander.Excute("plugin ctrlgraplug csv fwd output").ToString();
        }
    }
}