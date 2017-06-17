using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Command_Console : GH_Component
    {
        public Command_Console()
            : base("Command Console", "COM",
                "Send the commands into FlowDesigner",
                "FlowDesigner", Tools.sub_cate_05)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Command", "C", "Command", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Result", "Result", "", GH_ParamAccess.item);
            pManager.AddTextParameter("fd", "fd", "", GH_ParamAccess.item);
        }

        string result = "";

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string command = "";
            bool run = false;

            DA.GetData(0, ref command);
            DA.GetData(1, ref run);

            if(run)
            {
                result = Convert.ToString(FD_Commander.Excute(command)); 
            }

            DA.SetData(0, result);
            DA.SetData(1, FD_Commander.Message);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.command;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{363dbec1-d653-483a-abe6-518777a15a71}"); }
        }
    }
}