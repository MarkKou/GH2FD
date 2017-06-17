using System;
using System.Collections.Generic;

namespace FlowDesigner
{
    public class FD_Panel : FD_Object
    {
        public List<FD_Vertex> vertices;

        public bool IsTriangle
        {
            get { return vertices.Count == 3; }
        }

        public bool IsQuad
        {
            get { return vertices.Count == 4; }
        }

        public FD_Panel(FD_Vertex _vertex1, FD_Vertex _vertex2, FD_Vertex _vertex3)
            : base()
        {
            vertices = new List<FD_Vertex>();

            vertices.Add(_vertex1);
            vertices.Add(_vertex2);
            vertices.Add(_vertex3);
        }

        public FD_Panel(FD_Vertex _vertex1, FD_Vertex _vertex2, FD_Vertex _vertex3, FD_Vertex _vertex4)
            : base()
        {
            vertices = new List<FD_Vertex>();

            vertices.Add(_vertex1);
            vertices.Add(_vertex2);
            vertices.Add(_vertex3);
            vertices.Add(_vertex4);
        }

        public FD_Panel(List<FD_Vertex> _vertices)
            : base()
        {
            vertices = new List<FD_Vertex>();

            foreach (FD_Vertex vertex in _vertices)
            {
                vertices.Add(vertex);
            }
        }

        protected override string Create_string
        {
            get
            {
                string create_string = "obj create " + vertices.Count.ToString() + " ";

                foreach(FD_Vertex vertex in vertices)
                {
                    create_string += vertex.CreateString;
                }

                return create_string;
            }
        }
    }
}