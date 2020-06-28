using System;

namespace CatenaryCAD.Objects
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class CatenaryObjectAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public CatenaryObjectAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
