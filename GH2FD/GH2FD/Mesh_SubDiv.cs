using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Mesh_SubDiv : GH_Component
    {
        public Mesh_SubDiv()
            : base("Sub Mesh", "Sub Mesh",
                "Set the sub-division of the analysis mesh",
                "FlowDesigner", Tools.sub_cate_03)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddBoxParameter("Range Box", "RB", "", GH_ParamAccess.item);
            //1
            pManager.AddIntegerParameter("Division Scheme", "DS", "Division Scheme\r\n" +
                "    0: Evenly\r\n" +
                "    1: Refine lower coordinate proportionally\r\n" +
                "    2: Refine both side proportionally\r\n" +
                "    3: Refine higher coordinate proportionally", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("Evenly", 0);
            p1.AddNamedValue("Refine lower coordinate proportionally", 1);
            p1.AddNamedValue("Refine both side proportionally", 2);
            p1.AddNamedValue("Refine higher coordinate proportionally", 3);
            //2
            pManager.AddIntegerParameter("Mode", "Mo", "If DS = 0\r\n    0: By element number\r\n    1: By distance\r\nElse\r\n    0: By proportion\r\n    1: By prime mesh size", GH_ParamAccess.item,0);
            Param_Integer p2 = (Param_Integer)pManager[2];
            p2.AddNamedValue("By element number / By proportion", 0);
            p2.AddNamedValue("By distance / By prime mesh size", 1);
            //3
            pManager.AddNumberParameter("Proportion or Prime mesh size", "PP", "If DS = 0: No Effect\r\nElse\r\n    If Me = 0: Proportion\r\n    If Me = 1: Prime mesh size", GH_ParamAccess.item,0);
            //4
            pManager.AddNumberParameter("Distance or Element count in X axis", "X", "If DS = 0 and Me = 1: Distance\r\nElse: Element count\r\nSet this parameter to 0 if you do not want to divide x axis", GH_ParamAccess.item,0);
            //5
            pManager.AddNumberParameter("Distance or Element count in Y axis", "Y", "If DS = 0 and Me = 1: Distance\r\nElse: Element count\r\nSet this parameter to 0 if you do not want to divide x axis", GH_ParamAccess.item, 0);
            //6
            pManager.AddNumberParameter("Distance or Element count in Z axis", "Z", "If DS = 0 and Me = 1: Distance\r\nElse: Element count\r\nSet this parameter to 0 if you do not want to divide x axis", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Sub Division", "SD", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Box rb = new Box();
            int ds = 0;
            int me = 0;
            double pp = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            DA.GetData(0, ref rb);
            DA.GetData(1, ref ds);
            DA.GetData(2, ref me);
            DA.GetData(3, ref pp);
            DA.GetData(4, ref x);
            DA.GetData(5, ref y);
            DA.GetData(6, ref z);

            FD_Mesh.FD_Sub_Division sub = new FD_Mesh.FD_Sub_Division(ds, me);
            sub.Ratio = pp;

            Point3d[] verices = rb.GetCorners();
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

            if (x != 0)
            {
                sub.X0 = min.X;
                sub.X1 = max.X;
                sub.X_Count_or_Distance = x;
            }

            if (y != 0)
            {
                sub.Y0 = min.Y;
                sub.Y1 = max.Y;
                sub.Y_Count_or_Distance = y;
            }

            if (z != 0)
            {
                sub.Z0 = min.Z;
                sub.Z1 = max.Z;
                sub.Z_Count_or_Distance = z;
            }

            DA.SetData(0, sub);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Mesh_Sub;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{ed4a0254-8600-4f59-9d4c-27db4a22ebf9}"); }
        }
    }
}