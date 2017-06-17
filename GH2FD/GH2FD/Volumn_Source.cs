using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Volumn_Source : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Volumn_Source()
            : base("Volumn Source", "VSource",
                "Convert a Box into a Volumn Source",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Boxes", "B", "Boxes to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Generation Type", "GT", "Generation Type\r\n    0: Total\r\n    1: Per unit volume", GH_ParamAccess.item, 0);
            Param_Integer p1 = pManager[1] as Param_Integer;
            p1.AddNamedValue("Total", 0);
            p1.AddNamedValue("Per unit volume", 1);
            //2
            pManager.AddTextParameter("Heat Generation", "HG", "Heat Generation\r\n    Total: [W]\r\nPer unit volume: [W/m3]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Generation Properties", "GP", "Use the AP component to set humidity, etc.", GH_ParamAccess.item, "Default");
            //4
            pManager.AddTextParameter("Initial Properties", "IP", "Use the AP component to set temperature, humidity, etc.", GH_ParamAccess.item, "Default");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Cube", "C", "Cube converted from mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<dynamic> items = new List<dynamic>();
            DA.GetDataList(0, items);

            FD_Vol_Source object_group = new FD_Vol_Source(Tools.GenerateCubeList(items));

            int GT = 0;
            string HG = "";
            string GV = "";
            string IV = "";

            DA.GetData(1, ref GT);
            DA.GetData(2, ref HG);
            DA.GetData(3, ref GV);
            DA.GetData(4, ref IV);

            object_group.Gene_Type = GT;
            if (HG != "Default") { object_group.Heat_Gene = Convert.ToDouble(HG); }

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

            if (IV != "Default")
            {
                List<string> init_values = Tools.MultiLine2List(IV);

                if (init_values[0] != "Default")
                {
                    object_group.Init_Temp = Convert.ToDouble(init_values[0]);
                }

                if (init_values[1] != "Default")
                {
                    object_group.Init_Humi = Convert.ToDouble(init_values[1]);
                }

                if (init_values[2] != "Default")
                {
                    object_group.Init_Conta = Convert.ToDouble(init_values[2]);
                }

                if (init_values[3] != "Default")
                {
                    object_group.Init_Other1 = Convert.ToDouble(init_values[3]);
                }

                if (init_values[4] != "Default")
                {
                    object_group.Init_Other2 = Convert.ToDouble(init_values[4]);
                }

                if (init_values[5] != "Default")
                {
                    object_group.Init_Other3 = Convert.ToDouble(init_values[5]);
                }
            }

            DA.SetData(0, object_group);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Volume_source;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{b6f7e524-b0a4-4166-9889-1f385edd9ce8}"); }
        }
    }
}