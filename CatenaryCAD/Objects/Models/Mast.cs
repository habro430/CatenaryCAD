using CatenaryCAD.Geometry;
using System;

namespace CatenaryCAD.Models
{
    [Serializable]
    public abstract class Mast : Model, IMast
    {
        public abstract Point2D? GetDockingJointPoint(Ray2D ray);
        public abstract Point3D? GetDockingJointPoint(Ray3D ray);
    }
}
