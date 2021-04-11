using CatenaryCAD.Geometry;
using System;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Модель, описывающая опору контактной сети.
    /// </summary>
    [Serializable]
    public abstract class Mast : Model, IMast
    {
        /// <inheritdoc/>
        public virtual Type[] PossibleFoundations => new Type[] { typeof(Foundation) };

        /// <inheritdoc/>
        public abstract Point2D? GetDockingPoint(Ray2D ray);

        /// <inheritdoc/>
        public abstract Point3D? GetDockingPoint(Ray3D ray);
    }
}
