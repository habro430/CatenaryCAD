using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Models.Events;
using CatenaryCAD.Properties;
using static CatenaryCAD.OperationalCommands;

namespace CatenaryCAD.Models
{
    public interface IModel : IPositionable<Point3D>, IDirectionable<Vector3D>, ITransformable<Matrix3D, IModel>
    {
        /// <summary>
        /// Идентификатор модели
        /// </summary>
        public IObjectID Identifier { get; }

        /// <summary>
        /// Родительская модель <see cref="IModel"/>
        /// </summary>
        public IModel Parent { get; set; }

        /// <summary>
        /// Дочерние модели <see cref="IModel"/>
        /// </summary>
        /// <remarks>Добавление и исключение моделий к числу дочерних происходит 
        /// автоматически при изменении значения <see cref="Parent"/></remarks>

        public IModel[] Childrens { get; }

        /// <summary>
        /// Зависимые модели <see cref="IModel"/>
        /// </summary>
        public IModel[] Dependencies { get; }

        /// <summary>
        /// Массив параметров <see cref="IProperty"/> модели <see cref="IModel"/>
        /// </summary>
        public IProperty[] Properties { get; }

        /// <summary>
        /// Массив деталей <see cref="IComponent"/> модели <see cref="IModel"/>
        /// </summary>
        public IComponent[] Components { get; }

        /// <summary>
        /// Получить 2D геометрию для режима работы <see cref="OperationalMode.Scheme"/>
        /// </summary>
        /// <returns>2D геометрия</returns>
        public IShape[] GetGeometry();

        /// <summary>
        /// Возвращает точку присоединения <see cref="Point2D"/> к объекту <see cref="IModel"/> в месте пересечения луча <see cref="Ray2D"/> и геометрии <see cref="IShape"/>.
        /// </summary>
        /// <param name="ray">Луч для определения пересечения.</param>
        /// <param name="from">Тип обьекта <see cref="IModel"/> для которого выполняеться поиск точки присоединения.</param>
        /// <returns>Точка присодинения <see cref="Point2D"/> к объекту <see cref="IModel"/></returns>
        Point2D? GetDockingPoint(IModel from, Ray2D ray);

        /// <summary>
        /// Возвращает точку присоединения <see cref="Point3D"/> к объекту <see cref="IModel"/> в месте пересечения луча <see cref="Ray3D"/> и геометрии деталей <see cref="IMesh"/>.
        /// </summary>
        /// <param name="ray">Луч для определения пересечения.</param>
        /// <param name="from">Тип обьекта <see cref="IModel"/> для которого выполняеться поиск точки присоединения.</param>
        /// <returns>Точка присодинения <see cref="Point3D"/> к объекту <see cref="IModel"/></returns>
        Point3D? GetDockingPoint(IModel from, Ray3D ray);
    }
}
