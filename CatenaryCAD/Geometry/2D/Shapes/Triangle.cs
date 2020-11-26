using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public class Triangle : IShape
    {
        public Edge2D[] Edges { private set; get; }

        public Triangle(Point2D p0, Point2D p1, Point2D p2)
        {
            var vertices = new Point2D[] { p0, p1, p2 };

            Edges = new Edge2D[] {  new Edge2D(vertices[0], vertices[1]),
                                    new Edge2D(vertices[1], vertices[2]),
                                    new Edge2D(vertices[2], vertices[0]) };
        }

        public IShape TransformBy(in Matrix2D m)
        {
            int count = Edges.Length;
            for (int i = 0; i < count; i++)
                Edges[i] = Edges[i].TransformBy(m);

            return this;
        }

        public bool IsInside(in Point2D p)
        {
            Random rnd = new Random();
            Vector2D rnd_vector = new Vector2D(rnd.NextDouble(), rnd.NextDouble()).Normalize() * 9999d;

            #warning не определена макисмальная длинна вектора

            Ray2D ray = new Ray2D(p, rnd_vector);
            Point2D[] intesections = ray.GetIntersections(this);

            if (intesections == null) return false;

            if (intesections.Length % 2 == 0) return false;
            else return true;
        }
    }
}
