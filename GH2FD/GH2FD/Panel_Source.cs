using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Panel_Source : GH_Component
    {
        public Panel_Source()
            : base("Panel Source", "PSource",
                "Convert a Mesh into Passive Panel Source",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Temperature Type", "TT", "Temperature Type\r\n" +
                "    0: Fixed surface temperature\r\n" +
                "    1: Total surface heat generated\r\n" +
                "    2: Heat generated per unit area\r\n" +
                "    3: Heat transfer coef between external air and solid surface\r\n" +
                "    4: External air temperature and overall heat transfer coef to internal surface\r\n" +
                "    5: External air temperature and overall heat transfer coef", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Fixed surface temperature", 0);
            p1.AddNamedValue("Total surface heat generated", 1);
            p1.AddNamedValue("Heat generated per unit area", 2);
            p1.AddNamedValue("Heat transfer coef between external air and solid surface", 3);
            p1.AddNamedValue("External air temperature and overall heat transfer coef to internal surface", 4);
            p1.AddNamedValue("External air temperature and overall heat transfer coef", 5);
            //2
            pManager.AddTextParameter("Surface Temperature", "ST", "Surface Temperature [°C]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Heat Generation", "HG", "Heat Generation\r\n" +
                "    Total surface heat generated: [W]\r\n" +
                "    Heat generated per unit area: [W/m2]", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("External Temperature", "ET ", "External Temperature [°C]", GH_ParamAccess.item, "Default");
            //5
            pManager.AddTextParameter("Overall Heat Transfer", "OH", "Overall Heat Transfer [W/(m2.°C)]", GH_ParamAccess.item, "Default");
            //6
            pManager.AddTextParameter("External Surface Heat Transfer", "EH", "External Surface Heat Transfer [W/(m2.°C)]", GH_ParamAccess.item, "Default");
            //7
            pManager.AddTextParameter("Internal Surface Heat Transfer", "IH", "Internal Surface Heat Transfer [W/(m2.°C)]; 0 for auto calculation", GH_ParamAccess.item, "Default");
            //8
            pManager.AddIntegerParameter("Diffusion Type", "DT", "Diffusion Type\r\n    0: Total surface mass generated\r\n    1: Fixed surface value", GH_ParamAccess.item, 0);
            Param_Integer p8 = (Param_Integer)pManager[8];
            p8.AddNamedValue("Total surface mass generated", 0);
            p8.AddNamedValue("Fixed surface value", 1);
            //9
            pManager.AddTextParameter("Generation Properites", "GP", "Use the AP component to set humidity, etc.", GH_ParamAccess.item, "Default");
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

            FD_Panel_Source object_group = new FD_Panel_Source(Tools.GeneratePanelList(mesh_list));

            int TT = 0;
            string ST = "";
            string HG = "";
            string ET = "";
            string OH = "";
            string EH = "";
            string IH = "";
            int DT = 0;
            string GV = "";

            DA.GetData(1, ref TT);
            DA.GetData(2, ref ST);
            DA.GetData(3, ref HG);
            DA.GetData(4, ref ET);
            DA.GetData(5, ref OH);
            DA.GetData(6, ref EH);
            DA.GetData(7, ref IH);
            DA.GetData(8, ref DT);
            DA.GetData(9, ref GV);

            object_group.Temp_Type = TT;

            if (ST != "Default")
            {
                object_group.Surface_Temp = Convert.ToDouble(ST); 
            }

            if (HG != "Default")
            {
                object_group.Heat_Generation = Convert.ToDouble(HG); 
            }

            if (ET != "Default")
            {
                object_group.External_Temp = Convert.ToDouble(ET); 
            }

            if (OH != "Default")
            {
                object_group.Heat_Trans = Convert.ToDouble(OH); 
            }

            if (EH != "Default")
            {
                object_group.External_HTrans = Convert.ToDouble(EH); 
            }

            if (IH != "Default")
            {
                object_group.Internal_HTrans = Convert.ToDouble(IH); 
            }

            object_group.Diff_Type = DT;

            if (GV != "Default")
            {
                List<string> gen_properties = Tools.MultiLine2List(GV);

                if (gen_properties[1] != "Default")
                {
                    object_group.Humi_Gene = Convert.ToDouble(gen_properties[1]); 
                }

                if (gen_properties[2] != "Default")
                {
                    object_group.Conta_Genen = Convert.ToDouble(gen_properties[2]); 
                }

                if (gen_properties[3] != "Default")
                {
                    object_group.Other1 = Convert.ToDouble(gen_properties[3]); 
                }

                if (gen_properties[4] != "Default")
                {
                    object_group.Other2 = Convert.ToDouble(gen_properties[4]); 
                }

                if (gen_properties[5] != "Default")
                {
                    object_group.Other3 = Convert.ToDouble(gen_properties[5]); 
                }
            }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.panel_source;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{469a8668-2c83-4d27-8ba1-ca4fa94219a2}"); }
        }
    }
}