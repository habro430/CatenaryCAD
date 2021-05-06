using CatenaryCAD.Geometry;
using CatenaryCAD.Models;
using System;

namespace BasicFoundations
{
    [Serializable]
    public abstract class TSS : AbstractFoundation
    {
        public override Point3D? GetDockingPoint(IModel from, Ray3D ray) => new Point3D(0, 0, -800);
    }
}
