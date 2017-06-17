using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace GH2FD
{
    public class Export_CN : GH_Component
    {
        public Export_CN()
            : base("Export CN file for SolverExcuter", "CN",
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
            pManager.AddTextParameter("Folder Path", "FP", "The path of the folder to save the file", GH_ParamAccess.item);
            //4
            pManager.AddTextParameter("File Name", "FN", "The file name without extensional name", GH_ParamAccess.item);
            //5
            pManager.AddBooleanParameter("Write the File", "WF", "Write the File", GH_ParamAccess.item, false);
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
            DA.GetData(5, ref write);

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

                string folder = "";
                string file = "";

                DA.GetData(3, ref folder);
                DA.GetData(4, ref file);

                if (folder.Substring(folder.Length - 1, 1) != @"\")
                {
                    folder += @"\";
                }

                string saveas = "menu file saveas \"" + folder + file + ".fdp\" 0";
                string excn = "menu file export cn silent \"" + folder + file + ".cn\"";

                FD_Commander.Excute(saveas);
                FD_Commander.Excute(excn);

                Message = "Done";

                DA.SetData(1, Message);
            }

            DA.SetDataTree(0, ids);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Export_CN;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{ba61e7e6-4517-4d4d-92f8-2ea3b63ca370}"); }
        }
    }
}