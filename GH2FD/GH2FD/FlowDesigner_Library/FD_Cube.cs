using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesigner
{
    public class FD_Cube : FD_Object
    {
        public List<FD_Vertex> vertices;
        private double height;

        public bool IsTriangle
        {
            get { return vertices.Count == 3; }
        }

        public bool IsQuad
        {
            get { return vertices.Count == 4; }
        }

        public FD_Cube(FD_Vertex _vertex1, FD_Vertex _vertex2, FD_Vertex _vertex3, double _height)
            : base()
        {
            vertices = new List<FD_Vertex>();

            vertices.Add(_vertex1);
            vertices.Add(_vertex2);
            vertices.Add(_vertex3);

            height = _height;
        }

        public FD_Cube(FD_Vertex _vertex1, FD_Vertex _vertex2, FD_Vertex _vertex3, FD_Vertex _vertex4, double _height)
            : base()
        {
            vertices = new List<FD_Vertex>();

            vertices.Add(_vertex1);
            vertices.Add(_vertex2);
            vertices.Add(_vertex3);
            vertices.Add(_vertex4);

            height = _height;
        }

        public FD_Cube(List<FD_Vertex> _vertices, double _height)
            : base()
        {
            vertices = new List<FD_Vertex>();

            foreach (FD_Vertex vertex in _vertices)
            {
                vertices.Add(vertex);
            }

            height = _height;
        }

        protected override string Create_string
        {
            get
            {
                string create_string = "obj create " + vertices.Count.ToString() + " ";

                foreach (FD_Vertex vertex in vertices)
                {
                    create_string += vertex.CreateString;
                }

                create_string += height.ToString();

                return create_string;
            }
        }
    }
}