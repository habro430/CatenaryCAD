using System;

namespace CatenaryCAD.Models.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public ModelNameAttribute(string name) => Name = name;
        public override string ToString() => Name;
    }
}
