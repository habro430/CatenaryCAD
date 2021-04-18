using CatenaryCAD.Models;
using System;

namespace CatenaryCAD.Attributes
{
    /// <summary>
    /// Атрибут, указывающий на то что объект <see cref="IModel"/> не виден для <see cref="CatenaryCAD"/>.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelNonBrowsableAttribute : Attribute
    {
    }
}
