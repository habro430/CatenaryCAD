using CatenaryCAD.Geometry.Core;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CatenaryCAD.Geometry
{

    [Serializable]
    public sealed class Mesh : AbstractGeometry
    {
        public Mesh(Point3[] vertices, (int, int)[] edges)
        {
            Vertices = vertices;
            Edges = edges;
        }

        public static Mesh FromObj(string obj)
        {
            string[] lines = obj.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            List<Point3> vertices = new List<Point3>();
            List<(int, int)> edges = new List<(int, int)>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;

                String[] data = lines[i].Split(' ');

                bool success;
                switch (data[0])
                {
                    case "v":
                        double x, y, z;
                        
                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        vertices.Add(new Point3(x,y,z));
                        break;

                    case "f":
                        int vcount = data.Count() - 1;
                        int[] edge = new int[vcount];

                        for (int ivert = 0; ivert < vcount; ivert++)
                        {
                            string[] parts = data[ivert + 1].Split('/');

                            int vindex;
                            success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                            if (!success) throw new ArgumentException("Невозможно преобразовать параметр как число int");

                            edge[ivert] = vindex;
                        }

                        
                        (int, int)[] tmp_edges = new (int, int)[vcount];
                        for (int iedge = 0; iedge < vcount; iedge++)
                        {
                            if (iedge < vcount - 1)
                                tmp_edges[iedge] = (edge[iedge] - 1, edge[iedge + 1] - 1);
                            else
                                tmp_edges[iedge] = (edge[iedge] - 1, edge[0] - 1);
                        }

                        edges.AddRange(tmp_edges);

                        break;
                }
            }

            return new Mesh(vertices.ToArray(), edges.ToArray());
        }
    }
}
