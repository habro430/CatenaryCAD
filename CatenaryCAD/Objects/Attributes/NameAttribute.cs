using System;

namespace CatenaryCAD.Objects.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public string Name { get; private set; }
        public NameAttribute(string name) => Name = name;

    }
}
