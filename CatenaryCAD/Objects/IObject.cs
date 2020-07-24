using CatenaryCAD.Geometry;
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

        public void GetGeometry(out AbstractGeometry<XY>[] xy, out AbstractGeometry<XYZ>[] xyz);
    }
}
