using System;

namespace CatenaryCAD.Objects.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; private set; }
        public DescriptionAttribute(string description) => Description = description;
    }
}
