using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class CSV_Config : FD_Setting
    {
        public CSV_File Output_File;
        public CSV_DL Output_Type;
        public CSV_DL Panel_Direction;
        public CSV_DL Output_Unit;
        public CSV_DL Output_Range;

        public CSV_Config(int _analysistype, int _type)
            : base()
        {
            Output_File = new CSV_File(_analysistype, _type);
            Output_Type = new CSV_DL(_analysistype, _type, 0);
            Panel_Direction = new CSV_DL(_analysistype, _type, 1);
            Output_Unit = new CSV_DL(_analysistype, _type, 2);
            Output_Range = new CSV_DL(_analysistype, _type, 3);
        }

        protected override List<string> Update_Strings
        {
            get
            {
                List<string> upstr = new List<string>();
                upstr.Add(Output_File.Set_string);
                upstr.Add(Output_Type.Set_string);
                upstr.Add(Panel_Direction.Set_string);
                upstr.Add(Output_Unit.Set_string);
                upstr.Add(Output_Range.Set_string);
                return upstr;
            }
        }
    }
}