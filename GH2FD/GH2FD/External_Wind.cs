using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class External_Wind : GH_Component
    {
        public External_Wind()
            : base("External Wind", "Ex Wind",
                "Set configurations of the external wind",
                "FlowDesigner", Tools.sub_cate_00)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddIntegerParameter("Terrain Type", "TT", "Terrain Type\r\n" +
                "    1: Elevation independent\r\n" +
            "    2: Major city center\r\n" +
            "    3: Major city suburban\r\n" +
            "    4: Suburban\r\n" +
            "    5: Plane\r\n", GH_ParamAccess.item, 3);
            Param_Integer p0 = (Param_Integer)pManager[0];
            p0.AddNamedValue("Elevation independent", 1);
            p0.AddNamedValue("Major city center", 2);
            p0.AddNamedValue("Major city suburban", 3);
            p0.AddNamedValue("Suburban", 4);
            p0.AddNamedValue("Plane", 5);
            //1
            pManager.AddNumberParameter("Wind Direction", "WD", "Degree and Clockwise\r\n    N: 0\r\n    E: 90\r\n    S: 180\r\n    W: 270", GH_ParamAccess.item,0);
            //2
            pManager.AddNumberParameter("Wind Speed", "WS", "Wind Speed [m/s]", GH_ParamAccess.item, 0);
            //3
            pManager.AddNumberParameter("Reference Height", "RH", "Height of the observation point", GH_ParamAccess.item, 10);
            //4
            pManager.AddTextParameter("Air Propertities", "AP", "Use the AP component to set temperature, humidity, etc.", GH_ParamAccess.item, "Unset");
            //5
            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("External Wind", "EW", "External Wind object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int tt = 0;
            double wd = 0;
            double ws = 0;
            double rh = 0;
            string ap = "";

            DA.GetData(0, ref tt);
            DA.GetData(1, ref wd);
            DA.GetData(2, ref ws);
            DA.GetData(3, ref rh);
            DA.GetData(4, ref ap);

            wd = 360 - (wd + 90);
            if (wd < 0) { wd = wd + 360; }

            FD_External_Wind fd_ew = new FD_External_Wind();

            fd_ew.useouterairflow = true;
            fd_ew.outerairflowtype = tt;
            fd_ew.outerairflowdirtype = 1;
            fd_ew.outerairflowmanualdir = wd;
            fd_ew.outerairflowspeed = ws;
            fd_ew.outerairflowheight = rh;

            if (ap != "Unset")
            {
                List<string> air_ps = Tools.MultiLine2List(ap);

                if (air_ps[0] != "Default") { fd_ew.outerairflowtemperature = Convert.ToDouble(air_ps[0]); }
                if (air_ps[1] != "Default") { fd_ew.outerairflowhumidity = Convert.ToDouble(air_ps[1]); }
                if (air_ps[2] != "Default")
                {
                    fd_ew.useouterflowcontamination = true;
                    fd_ew.outerflowcontamination = Convert.ToDouble(air_ps[2]);
                }
                if (air_ps[3] != "Default")
                {
                    fd_ew.useouterflowotherdensity1 = true;
                    fd_ew.outerflowotherdensity1 = Convert.ToDouble(air_ps[3]);
                }
                if (air_ps[4] != "Default")
                {
                    fd_ew.useouterflowotherdensity2 = true;
                    fd_ew.outerflowotherdensity2 = Convert.ToDouble(air_ps[4]);
                }
                if (air_ps[5] != "Default")
                {
                    fd_ew.useouterflowotherdensity3 = true;
                    fd_ew.outerflowotherdensity3 = Convert.ToDouble(air_ps[5]);
                }
            }

            DA.SetData(0, fd_ew);


            bool go = false;
            DA.GetData(5, ref go);

            if (go) { fd_ew.Update(); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.External_Wind;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{024abd5e-b604-400a-93f4-ca630f80b468}"); }
        }
    }
}