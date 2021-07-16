using Catenary.Geometry;
using Catenary.Geometry.Interfaces;
using Catenary.Geometry.Meshes;
using Catenary.Maintenances;

namespace Catenary.Components
{
    /// <summary>
    /// Интерфейс, описывающий контракты для деталей моделей объектов контактной сети.
    /// </summary>
    public interface IComponent : IRotationable<Vector3D>, IOriginable<Point3D>
    {
        /// <summary>
        /// Возвращает массив 3D геометрии с оносительными для детали координатами.
        /// </summary>
        /// <value>
        /// Массив с 3D геометрией.
        /// </value>
        public IMesh[] Geometry { get; }

        /// <summary>
        /// Возвращает массив c необходимым набором видов технического облуживание детали.
        /// </summary>
        /// <value>
        /// Массив с видами технического обслуживания.
        /// </value>
        public IMaintenance[] Maintenances { get; }

    }
}
