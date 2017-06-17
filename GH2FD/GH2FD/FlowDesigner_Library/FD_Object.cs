using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Object
    {
        //Constructor
        public FD_Object() { }

        //Properties
        protected string id;
        public string ID { get { return id; } }
        protected virtual string Create_string { get { return ""; } }

        //Methods
        public void Create()
        {
            id = FD_Commander.Excute(Create_string).ToString();
        }

        public void Select() { FD_Commander.Select(id); }

        public void Delete()
        {
            FD_Commander.Excute("obj select " + id);
            FD_Commander.Excute("obj delete");
        }
    }
}