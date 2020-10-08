﻿using CatenaryCAD.Parts;
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
        public void TransformBy(Matrix3d m);

    }
}
