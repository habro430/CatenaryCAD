using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Maintenances;

namespace CatenaryCAD.ComponentParts
{
    /// <summary>
    /// Интерфейс, описывающий контракты для деталей моделей объектов контактной сети.
    /// </summary>
    public interface IComponentPart : IDirectionable<Vector3D>, IPositionable<Point3D>, ITransformable<Matrix3D, IComponentPart>
    {
        /// <summary>
        /// Возвращает массив 3D геометрии с оносительными для детали координатами.
        /// </summary>
        /// <value>
        /// Массив с 3D геометрией.
        /// </value>
        public IMesh[] Geometry { get; }

        /// <summary>
        /// Возвращает стоимость детали для рассчета сметы.
        /// </summary>
        /// <value>
        /// Каталожая стоимость детали.
        /// </value>
        public decimal Price { get; }

        /// <summary>
        /// Возвращает массу детали.
        /// </summary>
        /// <value>
        /// Масса детали в киллограммах.
        /// </value>
        public double Mass { get; }

        /// <summary>
        /// Возвращает центр массы с оносительными для детали координатами.
        /// </summary>
        /// <value>
        /// Цетр массы детали.
        /// </value>
        public Point3D CenterOfMass { get; }

        /// <summary>
        /// Возвращает массив c необходимым набором видов технического облуживание детали.
        /// </summary>
        /// <value>
        /// Массив с видами технического обслуживания.
        /// </value>
        public IMaintenance Maintenances { get; }

    }
}
