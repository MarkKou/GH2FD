using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Outlet : GH_Component
    {
        public Outlet()
            : base("Outlet", "Outlet",
                "Convert a Mesh into Passive Outlets",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Method", "Me", "Method\r\n    0: Flow speed\r\n    1: Flow volume", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Flow speed", 0);
            p1.AddNamedValue("Flow volume", 1);
            //2
            pManager.AddIntegerParameter("Balance Priority", "BP", "Balance Priority\r\n    0: Keep flow speed\r\n    1: Keep flow volume", GH_ParamAccess.item, 0);
            Param_Integer p2 = (Param_Integer)pManager[2];
            p2.AddNamedValue("Keep flow speed", 0);
            p2.AddNamedValue("Keep flow volume", 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Panels", "P", "Panels converted from mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(0, mesh_list);

            FD_Inlet object_group = new FD_Inlet(Tools.GeneratePanelList(mesh_list));

            int method = 0;
            int balance_prio = 0;

            DA.GetData(1, ref method);
            DA.GetData(2, ref balance_prio);

            object_group.Method = method;

            object_group.Balance_Prio = balance_prio;

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.outlet;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{51a932fa-9810-419d-abe3-9683aa171f56}"); }
        }
    }
}