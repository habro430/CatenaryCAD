using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Models
{
    public interface IModel
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
        /// Координаты расположения модели <see cref="IModel"/> в документе
        /// </summary>
        public Point3D Position { get; set; }

        /// <summary>
        /// Направление расположения модели <see cref="IModel"/> в документе
        /// </summary>
        public Vector3D Direction { get; set; }

        public IProperty[] Properties { get; }

        public IPart[] Parts { get; }

        /// <summary>
        /// Функция транформации модели <see cref="IModel"/>
        /// </summary>
        /// <param name="m">Матрица трансформации</param>
        public void TransformBy(Matrix3D m);

        /// <summary>
        /// Получить 3D геометрию для режима работы <see cref="OperationalMode.Layout"/>
        /// </summary>
        /// <returns>3D геометрия</returns>
        public IMesh[] GetGeometryForLayout();

        /// <summary>
        /// Получить 2D геометрию для режима работы <see cref="OperationalMode.Scheme"/>
        /// </summary>
        /// <returns>2D геометрия</returns>
        public IShape[] GetGeometryForScheme();

        public bool? SendMessageToHandler(HandlerMessages message);
    }
}
