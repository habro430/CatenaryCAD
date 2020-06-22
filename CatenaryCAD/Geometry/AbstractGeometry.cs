using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public abstract class AbstractGeometry
    {
        public virtual Point[] Vertices { get; protected set; }
        public virtual (int, int)[] Edges { get; protected set; }
        public virtual AbstractGeometry TransformBy(Matrix m)
        {
            Vertices = Vertices.TransformBy(m);
            return this;
        }
    }
}
