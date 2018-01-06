using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class FD_Extrusion : GH_Component
    {
        public FD_Extrusion()
            : base("FD Extrusion", "FD Ex",
                "Description",
                "FlowDesigner", Tools.sub_cate_02)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Base Brep", "B", "A planar brep used as the base of extrusion", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "H", "Height of the extrusion", GH_ParamAccess.item, 1.0);
            pManager.AddBooleanParameter("Reverse", "R", "Reverse the extrusion direction", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FD Extrusion", "G", @"A FD Extrusion object, connect this output to a component in '02 Cube Objects'", GH_ParamAccess.item);
            pManager.AddBrepParameter("Shape", "S", "Just for display", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep basebrep = new Brep();
            double height = 0;
            bool revers = true;

            DA.GetData(0, ref basebrep);
            DA.GetData(1, ref height);
            DA.GetData(2, ref revers);

            List<Point3d> ps = new List<Point3d>();
            List<BrepEdge> be_list = new List<BrepEdge>();

            foreach (BrepTrim btr in basebrep.Loops[0].Trims)
            {
                be_list.Add(btr.Edge);
            }

            AddFirstEdge(be_list[0], be_list[1]);

            for (int i = 1; i < be_list.Count - 1; i++)
            {
                AddEdge(be_list[i]);
            }

            if (revers)
            {
                ps.Reverse();
            }

            Vector3d nv = Tools.GetFaceNormal(ps)[0];

            nv = new Vector3d(nv.X * height, nv.Y * height, nv.Z * height);

            List<FD_Vertex> basepolyline = new List<FD_Vertex>();

            foreach (Point3d pt in ps)
            {
                basepolyline.Add(new FD_Vertex(pt.X, pt.Y, pt.Z));
            }

            ps.Add(ps[0]);

            PolylineCurve pc = new PolylineCurve(ps);

            Surface extrusion = Surface.CreateExtrusion(pc, nv);

            List<Brep> bl = new List<Brep>();
            bl.Add(basebrep);
            bl.Add(extrusion.ToBrep());

            Brep step1 = Brep.JoinBreps(bl, 0.1)[0];

            basebrep.Translate(nv);

            bl = new List<Brep>();
            bl.Add(basebrep);
            bl.Add(step1);

            Brep step2 = Brep.JoinBreps(bl, 0.1)[0];

            FD_Cube cube = new FD_Cube(basepolyline, height);

            DA.SetData(0, cube);
            DA.SetData(1, step2);

            bool SamePoint(Point3d p1, Point3d p2)
            {
                double dx = Math.Abs(p1.X - p2.X);
                double dy = Math.Abs(p1.Y - p2.Y);
                double dz = Math.Abs(p1.Z - p2.Z);

                if (dx < 0.001 && dy < 0.001 && dz < 0.001) { return true; }
                else { return false; }
            }

            void AddEdge(BrepEdge be)
            {
                if(SamePoint(ps[ps.Count-1],be.PointAtStart))
                { ps.Add(be.PointAtEnd); }
                else
                { ps.Add(be.PointAtStart); }
            }

            void AddFirstEdge(BrepEdge be0, BrepEdge be1)
            {
                ps.Clear();

                if(SamePoint( be0.PointAtEnd,be1.PointAtEnd)|| SamePoint(be0.PointAtEnd, be1.PointAtStart))
                {
                    ps.Add(be0.PointAtStart);
                    ps.Add(be0.PointAtEnd);
                }
                else
                {
                    ps.Add(be0.PointAtEnd);
                    ps.Add(be0.PointAtStart);
                }
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.FD_Extrusion;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{f06708f4-4cc0-4639-a370-cc257f8ed057}"); }
        }
    }
}