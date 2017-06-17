using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Group
    {
        //Constructor
        //public FD_Group(List<FD_Object> _Menbers)
        //{
        //    Menbers = _Menbers;
        //}

        public FD_Group() { Members = new List<FD_Object>(); }

        public FD_Group(List<FD_Object> _Members)
        {
            Members = new List<FD_Object>();
            foreach (FD_Object item in _Members) { Members.Add(item); }
        }

        //public FD_Group(List<FD_Panel> _Members)
        //{
        //    Members = new List<FD_Object>();
        //    foreach (FD_Panel item in _Members) { Members.Add(item); }
        //}

        //Valiables & Properties
        public List<FD_Object> Members;

        protected virtual string Object_Type { get { return ""; } }
        protected virtual List<string> Property_string { get { return new List<string>(); } }

        //Methods
        protected void create()
        {
            if (Members.Count != 0)
            {
                FD_Commander.Excute("property select " + Object_Type);
                foreach (FD_Object item in Members) { item.Create(); }
            }
        }

        protected void set_pro()
        {
            if (Property_string.Count != 0)
            {
                foreach (string property_str in Property_string)
                { FD_Commander.Excute(property_str); }
            }
        }

        public void Create_Set()
        {
            FD_Commander.Unselect();
            create();
            set_pro();
            FD_Commander.Unselect();
        }

        public void Create()
        {
            FD_Commander.Unselect();
            create();
            FD_Commander.Unselect();
        }

        public void Set_Properties()
        {
            FD_Commander.Unselect();
            foreach (FD_Object item in Members) { item.Select(); }
            set_pro();
            FD_Commander.Unselect();
        }

        public void Delete()
        {
            foreach (FD_Object item in Members) { item.Delete(); }
        }
    }
}