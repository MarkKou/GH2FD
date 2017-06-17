using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Empty_Set_Object : GH_Component
    {
        public Empty_Set_Object()
            : base("Empty Setting & Object", "Ghost",
                "Output an empty setting & an empty object",
                "FlowDesigner", Tools.sub_cate_05)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("S", "S", "An empty setting, connect it to the 'Set' input of 2FlowDesigner if necessary.", GH_ParamAccess.item);
            pManager.AddGenericParameter("O", "O", "An empty object, connect it to the 'Obj' input of 2FlowDesigner if necessary.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, new FD_Setting());
            DA.SetData(1, new FD_Group());
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Empty_SO;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{c12998f6-c77d-48ed-83db-c43961007cf4}"); }
        }
    }
}