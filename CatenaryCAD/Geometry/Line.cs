using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Line : AbstractGeometry
    {
        public Line(Point3 p0, Point3 p1)
        {
            Vertices = new Point3[] { p0, p1 };
            Edges = new (int, int)[] { (0, 1) };
        }
    }
}
