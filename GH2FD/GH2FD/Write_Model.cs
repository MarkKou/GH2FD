using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace GH2FD
{
    public class Write_Model : GH_Component
    {
        public Write_Model()
            : base("Write Model", "WM", "Write the Models into FlowDesigner", "FlowDesigner", Tools.sub_cate_05)
        {
            Message = "Waiting...";
            ids = new GH_Structure<GH_String>();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Model", "M", "Model to send to FD", GH_ParamAccess.list);
            //1
            pManager.AddTextParameter("IDs of undelete objects", "IU", "ID of undelete objects", GH_ParamAccess.tree, "Default");
            //2
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("Message", "M", "Messages", GH_ParamAccess.item);
            //1
            pManager.AddTextParameter("IDs of objects", "ID", "IDs od newly created objects", GH_ParamAccess.tree);
        }

        GH_Structure<GH_String> ids;

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;
            DA.GetData(2, ref run);

            if(run)
            {
                List<FD_Group> FD_G_list = new List<FD_Group>();
                GH_Structure<GH_String> undelete = new GH_Structure<GH_String>();

                DA.GetDataList(0, FD_G_list);
                DA.GetDataTree(1, out undelete);

                undelete.Flatten();

                FD_Commander.Select();
                foreach (GH_String item in undelete.Branches[0]) { FD_Commander.Unselect(item.ToString()); }
                FD_Commander.Delete();

                int counter = 0;

                ids = new GH_Structure<GH_String>();

                foreach (FD_Group item in FD_G_list)
                {
                    GH_Path path = new GH_Path(counter);

                    item.Create_Set();
                    foreach (FD_Object sub_item in item.Members)
                    {
                        ids.Append(new GH_String(sub_item.ID), path);
                    }

                    counter++;
                }

                Message = "Done";

                DA.SetData(0, Message);
            }

            DA.SetDataTree(1, ids);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.to_FD;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{d6a5a9e8-96ba-429f-95cf-9dcf470d8eb6}"); }
        }
    }
}