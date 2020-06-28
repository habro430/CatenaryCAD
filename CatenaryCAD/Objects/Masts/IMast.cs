using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;
using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects.Masts
{
    public interface IMast
    {
        public abstract event Action Updated;

        public abstract AbstractProperty[] GetProperties();
        public abstract AbstractGeometry[] GetGeometry(ViewType type);
    }
}
