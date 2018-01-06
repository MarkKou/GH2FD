using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Passive_Cube_Fluid : GH_Component
    {
        public Passive_Cube_Fluid()
            : base("Passive_Cube (Fluid)", "PCube F",
                "Convert a Box into a Passive cube (Fluid)",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Geometries", "G", "Geometries to be converted,\r\nYou can input\r\n  Box\r\n  FD_Extrusion\r\n  Mesh (will be triangulated)\r\n  Brep (will be triangulated)", GH_ParamAccess.list);
            //1
            pManager.AddTextParameter("Type", "Ty", "The name of the fluid. Please check the fluid list in FD", GH_ParamAccess.item, "Default");
            //2
            pManager.AddIntegerParameter("Apply Method", "AM", "Apply Method\r\n    0: Hollow\r\n    1: Volume", GH_ParamAccess.item, 0);
            Param_Integer p2 = pManager[2] as Param_Integer;
            p2.AddNamedValue("Hollow", 0);
            p2.AddNamedValue("Volume", 1);
            //3
            pManager.AddTextParameter("Heat Generation", "HG", "Heat Generation [W]", GH_ParamAccess.item, "Default");
            //4
            pManager.AddIntegerParameter("Air Change", "AC", "Air Change\r\n    0: Not calculated\r\n    1: Calculate", GH_ParamAccess.item, 0);
            Param_Integer p4 = pManager[4] as Param_Integer;
            p4.AddNamedValue("Not calculated", 0);
            p4.AddNamedValue("Calculate", 1);
            //5
            pManager.AddTextParameter("Generation Properties", "GP", "Use the AP component to set humidity, etc.", GH_ParamAccess.item, "Default");
            //6
            pManager.AddTextParameter("Initial Properties", "IP", "Use the AP component to set temperature, humidity, etc.", GH_ParamAccess.item, "Default");
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

            FD_Passive_Cube object_group = new FD_Passive_Cube(Tools.GenerateCubeList(items));

            string ty = "";
            int am = 0;
            string hg = "";
            int ac = 0;
            string gp = "";
            string ip = "";

            DA.GetData(1, ref ty);
            DA.GetData(2, ref am);
            DA.GetData(3, ref hg);
            DA.GetData(4, ref ac);
            DA.GetData(5, ref gp);
            DA.GetData(6, ref ip);

            object_group.Attribute = 1;
            if (ty != "Default") { object_group.Fluid_Type = ty; }
            object_group.Apply_Method = am; 
            if (hg != "Default") { object_group.Heat_Generation = Convert.ToDouble(hg); }
            object_group.Air_Change = ac;

            if (gp != "Default")
            {
                List<string> gps = Tools.MultiLine2List(gp);

                if (gps[1] != "Default") { object_group.Humidity_Generation = Convert.ToDouble(gps[1]); }
                if (gps[2] != "Default") { object_group.Contaminat_Generation = Convert.ToDouble(gps[2]); }
                if (gps[3] != "Default") { object_group.Other1_Generation = Convert.ToDouble(gps[3]); }
                if (gps[4] != "Default") { object_group.Other2_Generation = Convert.ToDouble(gps[4]); }
                if (gps[5] != "Default") { object_group.Other3_Generation = Convert.ToDouble(gps[5]); }
            }

            if (ip != "Default")
            {
                List<string> ips = Tools.MultiLine2List(ip);

                if (ips[0] != "Default") { object_group.Initial_Temperature = Convert.ToDouble(ips[0]); }
                if (ips[1] != "Default") { object_group.Initial_Humidity = Convert.ToDouble(ips[1]); }
                if (ips[2] != "Default") { object_group.Initial_Contaminat = Convert.ToDouble(ips[2]); }
                if (ips[3] != "Default") { object_group.Initial_Other1 = Convert.ToDouble(ips[3]); }
                if (ips[4] != "Default") { object_group.Initial_Other2 = Convert.ToDouble(ips[4]); }
                if (ips[5] != "Default") { object_group.Initial_Other3 = Convert.ToDouble(ips[5]); }
            }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Passive_object_Fluid;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{2af1e255-b289-4b1e-b4f1-f8c50636946b}"); }
        }
    }
}