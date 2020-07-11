using System;
using System.Linq;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public abstract class AbstractGeometry<T> where T : struct, IParticle<T>
    {
        public virtual Point<T>[] Vertices { get; protected set; }
        public virtual (Point<T>, Point<T>)[] Edges { get; protected set; }
        public virtual AbstractGeometry<T> TransformBy(Matrix m)
        {
            for(int i =0; i< Vertices.Count(); i++)
            {
                Vertices[i].TransformBy(m);
            }
            return this;
        }
    }
}
