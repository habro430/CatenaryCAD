using System;

namespace BasicMasts
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MastAttribute : Attribute
    {
        public string Type { get; private set; }
        public MastAttribute(string type) => Type = type;
    }
}
