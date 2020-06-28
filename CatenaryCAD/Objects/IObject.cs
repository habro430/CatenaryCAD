using CatenaryCAD.Geometry;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    public interface IObject
    {
        public abstract event Action Updated;
        public abstract IPart[] GetParts();
        public abstract IProperty[] GetProperties();
        public abstract AbstractGeometry[] GetGeometry(GeometryType type);
    }
}
