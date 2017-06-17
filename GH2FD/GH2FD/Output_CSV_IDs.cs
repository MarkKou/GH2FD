using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GH2FD
{
    public class Output_CSV_IDs : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Output_CSV_IDs class.
        /// </summary>
        public Output_CSV_IDs()
            : base("Output CSV IDs", "IDs",
                "Send the IDs of output objects to a static valiable in the menory, which will be used by the CSV Loop component",
                "FlowDesigner", Tools.sub_cate_05)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("IDs", "IDs", "", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) { }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Integer> id_tree = new GH_Structure<GH_Integer>();
            DA.GetDataTree(0, out id_tree);

            id_tree.Flatten();

            List<string> ids = new List<string>();

            foreach(GH_Integer id in id_tree.Branches[0])
            {
                ids.Add(id.ToString());
            }

            Tools.ids_in_loop = ids;
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.CSV_Loop_IDs;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{0d3ab5df-d8f6-492e-b8ea-3f5a6c2880d9}"); }
        }
    }
}