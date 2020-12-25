using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public class Triangle : Shape
    {

        public Triangle(Point2D a, Point2D b, Point2D c)
        {
            Vertices = new Point2D[] { a, b, c };
            Indices = new int[][] { new int[] { 0, 1 },
                                    new int[] { 1, 2 },
                                    new int[] { 2, 0 }};
        }
    }
}
