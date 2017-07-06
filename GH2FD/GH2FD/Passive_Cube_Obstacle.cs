using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Passive_Cube_Obstacle : GH_Component
    {
        public Passive_Cube_Obstacle()
            : base("Passive_Cube (Obstacle)", "PCube O",
                "Convert a Box into a Passive cube (Obstacle)",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Boxes", "B", "Boxes to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Surface heat generated", "SH", "Surface heat generated\r\n    0: NA\r\n    1: Surface temperature", GH_ParamAccess.item, 0);
            Param_Integer p1 = pManager[1] as Param_Integer;
            p1.AddNamedValue("NA", 0);
            p1.AddNamedValue("Surface temperature", 1);
            //2
            pManager.AddTextParameter("Surface temperature", "ST", "Surface temperature [C]", GH_ParamAccess.item, "Default");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Cubes", "C", "Cubes converted from boxes", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<dynamic> items = new List<dynamic>();
            DA.GetDataList(0, items);

            FD_Passive_Cube object_group = new FD_Passive_Cube(Tools.GenerateCubeList(items));

            int shg = 0;
            string st = "";

            DA.GetData(1, ref shg);
            DA.GetData(2, ref st);

            object_group.Surface_Heat_Generation = shg;
            if (st != "Default") { object_group.Surface_Temperature = Convert.ToDouble(st); }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Passive_object_Obstacle;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{ade2868c-0edc-4518-8d00-daf62da00bf7}"); }
        }
    }
}