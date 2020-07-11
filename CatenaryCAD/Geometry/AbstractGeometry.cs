using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public abstract class AbstractGeometry<T> where T : struct, IParticle<T>
    {
        public virtual Point<T>[] Vertices { get; protected set; }
        public virtual (Point<T>, Point<T>)[] Edges { get; protected set; }
        public virtual AbstractGeometry<T> TransformBy(Matrix3 m)
        {
            //Vertices = Vertices.TransformBy(m);
            return this;
        }
    }
}
