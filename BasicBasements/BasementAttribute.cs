using System;

namespace BasicBasements
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BasementAttribute : Attribute
    {
        public string Type { get; private set; }
        public BasementAttribute(string type) => Type = type;
    }
}
