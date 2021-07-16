using Catenary.Models;
using System;

namespace Catenary.Attributes
{
    /// <summary>
    /// Атрибут, указывающий на то что объект <see cref="IModel"/> не виден для <see cref="Catenary"/>.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelNonBrowsableAttribute : Attribute
    {
    }
}
