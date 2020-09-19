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
    [ModelName("TestFoundation1")]
    [ModelDescription("")]
    public class TestFoundation1 : IFoundation
    {
        public event Action Updated;

        IShape[] geom = new IShape[] { new Rectangle(new Point2D(), 100, 100) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public TestFoundation1()
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
