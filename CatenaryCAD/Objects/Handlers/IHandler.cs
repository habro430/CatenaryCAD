using CatenaryCAD.Properties;
using Multicad;
using Multicad.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatenaryCAD.Objects.Handlers
{
    internal interface IHandler
    {
        /// <summary>
        /// Объект CatenaryCAD
        /// </summary>
        public IObject CatenaryObject { get; set; }
        /// <summary>
        /// Уникальный идентификатор обработчика
        /// </summary>
        /// <returns>ID обработчика</returns>
        public McObjectId GetID();

        #region Parent & Childrens region
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
        /// Добавить дочерний обработчик <see cref="IHandler"/> в <see cref="IHandler"/>
        /// </summary>
        /// <param name="handler">Дочерний обработчик <see cref="IHandler"/></param>
        /// <returns>Если дочерний обработчик добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddChild(IHandler handler);

        /// <summary>
        /// Удалить дочерний обработчик <see cref="IHandler"/> из <see cref="IHandler"/>
        /// </summary>
        /// <param name="handler">Дочерний обработчик <see cref="IHandler"/></param>
        /// <returns>Если дочерний обработчик удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveChild(IHandler handler);
        #endregion

        #region Dependents region
        /// <summary>
        /// Получить зависимые обьекты <see cref="IHandler"/>
        /// </summary>
        /// <returns>Массив из зависимых обработчиков <see cref="IHandler"/>[]</returns>
        public IHandler[] GetDependents();

        /// <summary>
        /// Добавить зависимый обработчик <see cref="IHandler"/> в <see cref="IHandler"/>
        /// </summary>
        /// <param name="handler">Зависимый обработчик <see cref="IHandler"/></param>
        /// <returns>Если зависимый обработчик добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddDependent(IHandler handler);

        /// <summary>
        /// Удалить зависимый обьект <see cref="IHandler"/> из <see cref="IHandler"/>
        /// </summary>
        /// <param name="handler">Зависимый обработчик <see cref="IHandler"/></param>
        /// <returns>Если зависимый обработчик удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveDependent(IHandler handler);
        #endregion

        #region Properties region
        /// <summary>
        /// Получить параметры <see cref="IHandler"/> + <see cref="IObject"/> 
        /// </summary>
        /// <returns>Отсортированный по <see cref="IProperty.ID"/> массив параметров <see cref="IProperty"/>[]</returns>
        public IProperty[] GetProperties();

        /// <summary>
        /// Добавить параметр <see cref="IProperty"/> в <see cref="IHandler"/>
        /// </summary>
        /// <param name="property">Зависимый обработчик <see cref="IProperty"/></param>
        /// <returns>Если зависимый обработчик добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddProperty(IProperty property);

        /// <summary>
        /// Удалить параметр <see cref="IProperty"/> из <see cref="IHandler"/>
        /// </summary>
        /// <param name="property">Зависимый обработчик <see cref="IHandler"/></param>
        /// <returns>Если зависимый обработчик удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveProperty(IProperty property);
        #endregion

        #region Position & Direction region
        /// <summary>
        /// Позиция текущего обработчика
        /// </summary>
        public Point3d Position { get; set; }

        /// <summary>
        /// Направление текущего обработчика
        /// </summary>
        public Vector3d Direction { get; set; }
        #endregion

        /// <summary>
        /// Функция транформации обработчика
        /// </summary>
        /// <param name="m">Матрица трансформации</param>
        public void Transform(Matrix3d m);

    }
}
