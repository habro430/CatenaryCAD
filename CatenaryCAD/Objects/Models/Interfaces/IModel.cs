using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Models
{
    public interface IModel : IPositionable<Point3D>, IDirectionable<Vector3D>, ITransformable<Matrix3D, IModel>
    {
        /// <summary>
        /// Идентификатор модели
        /// </summary>
        public IIdentifier Identifier { get; }

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
        /// Массив деталей <see cref="IPart"/> модели <see cref="IModel"/>
        /// </summary>
        public IPart[] Parts { get; }

        /// <summary>
        /// Получить 3D геометрию для режима работы <see cref="OperationalMode.Layout"/>
        /// </summary>
        /// <returns>3D геометрия</returns>
        public IMesh[] GetGeometryForLayout();

        /// <summary>
        /// Получить 2D геометрию для режима работы <see cref="OperationalMode.Scheme"/>
        /// </summary>
        /// <returns>2D геометрия</returns>
        public IShape[] GetGeometry();

        public bool? SendMessageToHandler(HandlerMessages message);
    }
}
