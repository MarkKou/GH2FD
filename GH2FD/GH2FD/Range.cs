using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Range : GH_Component
    {
        public Range()
            : base("Analysis Range", "ARange",
                "Set the Analysis Range with this component.\r\nThis component will read in the current Analysis Range when initialized.",
                "FlowDesigner", Tools.sub_cate_00)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddIntegerParameter("Unit", "U", "SI Unit\r\n    0: m\r\n    1: cm\r\n    2: mm\r\n    3: μm\r\n"+
                "US Unit\r\n    0: Yard\r\n    1: Feet\r\n    2: Inch", GH_ParamAccess.item, 0);
            Param_Integer p0 = (Param_Integer)pManager[0];
            p0.AddNamedValue("Meter (m) / Yard", 0);
            p0.AddNamedValue("Centimetre (cm) / Feet", 1);
            p0.AddNamedValue("Millimetre (mm) / Inch", 2);
            p0.AddNamedValue("Micrometre (µm)", 3);
            //1
            pManager.AddNumberParameter("Width", "W", "Width of the Analysis Range", GH_ParamAccess.item, 0);
            //2
            pManager.AddNumberParameter("Deepth", "D", "Deepth of the Analysis Range", GH_ParamAccess.item, 0);
            //3
            pManager.AddNumberParameter("Height", "H", "Height of the Analysis Range", GH_ParamAccess.item, 0);
            //4
            pManager.AddNumberParameter("Offset X", "OX", "X coordinate of Origin Point", GH_ParamAccess.item, 0);
            //5
            pManager.AddNumberParameter("Offset Y", "OY", "Y coordinate of Origin Point", GH_ParamAccess.item, 0);
            //6
            pManager.AddNumberParameter("Offset Z", "OZ", "Z coordinate of Origin Point", GH_ParamAccess.item, 0);
            //7
            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Analysis Range", "AR", "Analysis Range object", GH_ParamAccess.item);
            //1
            pManager.AddBoxParameter("Range Box", "RB", "A Box stands for the Analysis Range", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int unit = 0;
            double width = 0;
            double deepth = 0;
            double height = 0;
            double offset_x = 0;
            double offset_y = 0;
            double offset_z = 0;
            bool run = false;

            DA.GetData(0, ref unit);
            DA.GetData(1, ref width);
            DA.GetData(2, ref deepth);
            DA.GetData(3, ref height);
            DA.GetData(4, ref offset_x);
            DA.GetData(5, ref offset_y);
            DA.GetData(6, ref offset_z);
            DA.GetData(7, ref run);

            FD_Range range;
            Box range_box;

            if (width != 0 && deepth != 0 && height != 0)
            {
                range = new FD_Range(unit, width, deepth, height, offset_x, offset_y, offset_z);
            }
            else
            {
                range = new FD_Range();
            }

            Plane baseplane = new Plane(new Point3d(range.Offset_X, range.Offset_Y, range.Offset_Z), new Vector3d(0, 0, 1));
            range_box = new Box(baseplane, new Interval(0, range.Width), new Interval(0, range.Deepth), new Interval(0, range.Height));

            DA.SetData(0, range);
            DA.SetData(1, range_box);

            if (run) { range.Update(); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Analysis_Range;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{6db1bf53-0250-48ba-b7ed-a8ff7f9f51f2}"); }
        }
    }
}