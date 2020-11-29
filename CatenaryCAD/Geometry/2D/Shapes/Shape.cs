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
            throw new NotImplementedException();
        }


    }
}
