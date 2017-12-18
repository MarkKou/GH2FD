using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Defined_Panel : GH_Component
    {
        public Defined_Panel()
            : base("Defined Panel", "DPanel",
                "Convert a Mesh into Defined Panel",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter(Tools.c_o_n, Tools.c_o_s, Tools.c_o_d, GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(0, mesh_list);

            FD_Defined_Panel object_group = new FD_Defined_Panel(Tools.GeneratePanelList(mesh_list));

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Defined_Panel;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{2a67bed9-d796-43a1-9aa2-3d80ea6eadb9}"); }
        }
    }
}