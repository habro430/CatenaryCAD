using System;

namespace BasicFoundations
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FoundationAttribute : Attribute
    {
        public string Type { get; private set; }
        public FoundationAttribute(string type) => Type = type;
    }
}
