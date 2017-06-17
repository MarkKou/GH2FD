using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Air_prop_in_op : GH_Component
    {
        public Air_prop_in_op()
            : base("Air Properties", @"AP",
                "Set air properties for inlet or opening",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("Tempearture", "Te", "Tempearture", GH_ParamAccess.item, "Default");
            //1
            pManager.AddTextParameter("Humidity", "Hu",  "Humidity", GH_ParamAccess.item, "Default");
            //2
            pManager.AddTextParameter("Contanmination", "Co", "Contanmination", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Other1", "O1", "Concentration of other material 1", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("Other2", "O2", "Concentration of other material 2]", GH_ParamAccess.item, "Default");
            //5
            pManager.AddTextParameter("Other3", "O3", "Concentration of other material 3]", GH_ParamAccess.item, "Default");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("Values", "V", "Values of all the properties", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string[] settings = new string[6];

            for (int i = 0; i < 6; i++)
            {
                settings[i] = "";
                DA.GetData(i, ref settings[i]);
            }

            DA.SetData(0, Tools.Array2MultiLine(settings));
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.air_properties;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{ecc8c227-f7ae-4fee-aced-7f0831e98e3c}"); }
        }
    }

    //public class Pse_sol_con_ps : GH_Component
    //{
    //    public Pse_sol_con_ps()
    //        : base("Panel_soure_setting", "Nickname",
    //            "Description",
    //            "Category", "Subcategory")
    //    {
    //    }

    //    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    //    {
    //    }

    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //    }

    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //    }

    //    protected override System.Drawing.Bitmap Icon
    //    {
    //        get
    //        {
    //            return null;
    //        }
    //    }

    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("{e2f86e4f-40ad-4b11-a6c7-c9217bf4ef8d}"); }
    //    }
    //}

    //public class Diff_mat_ps : GH_Component
    //{
    //    public Diff_mat_ps()
    //        : base("Panel_soure_setting", "Nickname",
    //            "Description",
    //            "Category", "Subcategory")
    //    {
    //    }

    //    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    //    {
    //    }

    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //    }

    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //    }

    //    protected override System.Drawing.Bitmap Icon
    //    {
    //        get
    //        {
    //            return null;
    //        }
    //    }

    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("{83f843b5-041b-4603-a191-5bc1083d394e}"); }
    //    }
    //}
}