using System;
using System.Collections.Generic;
using System.Globalization;

namespace CatenaryCAD.Geometry.Meshes
{
    [Serializable]
    public class Mesh : IMesh
    {
        public Point3D[] Vertices { get; private set; }
        public Vector3D[] Normals { get; private set; }
        public int[][] Indices { get; private set; }

        public Mesh(Point3D[] vertices, Vector3D[] normals, int[][] indices)
        { 
            Vertices = vertices;
            Normals = normals;
            Indices = indices;
        }

        public IMesh TransformBy(in Matrix3D m)
        {
            throw new NotImplementedException();
        }        
        
        //public IMesh TransformBy(in Matrix3D m)
        //{
        //    //int count = Faces.Length;

        //    //Face3D[] new_faces = new Face3D[count];

        //    //for (int i = 0; i < count; i++)
        //    //    new_faces[i] = Faces[i].TransformBy(m);

        //    //return new Mesh(new_faces);
        //}


        public static Mesh FromObj(string obj)
        {
            string[] lines = obj.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var vertices = new List<Point3D>();
            var normals = new List<Vector3D>();
            var indices = new List<int[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;

                string[] data = lines[i].Split(' ');

                bool success;
                switch (data[0])
                {
                    case "v": //парсер вершин
                        double xv, yv, zv;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out xv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out yv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out zv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        vertices.Add(new Point3D(xv, yv, zv));

                        break;

                    case "vn": //парсер нормалей
                        double xn, yn, zn;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out xn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out yn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out zn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        normals.Add(new Vector3D(xn, yn, zn));
                        break;

                    case "f": // парсер многоугольника
                        int vcount = data.Length - 1;
                        int[] vert = new int[vcount];

                        for (int ivert = 0; ivert < vcount; ivert++)
                        {
                            string[] parts = data[ivert + 1].Split('/');

                            int vindex;
                            success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                            if (!success) throw new ArgumentException("Невозможно преобразовать параметр как число int");

                            vert[ivert] = vindex - 1;
                        }

                        indices.Add(vert);
                        break;
                }
            }


            return new Mesh(vertices.ToArray(), normals.ToArray(), indices.ToArray());
        }



    }
}
