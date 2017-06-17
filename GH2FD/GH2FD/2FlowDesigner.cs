using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace GH2FD
{
    public class _2FlowDesigner : GH_Component
    {
        public _2FlowDesigner()
            : base("2FlowDesigner", "2FD",
                "Send the models and settings to FD and trigger the simulation",
                "FlowDesigner", Tools.sub_cate_03)
        { 
            Message = "Waiting...";
            ids = new GH_Structure<GH_String>();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddGenericParameter("Settings", "Set", "Settings of Analysis Range, External Wind, Analysis Mesh, etc.", GH_ParamAccess.list);
            //1
            pManager.AddTextParameter("IDs of Fixed Objects", "IFO", "IDs of the objects that will not be deleted when update model", GH_ParamAccess.tree, "");
            //2
            pManager.AddGenericParameter("Objects", "Odj", "Objects that will be updated", GH_ParamAccess.list);
            //3
            pManager.AddBooleanParameter("Write Model", "WM", "Write models and update the settings", GH_ParamAccess.item, false);
            //4
            pManager.AddBooleanParameter("Run Simulation", "RS", "Run Simulation", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //0
            pManager.AddTextParameter("IDs of objects", "ID", "IDs of objects which are updated", GH_ParamAccess.tree);
            //1
            pManager.AddTextParameter("Message", "M", "Message", GH_ParamAccess.item);
        }

        GH_Structure<GH_String> ids;

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Message = "Waiting...";

            bool write = false;
            DA.GetData(3, ref write);

            if (write)
            {
                List<FD_Group> FD_G_list = new List<FD_Group>();
                GH_Structure<GH_String> undelete = new GH_Structure<GH_String>();

                DA.GetDataList(2, FD_G_list);
                DA.GetDataTree(1, out undelete);

                undelete.Flatten();

                FD_Commander.Select();
                foreach (GH_String item in undelete.Branches[0]) { FD_Commander.Unselect(item.ToString()); }
                FD_Commander.Delete();

                ids = new GH_Structure<GH_String>();

                int counter = 0;

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

                List<FD_Setting> setlist = new List<FD_Setting>();
                DA.GetDataList(0, setlist);

                foreach (FD_Setting set in setlist) { set.Update(); }

                Message = "Ready";

                DA.SetData(1, Message);
            }

            DA.SetDataTree(0, ids);

            bool run = false;
            DA.GetData(4, ref run);

            if (run)
            {
                FD_Commander.Excute("menu analysis forward");
                Message = "Done";
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.FD;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{256bb5fe-ce06-449b-9aff-c0c284f8a20a}"); }
        }
    }
}