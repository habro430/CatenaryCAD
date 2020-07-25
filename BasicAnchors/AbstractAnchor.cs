using CatenaryCAD;
using CatenaryCAD.Geometry;
using CatenaryCAD.Objects;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Linq;

namespace BasicBasements
{
    [Serializable]
    [CatenaryObject("TestAnchor", "")]
    public class AbstractAnchor : IAnchor
    {
        public event Action Updated;

        AbstractGeometry<XY>[] geom = new AbstractGeometry<XY>[] { new Line(new Point<XY>((0, 0)), new Point<XY>((900, 0))),
                                                                   new Rectangle(new Point<XY>((1000, 0)), 200, 200) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public AbstractAnchor()
        {

        }

        public IPart[] GetParts() => throw new NotImplementedException();
        public IProperty[] GetProperties() => Properties.ToArray();

        public AbstractGeometry<XYZ>[] GetGeometryForLayout() => null;
        public AbstractGeometry<XY>[] GetGeometryForScheme() => geom;
    }
}
