using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public abstract class AbstractGeometry
    {
        public virtual Point3[] Vertices { get; protected set; }
        public virtual (int, int)[] Edges { get; protected set; }
        public virtual AbstractGeometry TransformBy(Matrix3 m)
        {
            Vertices = Vertices.TransformBy(m);
            return this;
        }
    }
}
