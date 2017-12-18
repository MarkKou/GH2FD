using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using FlowDesigner;
using Rhino.Geometry;

namespace GH2FD
{
    static class Tools
    {
        public static string sub_cate_00 = "00 Before Modelling";
        public static string sub_cate_01 = "01 Panel Objects";
        public static string sub_cate_02 = "02 Cube Objects";
        public static string sub_cate_03 = "03 Before Simulation";
        public static string sub_cate_04 = "04 After Simulation";
        public static string sub_cate_05 = "05 Tools";
        public static string sub_cate_06 = "06";

        public static string c_o_n = "FlowDesigner Objects";
        public static string c_o_s = "Obj";
        public static string c_o_d = "Objects to send to FlowDesigner";

        public static List<string> MultiLine2List(string ori_str)
        {
            List<string> converted = new List<string>();

            string temp = "";

            foreach (char item in ori_str.ToCharArray())
            {
                if (item == 10) { }
                else if (item == 13)
                {
                    converted.Add(temp);
                    temp = "";
                }
                else
                {
                    temp += item;
                }
            }

            return converted;
        }

        public static string Array2MultiLine(string[] ori_array)
        {
            string conbined = "";

            foreach(string item in ori_array)
            {
                conbined += item + Environment.NewLine;
            }

            return conbined;
        }

        public static string List2MultiLine(List<string> ori_list)
        {
            string conbined = "";

            foreach (string item in ori_list)
            {
                conbined += item + Environment.NewLine;
            }

            return conbined;
        }

        public static List<string> ids_in_loop = new List<string>();

        public static List<FD_Object> GenerateCubeList(List<dynamic> items)
        {
            List<FD_Object> cube_list = new List<FD_Object>();

            foreach (dynamic ori_box in items)
            {
                if (ori_box.Value is Box)
                {
                    Point3d[] temp_list = ori_box.Value.GetCorners();

                    FD_Vertex vertex1 = new FD_Vertex(temp_list[3].X, temp_list[3].Y, temp_list[3].Z);
                    FD_Vertex vertex2 = new FD_Vertex(temp_list[2].X, temp_list[2].Y, temp_list[2].Z);
                    FD_Vertex vertex3 = new FD_Vertex(temp_list[1].X, temp_list[1].Y, temp_list[1].Z);
                    FD_Vertex vertex4 = new FD_Vertex(temp_list[0].X, temp_list[0].Y, temp_list[0].Z);
                    cube_list.Add(new FD_Cube(vertex1, vertex2, vertex3, vertex4, ori_box.Value.Z.Length));
                }
                else if (ori_box.Value is FD_Cube)
                {
                    cube_list.Add(ori_box.Value);
                }
                else if (ori_box.Value is Mesh || ori_box.Value is Brep)
                {
                    Mesh ori_mesh;

                    if(ori_box.Value is Brep)
                    {
                        Mesh[] converted_list = Mesh.CreateFromBrep(ori_box.Value, new MeshingParameters());
                        ori_mesh = new Mesh();

                        foreach(Mesh m in converted_list)
                        {
                            ori_mesh.Append(m);
                        }
                    }
                    else { ori_mesh = ori_box.Value; }

                    FD_GeoMesh mo = new FD_GeoMesh();

                    foreach(Point3d v in ori_mesh.Vertices)
                    {
                        mo.Vertices.Add(new FD_Vertex(v.X, v.Y, v.Z));
                    }

                    foreach(MeshFace f in ori_mesh.Faces)
                    {
                        if (f.IsTriangle)
                        {
                            mo.Faces.Add(new FD_Face(f.A, f.B, f.C));
                        }
                        else if(f.IsQuad)
                        {
                            mo.Faces.Add(new FD_Face(f.A, f.B, f.C));
                            mo.Faces.Add(new FD_Face(f.C, f.D, f.A));
                        }
                    }

                    cube_list.Add(mo);
                }
            }

            return cube_list;
        }

        public static List<FD_Object> GeneratePanelList(List<Mesh> items)
        {
            List<FD_Object> panel_list = new List<FD_Object>();

            foreach (Mesh ori_mesh in items)
            {
                foreach (MeshFace m_face in ori_mesh.Faces)
                {
                    FD_Vertex temp_v1 = new FD_Vertex(ori_mesh.Vertices[m_face.A].X, ori_mesh.Vertices[m_face.A].Y, ori_mesh.Vertices[m_face.A].Z);
                    FD_Vertex temp_v2 = new FD_Vertex(ori_mesh.Vertices[m_face.B].X, ori_mesh.Vertices[m_face.B].Y, ori_mesh.Vertices[m_face.B].Z);
                    FD_Vertex temp_v3 = new FD_Vertex(ori_mesh.Vertices[m_face.C].X, ori_mesh.Vertices[m_face.C].Y, ori_mesh.Vertices[m_face.C].Z);

                    if (m_face.IsTriangle)
                    {
                        panel_list.Add(new FD_Panel(temp_v3, temp_v2, temp_v1));
                    }
                    else if (m_face.IsQuad)
                    {
                        FD_Vertex temp_v4 = new FD_Vertex(ori_mesh.Vertices[m_face.D].X, ori_mesh.Vertices[m_face.D].Y, ori_mesh.Vertices[m_face.D].Z);
                        panel_list.Add(new FD_Panel(temp_v4, temp_v3, temp_v2, temp_v1));
                    }
                    else { }
                }
            }

            return panel_list;
        }

        //public static void updateIDs(GH_Structure<GH_String> _new_ids)
        //{
        //    old_ids = new_ids;
        //    new_ids = _new_ids;
        //}
    }
}
