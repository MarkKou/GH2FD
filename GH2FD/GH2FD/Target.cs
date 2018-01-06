using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Target : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Target()
            : base("Target", "Target",
                "Convert a Box into a Target",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Geometries", "G", "Geometries to be converted,\r\nYou can input\r\n  Box\r\n  FD_Extrusion\r\n  Mesh (will be triangulated)\r\n  Brep (will be triangulated)", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Type", "Ty", "Type\r\n    0: Target\r\n    1: Equalize\r\n    2: Ignore", GH_ParamAccess.item, 0);
            Param_Integer p1 = pManager[1] as Param_Integer;
            p1.AddNamedValue("Target", 0);
            p1.AddNamedValue("Equalize", 1);
            p1.AddNamedValue("Ignore", 2);
            //2
            pManager.AddTextParameter("Flow Speed", "FS", "Flow Speed [m/s]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Temperature", "Te", "Temperature [C]", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("Density", "De", "Density of Other1 [ppm]", GH_ParamAccess.item, "Default");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter(Tools.c_o_n, Tools.c_o_s, Tools.c_o_d, GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<dynamic> items = new List<dynamic>();
            DA.GetDataList(0, items);

            FD_Target object_group = new FD_Target(Tools.GenerateCubeList(items));

            int type = 0;
            string speed = "";
            string temperature = "";
            string density = "";

            DA.GetData(1, ref type);
            DA.GetData(2, ref speed);
            DA.GetData(3, ref temperature);
            DA.GetData(4, ref density);

            object_group.Type = type;
            if (speed != "Default") { object_group.Speed = Convert.ToDouble(speed); }
            if (temperature != "Default") { object_group.Temperature = Convert.ToDouble(temperature); }
            if (density != "Default") { object_group.Density = Convert.ToDouble(density); }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.target;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{3f2258ec-4bc7-4232-8fdf-3753c141c178}"); }
        }
    }
}