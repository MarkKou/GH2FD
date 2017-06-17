using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace FlowDesigner
{
    public static class FD_Commander
    {
        public static dynamic fd = Activator.CreateInstance(Type.GetTypeFromProgID("AklModeler.CommandControl"));
        delegate int DelegateOnSolverEnd(long solvertype);

        public static bool jp_lan = false;

        private class Initial
        {
            public Initial()
            {
                fd.OnSolverEnd += new DelegateOnSolverEnd(fd_OnSolverEnd);
                jp_lan = (System.Globalization.CultureInfo.CurrentUICulture.ToString() == "ja-JP");
            }
        }

        static Initial init = new Initial();

        public static int counter = 0;

        public static string Message = "";
        //private string file_path;
        //public string Path { get { return file_path; } }

        public static void Open(string path)
        {
            //Excute("Open");
        }

        public static void Creat(string path, int pattern, int range, int layer, int grid_type, double width, double depth, double height, double offset_x, double offset_y, double offset_z, double grid_size)
        {
            //Excute("Creat");
        }

        public static void Select(string id = "all")
        {
            Excute("obj select " + id);
        }

        public static void Unselect(string id = "all")
        {
            Excute("obj unselect " + id);
        }

        public static void Delete()
        {
            Excute("obj delete");
        }

        public static void Delete_All()
        {
            Select();
            Delete();
        }

        public static object Excute(string command)
        {
            try
            {
                return fd.SendInstruction(command);
            }
            catch
            {
                return false;
            }
        }

        public static object Run()
        {
            //if (first_run)
            //{
            //    fd.OnSolverEnd += new DelegateOnSolverEnd(fd_OnSolverEnd);
            //    first_run = false;
            //}
            //Application.DoEvents();
            return Excute("menu analysis forward");
        }

        private static int fd_OnSolverEnd(long solvertype)
        {
            counter++;
            return 0;
        }
    }
}