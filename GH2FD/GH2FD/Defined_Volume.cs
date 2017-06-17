using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Defined_Volume : GH_Component
    {
        public Defined_Volume()
            : base("Defined_Volume", "DVolume",
                "Convert a Box into a Defined_Volume",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Boxes", "B", "Boxes to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Type", "TY", "Type\r\n    0: Simple volume\r\n    1: Nesting volume", GH_ParamAccess.item, 0);
            Param_Integer p1 = pManager[1] as Param_Integer;
            p1.AddNamedValue("Simple volume", 0);
            p1.AddNamedValue("Nesting volume", 1);
            //2
            pManager.AddTextParameter("X Plus", "XP", @"Apply X+ surface: 'Ture' or 'False'", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("X Minus", "XM", @"Apply X- surface: 'Ture' or 'False'", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("Y Plus", "YP", @"'Apply Y+ surface: Ture' or 'False'", GH_ParamAccess.item, "Default");
            //5
            pManager.AddTextParameter("Y Minus", "YM", @"'Apply Y- surface: Ture' or 'False'", GH_ParamAccess.item, "Default");
            //6
            pManager.AddTextParameter("Z Plus", "ZP", @"'Apply Z+ surface: Ture' or 'False'", GH_ParamAccess.item, "Default");
            //7
            pManager.AddTextParameter("Z Minus", "YM", @"'Apply Z- surface: Ture' or 'False'", GH_ParamAccess.item, "Default");
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

            FD_Defined_Volume object_group = new FD_Defined_Volume(Tools.GenerateCubeList(items));

            int type = 0;
            string xplus = "";
            string xminus = "";
            string yplus = "";
            string yminus = "";
            string zplus = "";
            string zminus = "";

            DA.GetData(1, ref type);
            DA.GetData(2, ref xplus);
            DA.GetData(3, ref xminus);
            DA.GetData(4, ref yplus);
            DA.GetData(5, ref yminus);
            DA.GetData(6, ref zplus);
            DA.GetData(7, ref zminus);

            object_group.Type = type;
            if (xplus != "Default") { object_group.X_Plus = Convert.ToBoolean(xplus); }
            if (xminus != "Default") { object_group.X_Minus = Convert.ToBoolean(xminus); }
            if (yplus != "Default") { object_group.Y_Plus = Convert.ToBoolean(yplus); }
            if (yminus != "Default") { object_group.Y_Minus = Convert.ToBoolean(yminus); }
            if (zplus != "Default") { object_group.Z_Plus = Convert.ToBoolean(zplus); }
            if (zminus != "Default") { object_group.Z_Minus = Convert.ToBoolean(zminus); }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Defined_Volume;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{8b1ec1b1-69f6-47a3-b0e7-bc7982b86cc0}"); }
        }
    }
}