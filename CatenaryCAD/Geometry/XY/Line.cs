using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Line : AbstractGeometry<XY>
    {
        public Line(Point<XY> p0, Point<XY> p1)
        {
            Vertices = new Point<XY>[] { p0, p1 };
            Edges = new (Point<XY>, Point<XY>)[] { (p0, p1) };
        }
    }
}
