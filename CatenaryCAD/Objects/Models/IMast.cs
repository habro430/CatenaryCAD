using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using System;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели опоры контактной сети.
    /// </summary>
    public interface IMast : IModel
    {
        /// <summary>
        /// Типы допустимых моделей <see cref="IFoundation"/>, на которые может быть установлена данная модель <see cref="IMast"/>.
        /// </summary>
        /// <value>Модели, наследуемые от <seealso cref="IFoundation"/>.</value>
        Type[] AllowableFoundations { get; }

        /// <summary>
        /// Возвращает точку присоединения <see cref="Point2D"/> арматуры к стойке в месте пересечения луча <see cref="Ray2D"/> и геометрии <see cref="IShape"/>.
        /// </summary>
        /// <param name="ray">Луч для определения пересечения.</param>
        /// <returns>Точка присодинения <see cref="Point2D"/> арматуры к <see cref="IMast"/> </returns>
        Point2D? GetDockingPointForArmatory(Ray2D ray);

        /// <summary>
        /// Возвращает точку присоединения <see cref="Point3D"/> арматуры к стойке в месте пересечения луча <see cref="Ray3D"/> и геометрии деталей <see cref="IMesh"/>.
        /// </summary>
        /// <param name="ray">Луч для определения пересечения.</param>
        /// <returns>Точка присодинения <see cref="Point3D"/> арматуры к <see cref="IMast"/> </returns>
        Point3D? GetDockingPointForArmatory(Ray3D ray);
    }
}
