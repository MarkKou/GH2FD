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
            pManager.AddBrepParameter("B", "B", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("H", "H", "", GH_ParamAccess.item, 1.0);
            pManager.AddBooleanParameter("R", "R", "", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("C", "C", "", GH_ParamAccess.item);
            pManager.AddBrepParameter("B", "B", "", GH_ParamAccess.item);
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

            foreach (BrepVertex bv in basebrep.Vertices)
            {
                ps.Add(bv.Location);
            }

            Vector3d nv = new Vector3d(0, 0, 0);

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                if (j == 3) j = 0;
                nv.X += (((ps[i].Z) + (ps[j].Z)) * ((ps[j].Y) - (ps[i].Y)));
                nv.Y += (((ps[i].X) + (ps[j].X)) * ((ps[j].Z) - (ps[i].Z)));
                nv.Z += (((ps[i].Y) + (ps[j].Y)) * ((ps[j].X) - (ps[i].X)));
            }

            nv.Unitize();

            if (revers)
            {
                height = -height;
            }

            nv = new Vector3d(nv.X * height, nv.Y * height, nv.Z * height);

            List<FD_Vertex> basepolyline = new List<FD_Vertex>();

            foreach (Point3d pt in ps)
            {
                basepolyline.Add(new FD_Vertex(pt.X, pt.Y, pt.Z));
            }

            ps.Add(basebrep.Vertices[0].Location);

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
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{f06708f4-4cc0-4639-a370-cc257f8ed057}"); }
        }
    }
}