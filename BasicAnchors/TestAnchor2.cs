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
    public class TestAnchor2 : IAnchor
    {
        public event Action Updated;

        private Point3D position;
        private Vector3D direction;

        public Point3D Position
        {
            get => position;
            set
            {
                position = value;
                Updated?.Invoke();
            }
        }
        public Vector3D Direction
        {
            get => direction;
            set
            {
                direction = value.Normalize();
                Updated?.Invoke();
            }
        }

        IShape[] geom = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                       new Rectangle(new Point2D(1000, 0), 200, 200) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public TestAnchor2()
        {

        }

        public IPart[] GetParts() => throw new NotImplementedException();
        public IProperty[] GetProperties() => Properties.ToArray();

        public IMesh[] GetGeometryForLayout() => null;
        public IShape[] GetGeometryForScheme() => geom;
    }
}
