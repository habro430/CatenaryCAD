using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    public interface IObject
    {
        /// <summary>
        /// Событие сигнализируещее об обновлении <see cref="IObject"/>
        /// </summary>
        /// <remarks>При обновлении объекта, обработчик <see cref="IHandler"/> получает команду на сохранение 
        /// изменений для undo и redo, а так же ставит объект <see cref="IObject"/> в очередь на перерисовку.</remarks>
        public event Action Updated;

        /// <summary>
        /// Получить детали обработчика <see cref="IHandler"/> и объекта <see cref="IObject"/> 
        /// </summary>
        /// <returns>Массив деталей <see cref="IPart"/>[]</returns>
        public IPart[] GetParts();

        /// <summary>
        /// Получить параметры объекта <see cref="IObject"/> 
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
