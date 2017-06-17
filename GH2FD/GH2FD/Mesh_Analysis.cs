using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Mesh_Analysis : GH_Component
    {
        public Mesh_Analysis()
            : base("Analysis Mesh", "AMesh",
                "Set the analysis mesh",
                "FlowDesigner", Tools.sub_cate_03)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Analysis Range", "AR", "Analysis Range", GH_ParamAccess.item);
            //1
            pManager.AddIntegerParameter("Mode", "M", "Mode\r\n    0: By element count\r\n    1: By distance", GH_ParamAccess.item, 0);
            Param_Integer p1 = (Param_Integer)pManager[1];
            p1.AddNamedValue("By element count", 0);
            p1.AddNamedValue("By distance", 1);
            //2
            pManager.AddNumberParameter("Element Count or Distance", "CD", "Element Count or Distance", GH_ParamAccess.item);
            //3
            pManager.AddGenericParameter("Sub Division", "SD", "Sub Division objects from Sub Mesh Component", GH_ParamAccess.list);
            //4
            pManager.AddBooleanParameter("Add Model Edge", "ME", "Add model edges or not. Defualt is true, but not recommended for complex shapes", GH_ParamAccess.item, true);
            //5
            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis Mesh", "AM", "Analysis Mesh object", GH_ParamAccess.item);
            pManager.AddLineParameter("Mesh Line", "ML", "Preview of the analysis mesh\r\nThe proportional sub-division will be displayed evenly", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FD_Range range = new FD_Range(1, 1, 1);
            int mode = 0;
            double cd = 0;
            List<FD_Mesh.FD_Sub_Division> sbs = new List<FD_Mesh.FD_Sub_Division>();
            bool me = true;
            bool run = false;

            DA.GetData(0, ref range);
            DA.GetData(1, ref mode);
            DA.GetData(2, ref cd);
            DA.GetDataList(3, sbs);
            DA.GetData(4, ref me);
            DA.GetData(5, ref run);

            List<double>[] meshes = new List<double>[0];

            if (mode == 0) { meshes = Divide_Range_by_Count(range, cd); }
            if (mode == 1) { meshes = Divide_Range_by_Dis(range, cd); }


            FD_Mesh analysis_mesh = new FD_Mesh(meshes[0], meshes[1], meshes[2]);
            analysis_mesh.Sub_Divisions = sbs;

            analysis_mesh.Add_Model_Edge = me;

            List<Line> mesh_line = new List<Line>();

            foreach (double item in meshes[0])
            {
                Point3d from = new Point3d(item, range.Offset_Y + range.Deepth, range.Offset_Z);
                Point3d to1 = new Point3d(item, range.Offset_Y, range.Offset_Z);
                Point3d to2 = new Point3d(item, range.Offset_Y + range.Deepth, range.Offset_Z + range.Height);
                mesh_line.Add(new Line(from, to1));
                mesh_line.Add(new Line(from, to2));
            }

            foreach (double item in meshes[1])
            {
                Point3d from = new Point3d(range.Offset_X + range.Width, item, range.Offset_Z);
                Point3d to1 = new Point3d(range.Offset_X, item, range.Offset_Z);
                Point3d to2 = new Point3d(range.Offset_X + range.Width, item, range.Offset_Z + range.Height);
                mesh_line.Add(new Line(from, to1));
                mesh_line.Add(new Line(from, to2));
            }

            foreach (double item in meshes[2])
            {
                Point3d from = new Point3d(range.Offset_X + range.Width, range.Offset_Y + range.Deepth, item);
                Point3d to1 = new Point3d(range.Offset_X, range.Offset_Y + range.Deepth, item);
                Point3d to2 = new Point3d(range.Offset_X + range.Width, range.Offset_Y, item);
                mesh_line.Add(new Line(from, to1));
                mesh_line.Add(new Line(from, to2));
            }

            foreach(FD_Mesh.FD_Sub_Division sd in sbs)
            {
                if (sd.sub_x)
                {
                    List<double> sb_mesh_x = new List<double>();

                    double space = 0;

                    if (sd.Scheme == 0 && sd.Mode == 1) { space = sd.X_Count_or_Distance; }
                    else { space = (sd.X1 - sd.X0) / sd.X_Count_or_Distance; }

                    double sub_div_x = sd.X0 + space;
                    while (sub_div_x < sd.X1)
                    {
                        sb_mesh_x.Add(sub_div_x);
                        sub_div_x += space;
                    }

                    foreach (double item in sb_mesh_x)
                    {
                        Point3d from = new Point3d(item, range.Offset_Y + range.Deepth, range.Offset_Z);
                        Point3d to1 = new Point3d(item, range.Offset_Y, range.Offset_Z);
                        Point3d to2 = new Point3d(item, range.Offset_Y + range.Deepth, range.Offset_Z + range.Height);
                        mesh_line.Add(new Line(from, to1));
                        mesh_line.Add(new Line(from, to2));
                    }
                }

                if (sd.sub_y)
                {
                    List<double> sb_mesh_y = new List<double>();

                    double space = 0;

                    if (sd.Scheme == 0 && sd.Mode == 1) { space = sd.Y_Count_or_Distance; }
                    else { space = (sd.Y1 - sd.Y0) / sd.Y_Count_or_Distance; }

                    double sub_div_y = sd.Y0 + space;
                    while (sub_div_y < sd.Y1)
                    {
                        sb_mesh_y.Add(sub_div_y);
                        sub_div_y += space;
                    }

                    foreach (double item in sb_mesh_y)
                    {
                        Point3d from = new Point3d(range.Offset_X + range.Width, item, range.Offset_Z);
                        Point3d to1 = new Point3d(range.Offset_X, item, range.Offset_Z);
                        Point3d to2 = new Point3d(range.Offset_X + range.Width, item, range.Offset_Z + range.Height);
                        mesh_line.Add(new Line(from, to1));
                        mesh_line.Add(new Line(from, to2));
                    }
                }

                if (sd.sub_z)
                {
                    List<double> sb_mesh_z = new List<double>();

                    double space = 0;

                    if (sd.Scheme == 0 && sd.Mode == 1) { space = sd.Z_Count_or_Distance; }
                    else { space = (sd.Z1 - sd.Z0) / sd.Z_Count_or_Distance; }

                    double sub_div_z = sd.Z0 + space;
                    while (sub_div_z < sd.Z1)
                    {
                        sb_mesh_z.Add(sub_div_z);
                        sub_div_z += space;
                    }

                    foreach (double item in sb_mesh_z)
                    {
                        Point3d from = new Point3d(range.Offset_X + range.Width, range.Offset_Y + range.Deepth, item);
                        Point3d to1 = new Point3d(range.Offset_X, range.Offset_Y + range.Deepth, item);
                        Point3d to2 = new Point3d(range.Offset_X + range.Width, range.Offset_Y, item);
                        mesh_line.Add(new Line(from, to1));
                        mesh_line.Add(new Line(from, to2));
                    }
                }
            }

            DA.SetData(0, analysis_mesh);
            DA.SetDataList(1, mesh_line);

            if (run) { analysis_mesh.Update(); }
        }

        private List<double>[] Divide_Range_by_Count(FD_Range range, double count)
        {
            double volume = range.Width * range.Deepth * range.Height / count;
            double unit_length = Math.Pow(volume, 1.0 / 3.0);
            double dis = Math.Round(unit_length, 2);
            return Divide_Range_by_Dis(range, dis);
        }

        private List<double>[] Divide_Range_by_Dis(FD_Range range, double dis)
        {
            List<double>[] Analysis_mesh = new List<double>[3];
            Analysis_mesh[0] = Divide_Axis_by_Dis(range.Offset_X, range.Width, dis);
            Analysis_mesh[1] = Divide_Axis_by_Dis(range.Offset_Y, range.Deepth, dis);
            Analysis_mesh[2] = Divide_Axis_by_Dis(range.Offset_Z, range.Height, dis);

            return Analysis_mesh;
        }

        private List<double> Divide_Axis_by_Dis(double min, double length, double dis)
        {
            List<double> temp_list = new List<double>();

            for (double i = 0; i < length; i += dis)
            {
                temp_list.Add(min + i);
            }

            temp_list.Add(min + length);

            return temp_list;
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.analysis_mesh;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{2b3d62e0-7a38-4a37-953d-1c7235671496}"); }
        }
    }
}