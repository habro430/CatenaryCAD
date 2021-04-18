using CatenaryCAD.Models;
using System;

namespace CatenaryCAD.Attributes
{
    /// <summary>
    /// Атрибут, представляющий описание объекта <see cref="IModel"/>.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelDescriptionAttribute : Attribute
    {
        /// <returns>
        /// Описание объекта <see cref="IModel"/>.
        /// </returns>
        public readonly string Description;

        /// <param name="description">Описание объекта <see cref="IModel"/>.</param>
        public ModelDescriptionAttribute(string description) => Description = description;

        public override string ToString() => Description;
    }
}
