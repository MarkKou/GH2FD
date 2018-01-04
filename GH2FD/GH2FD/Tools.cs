using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using FlowDesigner;
using Rhino.Geometry;
using Rhino;

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

        public static List<Surface> GenerateNormalArrow(List<Mesh> items)
        {
            RhinoDoc doc = RhinoDoc.ActiveDoc;
            string unit = doc.GetUnitSystemName(true, true, true, true);

            int mp = 1;

            if (unit == "m") { mp = 1; }
            else if (unit == "mm") { mp = 1000; }
            else if (unit == "cm") { mp = 100; }

            List<Surface> arrow_list = new List<Surface>();

            foreach (Mesh ori_mesh in items)
            {
                foreach (MeshFace m_face in ori_mesh.Faces)
                {
                    List<Point3d> ps = new List<Point3d>();

                    if (m_face.IsTriangle)
                    {
                        ps.Add(ori_mesh.Vertices[m_face.A]);
                        ps.Add(ori_mesh.Vertices[m_face.B]);
                        ps.Add(ori_mesh.Vertices[m_face.C]);
                    }
                    else if (m_face.IsQuad)
                    {
                        ps.Add(ori_mesh.Vertices[m_face.A]);
                        ps.Add(ori_mesh.Vertices[m_face.B]);
                        ps.Add(ori_mesh.Vertices[m_face.C]);
                        ps.Add(ori_mesh.Vertices[m_face.D]);
                    }
                    else { }

                    Vector3d[] tv = GetFaceNormal(ps);

                    Vector3d nv = tv[0];
                    Point3d cp = new Point3d(tv[1].X, tv[1].Y, tv[1].Z);

                    Plane bp = new Plane(cp, nv);
                    Circle bc = new Circle(bp, 0.1 * mp);
                    NurbsCurve _bc = bc.ToNurbsCurve();
                    Surface arrow = Surface.CreateExtrusionToPoint(_bc, new Point3d(cp.X + nv.X * mp, cp.Y + nv.Y * mp, cp.Z + nv.Z * mp));
                    arrow_list.Add(arrow);
                }
            }

            return arrow_list;
        }

        public static Vector3d[] GetFaceNormal(List<Point3d> ps)
        {
            Vector3d nv = new Vector3d(0, 0, 0);
            Vector3d cp = new Vector3d(0, 0, 0);

            for (int i = 0; i < ps.Count; i++)
            {
                int j = i + 1;
                if (j == ps.Count) j = 0;
                nv.X += (((ps[i].Z) + (ps[j].Z)) * ((ps[j].Y) - (ps[i].Y)));
                nv.Y += (((ps[i].X) + (ps[j].X)) * ((ps[j].Z) - (ps[i].Z)));
                nv.Z += (((ps[i].Y) + (ps[j].Y)) * ((ps[j].X) - (ps[i].X)));

                cp.X += ps[i].X;
                cp.Y += ps[i].Y;
                cp.Z += ps[i].Z;
            }

            nv.Unitize();

            Vector3d[] results = new Vector3d[2];
            results[0] = nv;
            results[1] = new Vector3d(cp.X / ps.Count, cp.Y / ps.Count, cp.Z / ps.Count);

            return results;
        }

        public static void Reverse_Meshs(List<Mesh> mesh_list)
        {
            foreach(Mesh mesh in mesh_list)
            {
                List<MeshFace> templist = new List<MeshFace>();
                foreach(MeshFace mf in mesh.Faces)
                {
                    templist.Add(mf);
                }

                mesh.Faces.Clear();

                foreach (MeshFace mf in templist)
                {
                    mesh.Faces.AddFace(Reverse_MeshFace(mf));
                }
            }
        }

        public static MeshFace Reverse_MeshFace(MeshFace mf)
        {
            if (mf.IsQuad)
            {
                return new MeshFace(mf.D, mf.C, mf.B, mf.A);
            }
            else
            {
                return new MeshFace(mf.C, mf.B, mf.A);
            }
        }

        //public static void updateIDs(GH_Structure<GH_String> _new_ids)
        //{
        //    old_ids = new_ids;
        //    new_ids = _new_ids;
        //}
    }
}
