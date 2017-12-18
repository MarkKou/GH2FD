using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Inlet : GH_Component
    {
        public Inlet()
            : base("Inlet", "Inlet",
                "Convert a Mesh into Passive Inlets",
                "FlowDesigner", Tools.sub_cate_01)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddMeshParameter("Mesh", "M", "Mesh to be converted", GH_ParamAccess.list);
            //1
            pManager.AddIntegerParameter("Method", "Me", "Method\r\n    0: Flow speed\r\n    1: Flow volume", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Flow speed", 0);
            p1.AddNamedValue("Flow volume", 1);
            //2
            pManager.AddTextParameter("Flow Speed", "FS", "Flow speed [m/s]", GH_ParamAccess.item, "Default");
            //3
            pManager.AddTextParameter("Flow Volume", "FV", "Flow volume [m3/min]", GH_ParamAccess.item, "Default");
            //4
            pManager.AddIntegerParameter("Balance Priority", "BP", "Balance Priority\r\n    0: Keep flow speed\r\n    1: Keep flow volume", GH_ParamAccess.item, 0);
            Param_Integer p4 = (Param_Integer)pManager[4];
            p4.AddNamedValue("Keep flow speed", 0);
            p4.AddNamedValue("Keep flow volume", 1);
            //5
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

            FD_Inlet object_group = new FD_Inlet(Tools.GeneratePanelList(mesh_list));

            int method = 0;
            string speed = "";
            string volume = "";
            int balance_prio = 0;
            string air_prop = "";

            DA.GetData(1, ref method);
            DA.GetData(2, ref speed);
            DA.GetData(3, ref volume);
            DA.GetData(4, ref balance_prio);
            DA.GetData(5, ref air_prop);

            object_group.Method = method;

            if (speed != "Default")
            {
                object_group.Speed = Convert.ToDouble(speed);
            }

            if (volume != "Default")
            {
                object_group.Volume = Convert.ToDouble(volume);
            }

            object_group.Balance_Prio = balance_prio;

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
                return Properties.Resources.inlet;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{d024a2af-f8f5-4c9c-8d21-b8ec6981544c}"); }
        }
    }
}