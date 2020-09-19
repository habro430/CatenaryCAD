using System;

namespace CatenaryCAD.Models.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }
        public ModelDescriptionAttribute(string description) => Description = description;
    }
}
