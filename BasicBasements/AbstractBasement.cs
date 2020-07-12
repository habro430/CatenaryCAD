using CatenaryCAD.Geometry;
using CatenaryCAD.Objects;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;

namespace BasicBasements
{
    [Serializable]
    [CatenaryObject("TestBasement", "")]
    public class AbstractBasement : IBasement
    {
        public event Action Updated;

        public void GetGeometry(out AbstractGeometry<XY>[] xy, out AbstractGeometry<XYZ>[] xyz)
        {
            xy = new AbstractGeometry<XY>[] { new Rectangle(new Point<XY>(), 100, 100) };
            xyz = null;
        }

        public IPart[] GetParts()
        {
            return null;
        }

        public IProperty[] GetProperties()
        {
            return null;
        }
    }
}
