using CatenaryCAD.Geometry;
using System;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract class Mast : Model, IMast
    {
        public virtual Type[] Foundations => new Type[] { typeof(Foundation) };

        public abstract Point2D? GetPointForDockingJoint(Ray2D ray);
        public abstract Point3D? GetPointForDockingJoint(Ray3D ray);
    }
}
