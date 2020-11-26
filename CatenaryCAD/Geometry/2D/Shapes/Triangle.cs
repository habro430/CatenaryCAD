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

        //public bool IsInside(in Point2D p)
        //{
        //    Random rnd = new Random();
        //    Vector2D rnd_vector = new Vector2D(rnd.NextDouble(), rnd.NextDouble()).Normalize() * 9999d;

        //    #warning не определена макисмальная длинна вектора

        //    Ray2D ray = new Ray2D(p, rnd_vector);
        //    Point2D[] intesections = ray.GetIntersections(this);

        //    if (intesections == null) return false;

        //    if (intesections.Length % 2 == 0) return false;
        //    else return true;
        //}
    }
}
