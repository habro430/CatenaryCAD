using Catenary.Models;
using System;

namespace Catenary.Attributes
{
    /// <summary>
    /// Атрибут, представляющий имя объекта <see cref="IModel"/>.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelNameAttribute : Attribute
    {
        /// <returns>
        /// Имя объекта <see cref="IModel"/>.
        /// </returns>
        public readonly string Name;

        /// <param name="name">Имя объекта <see cref="IModel"/>.</param>
        public ModelNameAttribute(string name) => Name = name;
        public override string ToString() => Name;
    }
}
