using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public class Triangle : Shape
    {
        public Triangle(Point2D p0, Point2D p1, Point2D p2)
        {
            Vertices = new Point2D[] { p0, p1, p2 };
            Indices = new int[][] { new int[] { 0, 1 },
                                    new int[] { 1, 2 },
                                    new int[] { 2, 0 }};
        }
    }
}
