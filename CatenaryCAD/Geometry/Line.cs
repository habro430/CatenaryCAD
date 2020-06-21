using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Line : AbstractGeometry
    {
        public Line(Point p0, Point p1)
        {
            Vertices = new Point[] { p0, p1 };
            Edges = new (int, int)[] { (0, 1) };
        }
    }
}
