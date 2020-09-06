using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public class Edge2D
    {
        public readonly Point2D First;
        public readonly Point2D Second;

        public Edge2D(Point2D first, Point2D second)
        {
            First = first;
            Second = second;
        }

        public Edge2D TransformBy(in Matrix2D m) => new Edge2D(First.TransformBy(m), Second.TransformBy(m));
    }
}
