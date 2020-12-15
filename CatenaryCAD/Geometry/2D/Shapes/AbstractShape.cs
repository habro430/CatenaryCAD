using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public abstract class AbstractShape : IShape
    {
        public virtual Point2D[] Vertices { get; protected set; }

        public virtual int[][] Indices { get; protected set; }

        public virtual IShape TransformBy(in Matrix2D matrix)
        {
            int count = Vertices.Length;

            for (int i = 0; i < count; i++)
                Vertices[i] = Vertices[i].TransformBy(matrix);

            return this;
        }

        public virtual bool IsInside(in Point2D point)
        {
            int count = Vertices.Length;

            bool c = false;
            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                if ((((Vertices[i].Y <= point.Y) && (point.Y < Vertices[j].Y)) || ((Vertices[j].Y <= point.Y) && (point.Y < Vertices[i].Y))) &&
                  (((Vertices[j].Y - Vertices[i].Y) != 0) && (point.X > ((Vertices[j].X - Vertices[i].X) * (point.Y - Vertices[i].Y) / (Vertices[j].Y - Vertices[i].Y) + Vertices[i].X))))
                    c = !c;
            }
            return c;
        }


    }
}
