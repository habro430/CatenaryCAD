using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Properties;
using static CatenaryCAD.Extensions;

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

        public bool? SendMessageToHandler(HandlerMessages message);
    }
}
