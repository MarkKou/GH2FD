using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Set_CSV_Targets : GH_Component
    {
        public Set_CSV_Targets()
            : base("CSV Target", "CSV T",
                "Set CSV Output Targets",
                "FlowDesigner", Tools.sub_cate_04)
        { }

        CSV_Target csvt;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            csvt = new CSV_Target(0);
            if (csvt.groups.Count == 0) { csvt = new CSV_Target(1); }

            foreach (CSV_Group group in csvt.groups)
            {
                foreach (CSV_CB item in group.Items)
                {
                    pManager.AddBooleanParameter(item.Name, item.Name, "Output " + item.Name + " or not", GH_ParamAccess.item, false);
                }
            }

            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "M", "Message from FD", GH_ParamAccess.list);
            pManager.AddGenericParameter("Target Settings", "T", "Target Settings", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<bool> values = new List<bool>();

            int counter = 0;

            foreach (CSV_Group group in csvt.groups)
            {
                foreach (CSV_CB item in group.Items)
                {
                    bool temp = false;
                    DA.GetData(counter, ref temp);
                    item.Checked = temp;
                    counter++;
                }
            }

            DA.SetData(1, csvt);

            bool go = false;
            DA.GetData(counter, ref go);

            if (go) { DA.SetDataList(0, csvt.Update()); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.CSV_Target;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{23f9c7b0-cff1-4e6a-bbe4-0cacfc195d8c}"); }
        }
    }
}