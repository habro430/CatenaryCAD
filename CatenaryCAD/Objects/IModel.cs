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
        /// Событие сигнализируещее об обновлении <see cref="IModel"/>
        /// </summary>
        /// <remarks>При обновлении объекта, обработчик <see cref="IHandler"/> получает команду на сохранение 
        /// изменений для undo и redo, а так же ставит объект <see cref="IModel"/> в очередь на перерисовку.</remarks>
        public event Action Updated;

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

    }
}
