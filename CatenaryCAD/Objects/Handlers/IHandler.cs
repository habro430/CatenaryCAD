using CatenaryCAD.Properties;
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

        #region Parent & Childrens region
        /// <summary>
        /// Родительский объект <see cref="AbstractHandler"/>
        /// </summary>
        public AbstractHandler Parent { get; }

        /// <summary>
        /// Получить дочерние объекты <see cref="AbstractHandler"/>
        /// </summary>
        /// <returns>Массив из дочерних объектов <see cref="AbstractHandler"/>[]</returns>
        public AbstractHandler[] GetChildrens();

        /// <summary>
        /// Добавить дочерний объект <see cref="AbstractHandler"/> в <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Дочерний объект <see cref="AbstractHandler"/></param>
        /// <returns>Если дочерний объект добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddChild(AbstractHandler handler);

        /// <summary>
        /// Удалить дочерний объект <see cref="AbstractHandler"/> из <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Дочерний объект <see cref="AbstractHandler"/></param>
        /// <returns>Если дочерний объект удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveChild(AbstractHandler handler);
        #endregion

        #region Dependents region
        /// <summary>
        /// Получить зависимые обьекты <see cref="AbstractHandler"/>
        /// </summary>
        /// <returns>Массив из зависимых объектов <see cref="AbstractHandler"/>[]</returns>
        public AbstractHandler[] GetDependents();

        /// <summary>
        /// Добавить зависимый объект <see cref="AbstractHandler"/> в <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Зависимый объект <see cref="AbstractHandler"/></param>
        /// <returns>Если зависимый объект добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddDependent(AbstractHandler handler);

        /// <summary>
        /// Удалить зависимый обьект <see cref="AbstractHandler"/> из <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="handler">Зависимый объект <see cref="AbstractHandler"/></param>
        /// <returns>Если зависимый объект удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveDependent(AbstractHandler handler);
        #endregion

        #region Properties region
        /// <summary>
        /// Получить параметры <see cref="AbstractHandler"/> + <see cref="IObject"/> 
        /// </summary>
        /// <returns>Отсортированный по <see cref="IProperty.ID"/> массив параметров <see cref="IProperty"/>[]</returns>
        public IProperty[] GetProperties();

        /// <summary>
        /// Добавить параметр <see cref="IProperty"/> в <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="property">Зависимый объект <see cref="IProperty"/></param>
        /// <returns>Если зависимый объект добавлен то <c>true</c>, иначе <c>false</c></returns>
        public bool AddProperty(IProperty property);

        /// <summary>
        /// Удалить параметр <see cref="IProperty"/> из <see cref="AbstractHandler"/>
        /// </summary>
        /// <param name="property">Зависимый объект <see cref="AbstractHandler"/></param>
        /// <returns>Если зависимый объект удален то <c>true</c>, иначе <c>false</c></returns>
        public bool RemoveProperty(IProperty property);
        #endregion

        #region Position & Direction region
        /// <summary>
        /// Позиция текущего объекта
        /// </summary>
        public Point3d Position { get; set; }

        /// <summary>
        /// Направление текущего объекта
        /// </summary>
        public Vector3d Direction { get; set; }
        #endregion

        /// <summary>
        /// Функция транформации объекта
        /// </summary>
        /// <param name="m">Матрица трансформации</param>
        public void Transform(Matrix3d m);

    }
}
