using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Line : Shape
    {
        public Line(in Point2D p0, in Point2D p1)
        {
            Vertices = new Point2D[] { p0, p1 };
            Indices = new int[][] { new int[] { 0, 1 } };
        }
    }
}
