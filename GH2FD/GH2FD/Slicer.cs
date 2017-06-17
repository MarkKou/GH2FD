using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;

namespace GH2FD
{
    public class Slicer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Slicer class.
        /// </summary>
        public Slicer()
            : base("Slicer", "Slicer",
                "Pick up the results wihin a certain section",
                "FlowDesigner", Tools.sub_cate_05)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("Results", "Re", "Data from 'Results' output of 'Read CSV' component", GH_ParamAccess.tree);
            //1
            pManager.AddIntegerParameter("XYZ", "XYZ", "The plane be displayed\r\n    0: X (YZ plane)\r\n    1: Y (XZ plane)\r\n    2: Z (XY plane)", GH_ParamAccess.item, 0);
            Grasshopper.Kernel.Parameters.Param_Integer plane_list = pManager[1] as Grasshopper.Kernel.Parameters.Param_Integer;
            plane_list.AddNamedValue("X (YZ plane)", 0);
            plane_list.AddNamedValue("Y (XZ plane)", 1);
            plane_list.AddNamedValue("Z (XY plane)", 2);
            //2
            pManager.AddTextParameter("Absolute Value", "AV", "The absolute value of the location", GH_ParamAccess.item, "NA");
            //3
            pManager.AddNumberParameter("Relative Value", "RV", "The relative value of the location, invalid when AV is used", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddPointParameter("Points", "Po", "Base points of the elements in the slides", GH_ParamAccess.tree);
            //1
            pManager.AddPathParameter("Paths", "Pa", "Paths selected elements", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_String> results = new GH_Structure<GH_String>();
            DA.GetDataTree(0, out results);

            int plane = 0;
            DA.GetData(1, ref plane);

            List<double> coordinates = new List<double>();

            int object_count = 0;

            char cycle = results.Paths[0].ToString().ToCharArray()[1];

            foreach (GH_Path path in results.Paths)
            {
                if (path.ToString().ToCharArray()[1] == cycle)
                {
                    object_count++;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < object_count; i++)
            {
                coordinates.Add(Convert.ToDouble(results.Branches[i][plane].ToString()));
            }

            double target_value = 0;

            string av = "";
            DA.GetData(2, ref av);
            if (av != "NA") { target_value = Convert.ToDouble(av); }
            else
            {
                List<double> temp = new List<double>();
                temp.AddRange(coordinates);
                temp.Sort();
                double min = temp[0];
                double max = temp[temp.Count - 1];

                double rv = 0;
                DA.GetData(3, ref rv);

                if (rv > 1) { rv = 1; }
                if (rv < 0) { rv = 0; }

                target_value = min + (max - min) * rv;
            }

            List<int> indices = Indices(coordinates, target_value);

            GH_Structure<GH_Point> points = new GH_Structure<GH_Point>();

            foreach (int index in indices)
            {
                double x = Convert.ToDouble(results.Branches[index][0].ToString());
                double y = Convert.ToDouble(results.Branches[index][1].ToString());
                double z = Convert.ToDouble(results.Branches[index][2].ToString());
                points.Append(new GH_Point(new Point3d(x, y, z)), results.Paths[index]);
            }

            double cycle_count = results.Branches.Count / object_count;

            List<GH_Path> paths = new List<GH_Path>();

            for (int c = 0; c < cycle_count; c++)
            {
                foreach (int index in indices)
                {
                    paths.Add(new GH_Path(results.Paths[object_count * c + index]));
                }
            }

            DA.SetDataTree(0, points);
            DA.SetDataList(1, paths);
        }

        private List<int> Indices(List<double> _coors, double target)
        {
            List<int> indices = new List<int>();
            List<double> distance = new List<double>();

            foreach (double coor in _coors)
            {
                distance.Add(Math.Abs(coor - target));
            }

            double min = distance[0];

            for (int i = 0; i < distance.Count; i++)
            {
                if (distance[i] < min - 0.0001)
                {
                    indices.Clear();
                    indices.Add(i);
                    min = distance[i];
                }
                else if(distance[i]<min+0.0001)
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Slice_View;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{86c2200a-b5d3-4a98-bb4b-5b74770ebe2a}"); }
        }
    }
}