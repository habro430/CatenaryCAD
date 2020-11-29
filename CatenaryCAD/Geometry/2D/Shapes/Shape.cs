using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public abstract class Shape : IShape
    {
        public virtual Point2D[] Vertices { get; protected set; }

        public virtual int[][] Indices { get; protected set; }

        public virtual IShape TransformBy(in Matrix2D m)
        {
            int count = Vertices.Length;
            for (int i = 0; i < count; i++)
                Vertices[i] = Vertices[i].TransformBy(m);

            return this;
        }


        public virtual bool IsInside(in Point2D p)
        {
            int npol = Vertices.Length;

            bool c = false;
            for (int i = 0, j = npol - 1; i < npol; j = i++)
            {
                if ((((Vertices[i].Y <= p.Y) && (p.Y < Vertices[j].Y)) || ((Vertices[j].Y <= p.Y) && (p.Y < Vertices[i].Y))) &&
                  (((Vertices[j].Y - Vertices[i].Y) != 0) && (p.X > ((Vertices[j].X - Vertices[i].X) * (p.Y - Vertices[i].Y) / (Vertices[j].Y - Vertices[i].Y) + Vertices[i].X))))
                    c = !c;
            }
            return c;
        }


    }
}
