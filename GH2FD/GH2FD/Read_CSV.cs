using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.IO;

namespace GH2FD
{
    public class Read_CSV : GH_Component
    {
        public string[] columns;
        public List<int> c_picked;
        public bool cp_set;
        private string ori_path;

        GH_InputParamManager pma;

        public Read_CSV()
            : base("Read CSV", "CSV→",
                "Read the result CSV file",
                "FlowDesigner", Tools.sub_cate_04)
        {
            Reset();
            ori_path = "";
        }

        private void Reset()
        {
            columns = new string[0];
            c_picked = new List<int>();
            cp_set = false;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "", GH_ParamAccess.item);
            pma = pManager;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Cycle", "C", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Objects", "O", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Object Type", "T", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Column Name", "C", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Results", "R", "", GH_ParamAccess.tree);
        }

        //public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        //{
        //    base.AppendAdditionalMenuItems(menu);
        //    Menu_AppendGenericMenuItem(menu, "Column Picker", SettingOpen);
        //}

        //public void SettingOpen(Object sender, EventArgs e)
        //{
        //    ColumnPicker cp = new ColumnPicker(this);
        //    cp.Show();
        //}

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string path = "";

            if (!DA.GetData(0, ref path)) { return; }

            FileStream fs = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("shift_jis"));

            List<string[]> table = new List<string[]>();

            string strLine = "";

            while ((strLine = sr.ReadLine()) != null)
            {
                table.Add(strLine.Split(','));
            }

            if (table[1][0] != "cycle") { table.RemoveAt(0); }

            int head = 0;
            if (table[0][4] != "Element number" && table[0][4] != "要素番号") { head = 4; }
            else if (table[0][7] == "Element center coordinate" || table[0][7] == "要素中心座標値") { head = 7; }
            else { head = 19; }

            int counter = 0;

            List<string> items = new List<string>();
            string current_group = "";

            for (int c = head; c < table[0].Length - 1; c++)
            {
                if (table[0][c].Length != 0) { current_group = table[0][c]; }

                items.Add(counter.ToString() + ": " + current_group + "." + table[1][c]);
                counter++;
            }

            GH_Structure<GH_String> res_tree = new GH_Structure<GH_String>();

            List<string> cycles = new List<string>();
            List<string> objects = new List<string>();
            List<string> types = new List<string>();

            string current_cycle = "";
            string current_object = "";
            int object_counter = -1;

            for (int r = 2; r < table.Count; r++)
            {
                if (current_cycle == table[r][0])
                {
                    if (current_object == table[r][2])
                    {
                        GH_Path data_path = new GH_Path(new int[] { Convert.ToInt32(current_cycle), object_counter, counter });
                        for (int c = head; c < table[r].Length - 1; c++)
                        {
                            res_tree.Append(new GH_String(table[r][c]), data_path);
                        }
                        counter++;
                    }
                    else
                    {
                        object_counter++;
                        current_object = table[r][2];
                        if (current_cycle == "0")
                        {
                            objects.Add(object_counter.ToString() + ": " + current_object);
                            types.Add(object_counter.ToString() + ": " + table[r][3]);
                        }
                        counter = 0;
                        r--;
                    }

                    //objects.Add(object_counter + ": " + table[2][2]);
                    //types.Add(object_counter + ": " + table[2][3]);
                }
                else
                {
                    current_cycle = table[r][0];
                    cycles.Add(current_cycle + ": " + table[r][1]);
                    object_counter = -1;
                    r--;
                }
            }

                //foreach (string[] row in table)
                //{
                //    GH_Path ghpath = new GH_Path(counter);
                //    foreach(string item in row)
                //    {
                //        res_tree.Append(new GH_String(item), ghpath);
                //    }
                //    counter++;
                //}

            DA.SetDataList(0, cycles);
            DA.SetDataList(1, objects);
            DA.SetDataList(2, types);
            DA.SetDataList(3, items);
            DA.SetDataTree(4, res_tree);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Read_CSV;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{5ef37723-adb7-481a-9d39-6e4c06da7215}"); }
        }
    }
}