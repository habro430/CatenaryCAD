using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;

using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    public interface IObject
    {
        public abstract event Action Updated;
        public abstract Object[] GetParts();
        public abstract IProperty[] GetProperties();
        public abstract AbstractGeometry[] GetGeometry(GeometryType type);
    }
}
