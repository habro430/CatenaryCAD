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
        public IIdentifier Identifier { get; }

        public IModel Parent { get; set; }
        public IModel[] Childrens { get; }
        public IModel[] Dependencies { get; }

        /// <summary>
        /// Координаты расположения модели <see cref="IModel"/> в документе
        /// </summary>
        public Point3D Position { get; set; }

        /// <summary>
        /// Направление расположения модели <see cref="IModel"/> в документ
        /// </summary>
        public Vector3D Direction { get; set; }

        /// <summary>
        /// Функция транформации модели <see cref="IModel"/>
        /// </summary>
        /// <param name="m">Матрица трансформации</param>
        public void TransformBy(Matrix3D m);

        /// <summary>
        /// Получить детали объекта <see cref="IModel"/> 
        /// </summary>
        /// <returns>Массив деталей <see cref="IPart"/>[]</returns>
        public IPart[] GetParts();

        /// <summary>
        /// Получить параметры объекта <see cref="IModel"/> 
        /// </summary>
        /// <returns>Массив параметров <see cref="IProperty"/>[]</returns>
        public IProperty[] GetProperties();

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

        public bool TryModifyModel();
        public bool UpdateModel();
    }
}
