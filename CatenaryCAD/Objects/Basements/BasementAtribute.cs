using System;

namespace CatenaryCAD.Objects.Basements
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BasementAtribute : Attribute
    {
        public string Type { get; private set; }

        public BasementAtribute(string type)
        {
            Type = type;
        }
    }
}
