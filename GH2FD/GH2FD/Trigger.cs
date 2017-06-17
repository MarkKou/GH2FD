//using System;
//using System.Collections.Generic;
//using FlowDesigner;
//using System.Windows.Forms;
//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using System.Threading;
//using System.ComponentModel;

//namespace GH2FD
//{
//    public class Trigger : GH_Component
//    {
//        delegate int DelegateOnSolverEnd(long solvertype);

//        public Trigger()
//            : base("Loop Trigger", "LT",
//                "Trigger the Loop when calculation is finished",
//                "FlowDesigner", Tools.sub_cate_05)
//        {
//            counter = 0;
//            limitation = 0;
//            reset = false;
//            goon = false;
//            bgw = new BackgroundWorker();
//            bgw.WorkerReportsProgress = true;
//            bgw.DoWork += new DoWorkEventHandler(EndlessChecking);
//            bgw.ProgressChanged += new ProgressChangedEventHandler(UpdateData);
//            bgw.RunWorkerAsync();
//        }

//        int counter;
//        int limitation;
//        bool reset;
//        bool goon;
//        BackgroundWorker bgw;

//        // On worker thread so do our thing!
//        void EndlessChecking(object sender, DoWorkEventArgs e)
//        {
//            int recoder = FD_Commander.counter;
//            while (true)
//            {
//                if (recoder != FD_Commander.counter && counter < limitation && goon && !reset)
//                {
//                    recoder = FD_Commander.counter;
//                    counter++;
//                    bgw.ReportProgress(counter);
//                }
//                System.Threading.Thread.Sleep(10000);
//            }
//        }
 

//        void UpdateData(object sender, ProgressChangedEventArgs e)
//        {
//            ExpireSolution(true);
//        }

//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddBooleanParameter("Reset", "R", "Reset the counter", GH_ParamAccess.item, false);
//            pManager.AddIntegerParameter("Loops", "L", "Counts of the Loops", GH_ParamAccess.item, 16);
//            pManager.AddBooleanParameter("Go on", "G", "Continue the Loop", GH_ParamAccess.item, false);
//        }

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddTextParameter("IDs", "I", "", GH_ParamAccess.tree);
//            pManager.AddIntegerParameter("Counter", "C", "", GH_ParamAccess.item);
//            pManager.AddBooleanParameter("Trigger", "T", "", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            DA.GetData(0, ref reset);
//            DA.GetData(1, ref limitation);
//            DA.GetData(2, ref goon);
//            if (reset) { counter = 0; }
//            //DA.SetDataTree(0, Tools.ids_in_loop);
//            DA.SetData(1, counter);
//            DA.SetData(2, (counter != limitation && goon && !reset));
//        }


//        protected override System.Drawing.Bitmap Icon
//        {
//            get
//            {
//                return Properties.Resources.Loop_Trigger;
//            }
//        }

//        public override Guid ComponentGuid
//        {
//            get { return new Guid("{2f2531c0-d4af-4486-b808-1a9caccec03a}"); }
//        }
//    }
//}