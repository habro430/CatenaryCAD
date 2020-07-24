using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using Multicad;
using Multicad.Geometry;

namespace CatenaryCAD.Objects
{
    internal interface IHandler
    {
        /// <summary>
        /// Объект <see cref="CatenaryCAD"/>
        /// </summary>
        public IObject CatenaryObject { get; set; }

        /// <summary>
        /// Уникальный идентификатор <see cref="McObjectId"/> обработчика <see cref="IHandler"/>
        /// </summary>
        public McObjectId Identifier { get; }

        /// <summary>
        /// Родительский обработчик <see cref="IHandler"/>
        /// </summary>
        public IHandler Parent { get; }

        /// <summary>
        /// Получить дочерние обработчики <see cref="IHandler"/>
        /// </summary>
        /// <returns>Массив из дочерних обработчиков <see cref="IHandler"/>[]</returns>
        public IHandler[] GetChildrens();

        /// <summary>
        /// Получить зависимые обьекты <see cref="IHandler"/>
        /// </summary>
        /// <returns>Массив из зависимых обработчиков <see cref="IHandler"/>[]</returns>
        public IHandler[] GetDependents();

        /// <summary>
        /// Получить параметры обработчика <see cref="IHandler"/> и объекта <see cref="IObject"/> 
        /// </summary>
        /// <returns>Массив параметров <see cref="IProperty"/>[]</returns>
        public IProperty[] GetProperties();

        /// <summary>
        /// Получить детали обработчика <see cref="IHandler"/> и объекта <see cref="IObject"/> 
        /// </summary>
        /// <returns>Массив деталей <see cref="IPart"/>[]</returns>
        public IPart[] GetParts();

        /// <summary>
        /// Позиция текущего обработчика
        /// </summary>
        public Point3d Position { get; set; }

        /// <summary>
        /// Направление текущего обработчика
        /// </summary>
        public Vector3d Direction { get; set; }

        /// <summary>
        /// Функция транформации обработчика
        /// </summary>
        /// <param name="m">Матрица трансформации</param>
        public void Transform(Matrix3d m);

    }
}
