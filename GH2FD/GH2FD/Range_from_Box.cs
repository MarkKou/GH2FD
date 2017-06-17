using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Range_from_Box : GH_Component
    {
        public Range_from_Box()
            : base("Range from Box", "ARange B",
                "Description",
                "FlowDesigner", Tools.sub_cate_00)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddIntegerParameter("Unit", "U", "SI Unit\r\n    0: m\r\n    1: cm\r\n    2: mm\r\n    3: μm\r\n" +
                "US Unit\r\n    0: Yard\r\n    1: Feet\r\n    2: Inch", GH_ParamAccess.item, 0);
            Param_Integer p0 = (Param_Integer)pManager[0];
            p0.AddNamedValue("Meter (m) / Yard", 0);
            p0.AddNamedValue("Centimetre (cm) / Feet", 1);
            p0.AddNamedValue("Millimetre (mm) / Inch", 2);
            p0.AddNamedValue("Micrometre (µm)", 3);
            //1
            pManager.AddBoxParameter("Box", "B", "A Box to be converted to Analysis Range", GH_ParamAccess.item);
            //2
            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Analysis Range", "AR", "Analysis Range object", GH_ParamAccess.item);
            //1
            pManager.AddTextParameter("Values", "V", "The values of properties if the Analysis Range", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int unit = 0;
            Box box = new Box();
            bool run = false;

            DA.GetData(0, ref unit);
            DA.GetData(1, ref box);
            DA.GetData(2, ref run);

            Point3d[] verices = box.GetCorners();
            Point3d min = verices[0];
            Point3d max = verices[0];

            foreach (Point3d item in verices)
            {
                if (item.X < min.X || item.Y < min.Y || item.Z < min.Z)
                {
                    min = item;
                }
                if (item.X > max.X || item.Y > max.Y || item.Z > max.Z)
                {
                    max = item;
                }
            }

            FD_Range range = new FD_Range(unit, max.X - min.X, max.Y - min.Y, max.Z - min.Z, min.X, min.Y, min.Z);

            string values = "Unit: " + range.Unit.ToString() + Environment.NewLine;
            values += "Width: " + range.Width.ToString() + Environment.NewLine;
            values += "Deepth: " + range.Deepth.ToString() + Environment.NewLine;
            values += "Height: " + range.Height.ToString() + Environment.NewLine;
            values += "Offset_X: " + range.Offset_X.ToString() + Environment.NewLine;
            values += "Offset_Y: " + range.Offset_Y.ToString() + Environment.NewLine;
            values += "Offset_Z: " + range.Offset_Z.ToString();

            DA.SetData(0, range);
            DA.SetData(1, values);

            if (run)
            {
                range.Update();
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.analysis_range_Box;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{2f2c798a-9bef-468a-a46b-899412de96e2}"); }
        }
    }
}