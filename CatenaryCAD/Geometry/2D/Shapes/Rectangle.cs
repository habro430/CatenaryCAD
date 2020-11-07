using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Rectangle : IShape
    {
        public Edge2D[] Edges { private set; get; }

        public Rectangle(in Point2D center, double width, double height)
        {
            var vertices = new Point2D[]
            {
                new Point2D(center.X - height / 2, center.Y + width / 2),
                new Point2D(center.X + height / 2, center.Y + width / 2),
                new Point2D(center.X + height / 2, center.Y - width / 2),
                new Point2D(center.X - height / 2, center.Y - width / 2)
            };

            Edges = new Edge2D[] {  new Edge2D(vertices[0], vertices[1]),
                                    new Edge2D(vertices[1], vertices[2]),
                                    new Edge2D(vertices[2], vertices[3]),
                                    new Edge2D(vertices[3], vertices[0]) };
        }

        public Rectangle(Point2D p0, Point2D p1, Point2D p2, Point2D p3)
        {
            var vertices = new Point2D[] { p0, p1, p2, p3 };

            Edges = new Edge2D[] {  new Edge2D(vertices[0], vertices[1]),
                                    new Edge2D(vertices[1], vertices[2]),
                                    new Edge2D(vertices[2], vertices[3]),
                                    new Edge2D(vertices[3], vertices[0]) };
        }

        public IShape TransformBy(in Matrix2D m)
        {
            int count = Edges.Length;

            Edge2D[] edges = new Edge2D[count];

            for(int i =0; i<count; i++)
                edges[i] = Edges[i].TransformBy(m);

            return new Rectangle(Point2D.Origin, Point2D.Origin, Point2D.Origin, Point2D.Origin) { Edges = edges };
        }
    }
}
