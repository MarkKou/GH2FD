using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Opening : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Opening()
            : base("Opening", "Opening",
                "Convert a Mesh into Passive Openings",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Type", "Ty", "Type\r\n    0: Free in and out flow\r\n    1: Specified pressure\r\n    2: Perforated ratio", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Free in and out flow", 0);
            p1.AddNamedValue("Specified pressure", 1);
            p1.AddNamedValue("Perforated ratio", 2);
            //2
            pManager.AddTextParameter("External Pressure", "EP", "External Pressure[Pa]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Perforated Ratio", "PR", "Perforated Ratio: 0 - 100; Default: 100", GH_ParamAccess.item, "100");
            //4
            pManager.AddTextParameter("Friction Coef", "FC", "Friction Coef: 0 -1; Default: 0", GH_ParamAccess.item, "0");
            //5
            pManager.AddTextParameter("Pressure Drop Exponent", "PD", "Pressure Drop Exponent; Default: 2", GH_ParamAccess.item, "2");
            //6
            pManager.AddTextParameter("Air Properties", "AP", "Use the AP component to set temperature, humidity, etc.", GH_ParamAccess.item, "Default");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter(Tools.c_o_n, Tools.c_o_s, Tools.c_o_d, GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(0, mesh_list);

            FD_Opening object_group = new FD_Opening(Tools.GeneratePanelList(mesh_list));

            int type = 0;
            string pressure = ""; 
            string perf_ratio = ""; 
            string fri_coef = ""; 
            string pre_drop = "";
            string air_prop = "";

            DA.GetData(1, ref type);
            DA.GetData(2, ref pressure);
            DA.GetData(3, ref perf_ratio);
            DA.GetData(4, ref fri_coef);
            DA.GetData(5, ref pre_drop);
            DA.GetData(6, ref air_prop);

            object_group.Type = type;

            if (pressure != "Default")
            {
                object_group.Pressure = Convert.ToInt32(pressure);
            }

            if (perf_ratio != "Default")
            {
                object_group.Perf_ratio = Convert.ToInt32(perf_ratio);
            }

            if (fri_coef != "Default")
            {
                object_group.Fri_coef = Convert.ToDouble(fri_coef);
            }

            if (pre_drop != "Default")
            {
                object_group.Pre_drop = Convert.ToDouble(pre_drop);
            }

            if (air_prop != "Default")
            {
                List<string> air_properties = Tools.MultiLine2List(air_prop);

                if (air_properties[0] != "Default")
                {
                    object_group.Temperature = Convert.ToDouble(air_properties[0]);
                }

                if (air_properties[1] != "Default")
                {
                    object_group.R_Humidity = Convert.ToDouble(air_properties[1]);
                }

                if (air_properties[2] != "Default")
                {
                    object_group.Contamination = Convert.ToDouble(air_properties[2]);
                }

                if (air_properties[3] != "Default")
                {
                    object_group.Other1 = Convert.ToDouble(air_properties[3]);
                }

                if (air_properties[4] != "Default")
                {
                    object_group.Other2 = Convert.ToDouble(air_properties[4]);
                }

                if (air_properties[5] != "Default")
                {
                    object_group.Other3 = Convert.ToDouble(air_properties[5]);
                }
            }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.opening;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{096dc568-c556-4d90-b944-afbcab222aeb}"); }
        }
    }
}