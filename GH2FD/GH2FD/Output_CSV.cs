using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Output_CSV : GH_Component
    {
        public Output_CSV()
            : base("Output CSV", "→CSV",
                "Output the results into a CSV file",
                "FlowDesigner", Tools.sub_cate_04)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Settings", "Set", "Settings of 'Target', 'Cycle' & 'Output Configuration'", GH_ParamAccess.list);
            pManager.AddTextParameter("IDs of the output objets", "IDs", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Excute Output", "Go", "", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "M", "Messages from FD", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool go = false;
            DA.GetData(2, ref go);

            if (go)
            {
                List<FD_Setting> settings = new List<FD_Setting>();
                DA.GetDataList(0, settings);

                List<string> IDs = new List<string>();
                DA.GetDataList(1, IDs);

                List<string> messages = new List<string>();

                foreach(FD_Setting set in settings)
                {
                    messages.AddRange(set.Update());
                }

                CSV_Output csv_out = new CSV_Output();

                csv_out.IDs = IDs;
                messages.Add(csv_out.Output());

                DA.SetDataList(0, messages);
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Output_CSV;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{287f9ab7-45a5-4672-baac-15d9b9c9a95b}"); }
        }
    }
}