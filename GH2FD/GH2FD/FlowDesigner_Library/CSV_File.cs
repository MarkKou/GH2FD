using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class CSV_File
    {
        int analysistype;
        int group_type;

        public string AT_String
        {
            get
            {
                if (analysistype == 0) { return "fwd"; }
                else if (analysistype == 1) { return "opt"; }
                else { return ""; }
            }
        }

        public string GT_String
        {
            get
            {
                if (group_type == 0) { return "atarget"; }
                else if (group_type == 1) { return "starget"; }
                else if (group_type == 2) { return "ctarget"; }
                else { return ""; }
            }
        }

        string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Set_string
        {
            get
            {
                return "plugin ctrlgraplug csv set " + AT_String + " " + GT_String + " outputfile " + Path.Replace(@"\", @"\\");
            }
        }

        public CSV_File(int _analysistype, int _type)
        {
            analysistype = _analysistype;
            group_type = _type;
            path = "";
        }

        public CSV_File(int _analysistype, int _type, string _path)
        {
            analysistype = _analysistype;
            group_type = _type;
            path = _path;
        }

        public void Update()
        {
            FD_Commander.Excute(Set_string);
        }
    }
}