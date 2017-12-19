using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace GH2FD
{
    public class Perf_Panel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Perf_Panel()
            : base("Porfrated panel", "PPanel",
                "Convert a Mesh into Passive Porfrated panels",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddTextParameter("Perforated Ratio", "PR", "Perforated Ratio: 0 - 100; Default: 100", GH_ParamAccess.item, "100");
            //2
            pManager.AddTextParameter("Friction Coef", "FC", "Friction Coef: 0 -1; Default: 0", GH_ParamAccess.item, "0");
            //3
            pManager.AddTextParameter("Pressure Drop Exponent", "PD", "Pressure Drop Exponent; Default: 2", GH_ParamAccess.item, "2");
            //4
            pManager.AddIntegerParameter("Macro Model", "MM", "Macro Model\r\n    0: Filter\r\n    1: Louver", GH_ParamAccess.item, 0);
            Param_Integer p4 = (Param_Integer)pManager[4];
            p4.AddNamedValue("Filter", 0);
            p4.AddNamedValue("Louver", 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter(Tools.c_o_n, Tools.c_o_s, Tools.c_o_d, GH_ParamAccess.item);
            //1
            pManager.AddGeometryParameter("Normal Vector", "NV", "Display the normal vector", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(0, mesh_list);

            FD_Perf_Panel object_group = new FD_Perf_Panel(Tools.GeneratePanelList(mesh_list));

            //Set properties
            string pr = "";
            string fc = "";
            string pd = "";
            int mm = 0;
            DA.GetData(1, ref pr);
            DA.GetData(2, ref fc);
            DA.GetData(3, ref pd);
            DA.GetData(4, ref mm);

            if (pr != "Default")
            {
                object_group.Perf_ratio = Convert.ToInt32(pr); 
            }

            if (fc != "Default")
            {
                object_group.Fri_coef = Convert.ToDouble(fc); 
            }

            if (pd != "Default")
            {
                object_group.Pre_drop = Convert.ToDouble(pd); 
            }

            object_group.Macro_model = mm;

            List<GH_Surface> arrow_list = Tools.GenerateNormalArrow(mesh_list);

            DA.SetData(0, object_group);
            DA.SetDataList(1, arrow_list);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Porfrated_Panel;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{9e1541b1-951d-4432-a5a6-3acec7c6c287}"); }
        }
    }
}