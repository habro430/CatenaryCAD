using CatenaryCAD.Geometry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD.Geometry
{

    [Serializable]
    public sealed class Mesh:AbstractGeometry
    {
        public Mesh(Point[] vertices, (int, int)[] edges)
        {
            Vertices = vertices;
            Edges = edges;
        }

        public static Mesh FromObj(string model)
        {

            throw new NotImplementedException();
        }
    }
}
