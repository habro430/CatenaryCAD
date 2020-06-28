using System;

namespace CatenaryCAD.Objects
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class CatenaryObjectAttribute : Attribute
    {
        public string Type { get; private set; }
        public string Description { get; private set; }
        public CatenaryObjectAttribute(string type, string description)
        {
            Type = type;
            Description = description;
        }
    }
}
