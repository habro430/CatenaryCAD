using CatenaryCAD.Geometry;
using System;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract class Mast : Model, IMast
    {
        public virtual Type[] Foundations => new Type[] { typeof(Foundation) };

        public abstract Point2D? GetDockingPoint(Ray2D ray);
        public abstract Point3D? GetDockingPoint(Ray3D ray);
    }
}
