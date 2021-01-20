using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Geometry.Meshes;

namespace CatenaryCAD.Parts
{
    public interface IPart : IDirectionable<Vector3D>, IPositionable<Point3D>, ITransformable<Matrix3D, IPart>
    {
        /// <summary>
        /// 3D геометрия для режима работы <see cref="OperationalMode.Layout"/>
        /// </summary>
        /// <returns>Массив объектов <see cref="IMesh"/></returns>
        public IMesh[] Geometry { get; }
    }
}
