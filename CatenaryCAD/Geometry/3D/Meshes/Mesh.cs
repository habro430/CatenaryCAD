using System;
using System.Collections.Generic;
using System.Globalization;

namespace CatenaryCAD.Geometry.Meshes
{
    [Serializable]
    public class Mesh : IMesh
    {
        public Face3D[] Faces { private set; get; }
        public Mesh(params Face3D[] faces) => Faces = faces;

        public static Mesh FromObj(string obj)
        {
            string[] lines = obj.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            List<Point3D> vertices = new List<Point3D>();
            List<Face3D> faces = new List<Face3D>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;

                string[] data = lines[i].Split(' ');

                bool success;
                switch (data[0])
                {
                    case "v": //парсер вершин
                        double x, y, z;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        vertices.Add(new Point3D(x, y, z));
                        break;

                    case "f": // парсер многоугольника
                        int vcount = data.Length - 1;
                        int[] edge = new int[vcount];

                        for (int ivert = 0; ivert < vcount; ivert++)
                        {
                            string[] parts = data[ivert + 1].Split('/');

                            int vindex;
                            success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                            if (!success) throw new ArgumentException("Невозможно преобразовать параметр как число int");

                            edge[ivert] = vindex;
                        }


                        Edge3D[] edges = new Edge3D[vcount];
                        for (int iedge = 0; iedge < vcount; iedge++)
                        {
                            if (iedge < vcount - 1)
                                edges[iedge] = new Edge3D(vertices[edge[iedge] - 1], vertices[edge[iedge + 1] - 1]);
                            else
                                edges[iedge] = new Edge3D(vertices[edge[iedge] - 1], vertices[edge[0] - 1]);
                        }

                        faces.Add(new Face3D(edges));

                        break;
                }
            }

            return new Mesh(faces.ToArray());
        }

        public IMesh TransformBy(in Matrix3D m)
        {
            int count = Faces.Length;

            Face3D[] new_faces = new Face3D[count];

            for (int i = 0; i < count; i++)
                new_faces[i] = Faces[i].TransformBy(m);

            return new Mesh(new_faces);
        }
    }
}
