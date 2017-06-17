using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_GeoMesh : FD_Object
    {
       public List<FD_Vertex> Vertices;
       public List<FD_Face> Faces;

        public FD_GeoMesh()
        {
            Vertices = new List<FD_Vertex>();
            Faces = new List<FD_Face>();
        }

        public FD_GeoMesh(List<FD_Vertex> vertices, List<FD_Face> faces)
        {
            Vertices = new List<FD_Vertex>();

            foreach(FD_Vertex v in vertices)
            {
                Vertices.Add(v);
            }

            Faces = new List<FD_Face>();

            foreach (FD_Face f in faces)
            {
                Faces.Add(f);
            }
        }

        protected override string Create_string
        {
            get
            {
                string create_string = "obj createbystrip ";
                create_string += Vertices.Count.ToString() + " ";

                foreach(FD_Vertex v in Vertices)
                {
                    create_string += v.CreateString;
                }

                create_string += Faces.Count.ToString() + " ";

                foreach(FD_Face f in Faces)
                {
                    create_string += f.CreateString;
                }

                return create_string;
            }
        }
    }

    public class FD_Face
    {
        public int A;
        public int B;
        public int C;

        public FD_Face(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public string CreateString
        {
            get
            {
                return A.ToString() + "," + B.ToString() + "," + C.ToString() + " ";
            }
        }
    }
}
