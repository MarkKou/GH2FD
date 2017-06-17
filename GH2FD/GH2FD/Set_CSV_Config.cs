using System;
using System.Collections.Generic;
using FlowDesigner;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GH2FD
{
    public class Set_CSV_Config : GH_Component
    {
        public Set_CSV_Config()
            : base("CSV Output Configuration", "CSV OC",
                "Set CSV Output Configurations",
                "FlowDesigner", Tools.sub_cate_04)
        {
            CSV_Cycle _csvc = new CSV_Cycle(0);
            if (_csvc.Items.Count != 0) { analysis_type = 0; }
            else { analysis_type = 1; }
        }

        int analysis_type;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0
            pManager.AddIntegerParameter("Group", "G", "Group\r\n    0: Group to be analyzed\r\n    1: Surface group", GH_ParamAccess.item, 0);
            Param_Integer p0 = (Param_Integer)pManager[0];
            p0.AddNamedValue("Group to be analyzed", 0);
            p0.AddNamedValue("Surface group", 1);
            //1
            pManager.AddTextParameter("Path", "P", "File path of the CSV file", GH_ParamAccess.item, "");
            //2
            pManager.AddIntegerParameter("Output Type", "OT", "Output Type\r\n    0: Element unit\r\n    1: Average", GH_ParamAccess.item, 1);
            Param_Integer p2 = (Param_Integer)pManager[2];
            p2.AddNamedValue("Element unit", 0);
            p2.AddNamedValue("Average", 1);
            //3
            pManager.AddIntegerParameter("Panel Direction", "PD", "Panel Direction\r\n    0: (+) direction\r\n    1: (-) direction", GH_ParamAccess.item, 0);
            Param_Integer p3 = (Param_Integer)pManager[3];
            p3.AddNamedValue("(+) direction", 0);
            p3.AddNamedValue("(-) direction", 1);
            //4
            pManager.AddIntegerParameter("Output Unit", "OU", "Output Unit\r\n    0: Single file\r\n    1: Target each\r\n    2: Object each", GH_ParamAccess.item, 0);
            Param_Integer p4 = (Param_Integer)pManager[4];
            p4.AddNamedValue("Single file", 0);
            p4.AddNamedValue("Target each", 1);
            p4.AddNamedValue("Object each", 2);
            //5
            pManager.AddIntegerParameter("Output Range", "OR", "Output Range\r\n    0: All elements\r\n    1: Only elements including center", GH_ParamAccess.item, 0);
            Param_Integer p5 = (Param_Integer)pManager[5];
            p5.AddNamedValue("All elements", 0);
            p5.AddNamedValue("Only elements including center", 1);
            //6
            pManager.AddBooleanParameter("Update", "Up", "Update the settings", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "M", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Output Configurations", "O", "Output Configurations", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int type = 0;
            string path = "";
            int out_type = 0;
            int p_dire = 0;
            int unit = 0;
            int range = 0;

            DA.GetData(0, ref type);
            DA.GetData(1, ref path);
            DA.GetData(2, ref out_type);
            DA.GetData(3, ref p_dire);
            DA.GetData(4, ref unit);
            DA.GetData(5, ref range);

            CSV_Config csvoc = new CSV_Config(analysis_type, type);

            csvoc.Output_File.Path = path;
            csvoc.Output_Type.SelectedIndex = out_type;
            csvoc.Panel_Direction.SelectedIndex = p_dire;
            csvoc.Output_Unit.SelectedIndex = unit;
            csvoc.Output_Range.SelectedIndex = range;

            DA.SetData(1, csvoc);

            bool go = false;
            DA.GetData(6, ref go);

            if (go)
            {
                DA.SetDataList(0, csvoc.Update());
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.CSV_OC;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{63c24582-c173-40f2-b5f4-ca15c3c9b262}"); }
        }
    }
}