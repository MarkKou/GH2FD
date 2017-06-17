using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Fan_Panel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Fan_Panel()
            : base("Fan panel", "FPanel",
                "Convert a Mesh into Passive Fan panels",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Method", "Me", "Method\r\n    0: Flow speed\r\n    1: Flow volume\r\n    2: P-Q curve", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Flow speed", 0);
            p1.AddNamedValue("Flow volume", 1);
            p1.AddNamedValue("P-Q curve", 2);
            //2
            pManager.AddTextParameter("Flow Speed", "FS", "Flow speed [m/s]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Flow Volume", "FV", "Flow volume [m3/min]", GH_ParamAccess.item, "Default");
            //4
            pManager.AddIntegerParameter("Direction", "Di", "Direction\r\n    0: Plus\r\n    1: Minus", GH_ParamAccess.item, 0);
            Param_Integer p4 = (Param_Integer)pManager[4];
            p4.AddNamedValue("Plus", 0);
            p4.AddNamedValue("Minus", 1);
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

            FD_Fan_Panel object_group = new FD_Fan_Panel(Tools.GeneratePanelList(mesh_list));

            int method = 0;
            string speed = "";
            string volume = "";
            int direction = 0;

            DA.GetData(1, ref method);
            DA.GetData(2, ref speed);
            DA.GetData(3, ref volume);
            DA.GetData(4, ref direction);

            object_group.Method = method;

            if (speed != "Default")
            {
                object_group.Speed = Convert.ToDouble(speed);
            }

            if (volume != "Default")
            {
                object_group.Volume = Convert.ToDouble(volume);
            }

            object_group.Direction = direction;

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Fan_panel;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{cb4f995d-1251-48f3-8a31-81c11e8cd5e5}"); }
        }
    }
}