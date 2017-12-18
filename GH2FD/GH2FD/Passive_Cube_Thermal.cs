using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Passive_Cube_Thermal : GH_Component
    {
        public Passive_Cube_Thermal()
            : base("Passive_Cube (Thermal object)", "PCube T",
                "Convert a Box into a Passive cube (Thermal object)",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Boxes", "B", "Boxes to be converted", GH_ParamAccess.list);
            //1
            pManager.AddTextParameter("Material", "MA", "The name of the material. Please check the material list in FD", GH_ParamAccess.item, "Default");
            //2
            pManager.AddTextParameter("Heat Generation", "HG", "Heat Generation [W]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Heat Transfer Coefficient", "HT", "Heat Transfer Coefficient [W/(m2.C)]", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("Initial Temperature", "IT", "Initial Temperature [C]", GH_ParamAccess.item, "Default");
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

            string ma = "";
            string hg = "";
            string ht = "";
            string it = "";

            DA.GetData(1, ref ma);
            DA.GetData(2, ref hg);
            DA.GetData(3, ref ht);
            DA.GetData(4, ref it);

            object_group.Attribute = 2;
            if (ma != "Default") { object_group.Material = ma; }
            if (hg != "Default") { object_group.Heat_Generation = Convert.ToDouble(hg); }
            if (ht != "Default") { object_group.Heat_Trans_Coef = Convert.ToDouble(ht); }
            if (it != "Default") { object_group.Initial_Temperature = Convert.ToDouble(it); }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Passive_object_Thermal;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{b6717cf2-7191-4b34-aa1b-036c34d73075}"); }
        }
    }
}