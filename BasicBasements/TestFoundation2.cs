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
    [ModelName("TestFoundation2")]
    [ModelDescription("")]
    public class TestFoundation2 : IFoundation
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

        IShape[] geom = new IShape[] { new Rectangle(new Point2D(), 100, 100) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public TestFoundation2()
        {
            var prop = new Property<int>("01_basement_size", "basement_size", "Фундамент", ConfigFlags.None);

            prop.Value = 100;
            prop.Updated += (val) =>
            {
                geom[0] = new Rectangle(new Point2D(), val, val);

                Updated?.Invoke();
            };

            Properties.Add(prop);
        }

        public IPart[] GetParts() => throw new NotImplementedException();
        public IProperty[] GetProperties() => Properties.ToArray();

        public IMesh[] GetGeometryForLayout() => null;
        public IShape[] GetGeometryForScheme() => geom;
    }
}
