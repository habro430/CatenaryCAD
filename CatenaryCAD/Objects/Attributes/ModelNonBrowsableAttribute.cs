using System;

namespace CatenaryCAD.Models.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelNonBrowsableAttribute : Attribute
    {
    }
}
