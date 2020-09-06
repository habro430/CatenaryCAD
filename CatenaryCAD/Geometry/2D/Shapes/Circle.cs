using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Circle : IShape
    {
        public Edge2D[] Edges { private set; get; }

        public Circle(Point2D center, double radius, int resolution)
        {
            var vertices = new Point2D[resolution];
            Edges = new Edge2D[resolution];

            for (int i = 0; i < resolution; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / resolution) * radius + center.X;
                double y = Math.Sin(2 * Math.PI * i / resolution) * radius + center.Y;

                vertices[i] = new Point2D(x, y);
            }

            for (int i = 0; i < resolution; i++)
            {
                if (i < resolution - 1)
                    Edges[i] = new Edge2D(vertices[i], vertices[i + 1]);
                else
                    Edges[i] = new Edge2D(vertices[i], vertices[0]);
            }

        }

        public IShape TransformBy(in Matrix2D m)
        {
            int count = Edges.Length;

            Edge2D[] edges = new Edge2D[count];

            for (int i = 0; i < count; i++)
                edges[i] = Edges[i].TransformBy(m);

            return new Circle(new Point2D(), 0, 0) { Edges = edges };
        }
    }

}
