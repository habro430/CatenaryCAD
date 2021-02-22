using CatenaryCAD.Components;
using CatenaryCAD.Properties;
using Multicad.Geometry;

namespace CatenaryCAD.Models.Handlers
{
    internal interface IHandler
    {
        /// <summary>
        /// Объект <see cref="CatenaryCAD"/>
        /// </summary>
        public IModel Model { get; set; }

        /// <summary>
        /// Получить параметры обработчика <see cref="IHandler"/> и объекта <see cref="IModel"/> 
        /// </summary>
        /// <returns>Массив параметров <see cref="IProperty"/>[]</returns>
        public IProperty[] GetProperties();

    }
}
