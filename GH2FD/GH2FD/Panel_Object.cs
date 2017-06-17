using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Passive_Panel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Passive_Panel()
            : base("Passive Panel", "Panel",
                "Convert a Mesh into Passive objects in Panel shape",
                "FlowDesigner", "01 Panel Objects")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Attribute", "AT", "Attribute\r\n" +
                "    0: Adiabatic\r\n" +
            "    1: Heat transmissivity\r\n" +
            "    2: Pseudo solid\r\n", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Adiabatic", 0);
            p1.AddNamedValue("Heat transmissivity", 1);
            p1.AddNamedValue("Pseudo solid", 2);
            //2
            pManager.AddTextParameter("Heat Transmissivity", "HT", "Heat Transmissivity [W/(m2.C)]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Material", "MA", "The name of the material. Please check the material list in FD", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("Thickness", "TH", "Thickness [m]", GH_ParamAccess.item, "Default");
            //5
            pManager.AddTextParameter("Heat Generation", "HG", "Heat Generation [W/m3]", GH_ParamAccess.item, "Default");
            //6
            pManager.AddTextParameter("Heat Transfer Coef Plus", "HP", "Heat Transfer Coef Plus [W/(m2.C)]", GH_ParamAccess.item, "Default");
            //7
            pManager.AddTextParameter("Heat Transfer Coef Minus", "HM", "Heat Transfer Coef Minus [W/(m2.C)]", GH_ParamAccess.item, "Default");
            //8
            pManager.AddTextParameter("Initial Temperature", "IT", "Initial Temperature [C]", GH_ParamAccess.item, "Default");
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

            FD_Passive_Panel object_group = new FD_Passive_Panel(Tools.GeneratePanelList(mesh_list));

            int at = 0;
            string ht = "";
            string ma = "";
            string th = "";
            string hg = "";
            string hp = "";
            string hm = "";
            string it = "";

            DA.GetData(1, ref at);
            DA.GetData(2, ref ht);
            DA.GetData(3, ref ma);
            DA.GetData(4, ref th);
            DA.GetData(5, ref hg);
            DA.GetData(6, ref hp);
            DA.GetData(7, ref hm);
            DA.GetData(8, ref it);

            object_group.Attribute = at;
            if (ht != "Default") { object_group.Heat_Transmissivity = Convert.ToDouble(ht); }
            if (ma != "Default") { object_group.Material = ma; }
            if (th != "Default") { object_group.Thickness = Convert.ToDouble(th); }
            if (hg != "Default") { object_group.Heat_Generation = Convert.ToDouble(hg); }
            if (hp != "Default") { object_group.Heat_Transfer_Coef_Plus = Convert.ToDouble(hp); }
            if (hm != "Default") { object_group.Heat_Transfer_Coef_Minus = Convert.ToDouble(hm); }
            if (it != "Default") { object_group.Initial_Temperature = Convert.ToDouble(it); }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Passive_panel;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{d024a2af-f8f5-4c9c-8d21-b8ec6981544d}"); }
        }
    }
}