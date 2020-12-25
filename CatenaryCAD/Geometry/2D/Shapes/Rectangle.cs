using System;

namespace CatenaryCAD.Geometry.Shapes
{
    /// <summary>
    /// Класс, реализующий стандартный круг в 2D пространстве.
    /// </summary>
    [Serializable]
    public sealed class Rectangle : Shape
    {
        public Rectangle(in Point2D center, double width, double height)
        {
            Vertices = new Point2D[]
            {
                new Point2D(center.X - height / 2, center.Y + width / 2),
                new Point2D(center.X + height / 2, center.Y + width / 2),
                new Point2D(center.X + height / 2, center.Y - width / 2),
                new Point2D(center.X - height / 2, center.Y - width / 2)
            };
            Indices = new int[][] 
            {  
                new int[]{ 0, 1 },
                new int[]{ 1, 2 },
                new int[]{ 2, 3 },
                new int[]{ 3, 0 } 
            };
        }

        public Rectangle(Point2D p0, Point2D p1, Point2D p2, Point2D p3)
        {
            Vertices = new Point2D[] { p0, p1, p2, p3 };
            Indices = new int[][]
            {
                new int[]{ 0, 1 },
                new int[]{ 1, 2 },
                new int[]{ 2, 3 },
                new int[]{ 3, 0 }
            };
        }
    }
}
