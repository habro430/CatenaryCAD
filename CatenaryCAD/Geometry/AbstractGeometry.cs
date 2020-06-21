using CatenaryCAD.Geometry.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public abstract class AbstractGeometry
    {
        public virtual Point[] Points { get; protected set; }
        public virtual (int, int)[] Edges { get; protected set; }
        public virtual AbstractGeometry TransformBy(Matrix m)
        {
            Points = Points.TransformBy(m);
            return this;
        }
    }
}
