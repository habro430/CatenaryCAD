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
        /// Типы допустимых фундаментов для опоры.
        /// </summary>
        /// <remarks>
        /// При наличии в массиве абстрактного класса, то допустимыми фундаментами являются
        /// все наследуемые от абстрактного класса модели фундаментов. 
        /// При наличии класса определенной модели фундамента, то допустимым фундаментом 
        /// являеться указанная модель и все не абстрактные, наследуемые от указанного класса модели. 
        /// </remarks>
        /// <value>По умолчанию возвращает все модели, наследуемые от <seealso cref="Foundation"/>.</value>
        Type[] PossibleFoundations { get; }

        /// <summary>
        /// Возвращает точку присоединения <see cref="Point2D"/> арматуры к стойке в месте пересечения луча <see cref="Ray2D"/> и геометрии <see cref="IShape"/>.
        /// </summary>
        /// <param name="ray">Луч для определения пересечения.</param>
        /// <returns>Если луч и геометрия на плане пересекаются, то возвращает <seealso cref="Point2D"/> 
        /// в месте пересечения, в противном случае возвращает <see langword="null"/>.</returns>
        Point2D? GetDockingPoint(Ray2D ray);

        /// <summary>
        /// Возвращает точку присоединения <see cref="Point3D"/> арматуры к стойке в месте пересечения луча <see cref="Ray3D"/> и геометрии деталей <see cref="IMesh"/>.
        /// </summary>
        /// <param name="ray">Луч для определения пересечения.</param>
        /// <returns>Если луч и геометрия на макете пересекаются, то возвращает <seealso cref="Point3D"/> 
        /// в месте пересечения, в противном случае возвращает <see langword="null"/>.</returns>
        Point3D? GetDockingPoint(Ray3D ray);
    }
}
