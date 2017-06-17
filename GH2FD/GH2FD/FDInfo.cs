using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GH2FD
{
    public class FDInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "FD";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("294f00a2-0439-4fc9-a07f-eee7f6bbfcfc");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
