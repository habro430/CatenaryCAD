using CatenaryCAD.Objects.Masts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
