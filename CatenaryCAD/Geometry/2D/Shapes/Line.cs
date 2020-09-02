using CatenaryCAD.Geometry.Interfaces;
using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Line : IShape
    {
        public Edge2D[] Edges { private set; get; }

        public Line(Point2D p0, Point2D p1)
        {
            Edges = new Edge2D[] { new Edge2D(p0, p1) };
        }

        public IShape TransformBy(in Matrix2D m)
        {
            int count = Edges.Length;

            Edge2D[] edges = new Edge2D[count];

            for (int i = 0; i < count; i++)
                edges[i] = Edges[i].TransformBy(m);

            return new Line(new Point2D(), new Point2D()) { Edges = edges };
        }
    }
}
