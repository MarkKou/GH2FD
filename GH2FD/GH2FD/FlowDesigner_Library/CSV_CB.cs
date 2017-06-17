using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowDesigner
{
    public class CSV_CB : CheckBox
    {
        int analysistype;
        int grouptype;
        int groupindex;
        int index;

        public bool OnOff
        {
            set { Checked = value; }
            get { return Checked; }
        }

        public string State
        {
            get
            {
                if (Checked) { return "on"; }
                else { return "off"; }
            }
        }

        public string Analysis_Type
        {
            get
            {
                if (analysistype == 0) { return "fwd"; }
                else if (analysistype == 1) { return "opt"; }
                else { return ""; }
            }
        }

        public string Group_Type
        {
            get
            {
                if (grouptype == 0) { return "target"; }
                else if (grouptype == 1) { return "outputcycle"; }
                else { return ""; }
            }
        }

        public string Group_Index
        {
            get
            {
                if (grouptype == 0) { return groupindex.ToString(); }
                else if (grouptype == 1) { return ""; }
                else { return ""; }
            }
        }

        public string Set_string
        {
            get
            {
                return "plugin ctrlgraplug csv set " + Analysis_Type + " " + Group_Type + " " + Group_Index + " " + index.ToString() + " " + State;
            }
        }

        public CSV_CB(int _analysistype, int _grouptype, int _groupindex, int _index, string _name)
            : base()
        {
            analysistype = _analysistype;
            grouptype = _grouptype;
            groupindex = _groupindex;
            index = _index;
            Name = _name;
            Text = _name;
            Checked = false;
        }

        public void Update()
        {
            FD_Commander.Excute(Set_string);
        }
    }
}