using CatenaryCAD;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Linq;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestAnchor2")]
    [ModelDescription("")]
    public class TestAnchor2 : Anchor
    {
        IShape[] geom = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                       new Rectangle(new Point2D(1000, 0), 200, 200) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public TestAnchor2()
        {

        }

        public override IMesh[] GetGeometryForLayout() => null;
        public override IShape[] GetGeometryForScheme() => geom;
    }
}
