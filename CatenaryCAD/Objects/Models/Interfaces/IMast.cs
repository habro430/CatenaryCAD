using CatenaryCAD.Geometry;
using System;

namespace CatenaryCAD.Models
{
    public interface IMast : IModel
    {
        /// <summary>
        /// Типы допустимых фундаментов для опоры
        /// </summary>
        /// <remarks>
        /// При наличии в массиве абстрактного класса, то допустимыми фундаментами являються
        /// все наследуемые от абстрактного класса модели фундаментов. 
        /// При наличии класса определенной модели фундамента, то допустимым фундаментом 
        /// являеться только указанная модель (т.е. наследуемые модели от указанной не учитываются) 
        /// </remarks>
        /// <value>По умолчанию возвращает все фундаменты, наследуемые от <seealso cref="Foundation"/></value>
        Type[] Foundations { get; }

        /// <summary>
        /// Возвращает точку присоединения арматуры к стойке на плане
        /// </summary>
        /// <param name="ray">Луч для определения пересечения</param>
        /// <returns>Если луч и геометрия на плане пересекаются, то возвращает <seealso cref="Point2D"/> 
        /// в месте пересечения, в противном случае возвращает null</returns>
        Point2D? GetPointForDockingJoint(Ray2D ray);

        /// <summary>
        /// Возвращает точку присоединения арматуры к стойке на макете
        /// </summary>
        /// <param name="ray">Луч для определения пересечения</param>
        /// <returns>Если луч и геометрия на макете пересекаются, то возвращает <seealso cref="Point3D"/> 
        /// в месте пересечения, в противном случае возвращает null</returns>
        Point3D? GetPointForDockingJoint(Ray3D ray);
    }
}
