using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Line : IShape
    {
        public Edge2D[] Edges { private set; get; }

        public Line(in Point2D p0, in Point2D p1)
        {
            Edges = new Edge2D[] { new Edge2D(p0, p1) };
        }

        public IShape TransformBy(in Matrix2D m)
        {
            int count = Edges.Length;
            for (int i = 0; i < count; i++)
                Edges[i] = Edges[i].TransformBy(m);

            return this;
        }
    }
}
