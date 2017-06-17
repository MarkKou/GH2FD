using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Set_CSV_Cycle : GH_Component
    {
        public Set_CSV_Cycle()
            : base("CSV Cycle", "CSV C",
                "Set CSV Output Cycles",
                "FlowDesigner", Tools.sub_cate_04)
        { }

        CSV_Cycle csvc;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            csvc = new CSV_Cycle(0);
            if (csvc.Items.Count == 0) { csvc = new CSV_Cycle(1); }

            foreach (CSV_CB item in csvc.Items)
            {
                pManager.AddBooleanParameter(item.Name, item.Name, "Output "+item.Name+" or not", GH_ParamAccess.item, false);
            }

            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "M", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Cycle Settings", "C", "Cycle Settings", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int counter = 0;

            foreach (CSV_CB item in csvc.Items)
            {
                bool temp = false;
                DA.GetData(counter, ref temp);
                item.Checked = temp;
                counter++;
            }

            DA.SetData(1, csvc);

            bool go = false;
            DA.GetData(counter, ref go);

            if (go)
            {
                DA.SetDataList(0, csvc.Update());
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.CSV_Cycle;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{0fee66e5-b310-41d1-8c1f-c0049c94f0c9}"); }
        }
    }
}