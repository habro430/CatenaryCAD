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
        public Mesh(Point[]points, (int, int)[] edges)
        {
            Points = points;
            Edges = edges;
        }

        public static Mesh GenerateFromObj(string model)
        {

            throw new NotImplementedException();
        }
    }
}
