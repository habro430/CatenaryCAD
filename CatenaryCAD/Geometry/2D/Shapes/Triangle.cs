using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    class Triangle : IShape
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

            Edge2D[] edges = new Edge2D[count];

            for (int i = 0; i < count; i++)
                edges[i] = Edges[i].TransformBy(m);

            return new Triangle(Point2D.Origin, Point2D.Origin, Point2D.Origin) { Edges = edges };
        }
    }
}
