using System;

namespace CatenaryCAD.Objects.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class NonBrowsableAttribute : Attribute
    {
    }
}
