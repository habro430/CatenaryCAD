using System;

namespace BasicAnchors
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AnchorAttribute : Attribute
    {
        public string Type { get; private set; }
        public AnchorAttribute(string type) => Type = type;
    }
}
