using System;
using System.Collections.Generic;
using FlowDesigner;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Threading;
using System.ComponentModel;

namespace GH2FD
{
    public class Output_CSV_Loop : GH_Component
    {
        delegate int DelegateOnSolverEnd(long solvertype);

        public Output_CSV_Loop()
            : base("CSV Loop", "Loop",
                "Write the results to a CSV file then Trigger the Loop when calculation is finished",
                "FlowDesigner", Tools.sub_cate_05)
        {
            counter = 0;
            limitation = 0;
            reset = false;
            goon = false;
            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += new DoWorkEventHandler(EndlessChecking);
            bgw.ProgressChanged += new ProgressChangedEventHandler(UpdateData);
            bgw.RunWorkerAsync();

            Message = "Waiting...";
        }

        int counter;
        int limitation;
        bool reset;
        bool goon;
        BackgroundWorker bgw;

        void EndlessChecking(object sender, DoWorkEventArgs e)
        {
            int recoder = FD_Commander.counter;
            while (true)
            {
                if (recoder != FD_Commander.counter && counter < limitation && goon && !reset)
                {
                    recoder = FD_Commander.counter;
                    counter++;
                    bgw.ReportProgress(counter);
                }
                System.Threading.Thread.Sleep(10000);
            }
        }
 

        void UpdateData(object sender, ProgressChangedEventArgs e)
        {
            ExpireSolution(true);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Seetings", "Set", "Settings of 'Target', 'Cycle' & 'Output Configuration", GH_ParamAccess.list);
            pManager.AddTextParameter("IDs of the output objets", "IDs", "IDs of the output objets, if blank, the component will read from a static valiable in the memory written by CSV IDs", GH_ParamAccess.list, "");
            pManager.AddBooleanParameter("Reset", "Re", "Reset the counter", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("Loops", "Lo", "Count of the Loops", GH_ParamAccess.item, 16);
            pManager.AddBooleanParameter("Go on", "Go", "Trigger the Loop", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "M", "Messages from FD", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Counter", "C", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Trigger", "T", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.GetData(2, ref reset);
            DA.GetData(3, ref limitation);
            DA.GetData(4, ref goon);

            if (reset)
            {
                counter = 0;
                Message = "Waiting...";
            }

            if (counter != 0 && goon && !reset)
            {
                List<FD_Setting> settings = new List<FD_Setting>();
                DA.GetDataList(0, settings);

                List<string> messages = new List<string>();

                foreach (FD_Setting set in settings)
                {
                    messages.AddRange(set.Update());
                }

                CSV_Output csv_out = new CSV_Output();

                List<string> ids = new List<string>();
                DA.GetDataList(1, ids);

                if (ids.Count != 0)
                {
                    csv_out.IDs = ids;
                }
                else
                {
                    csv_out.IDs = Tools.ids_in_loop;
                }
                messages.Add(csv_out.Output());

                DA.SetDataList(0, messages);
            }

            bool trigger = counter != limitation && goon && !reset;

            if (trigger) { Message = "Looping"; }
            if (counter == limitation) { Message = "Done"; }

            DA.SetData(1, counter);
            DA.SetData(2, trigger);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.CSV_Loop;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{241dfcf6-4634-4867-b616-eafc3df8253e}"); }
        }
    }
}