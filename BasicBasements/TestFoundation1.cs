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
using System.Reflection;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestFoundation1")]
    [ModelDescription("")]
    public class TestFoundation1 : Foundation
    {
        IShape[] geom = new IShape[] { new Rectangle(new Point2D(), 100, 100) };

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        public TestFoundation1()
        {
            var prop = new Property<int>("01_basement_size", "basement_size", "Фундамент", ConfigFlags.None);

            prop.Value = 100;
            prop.Updated += (val) =>
            {
                
                if (TryModifyModel())
                {
                    geom[0] = new Rectangle(new Point2D(), val, val);
                }
            };

            Properties.Add(prop);
        }

        public override IPart[] GetParts() => throw new NotImplementedException();
        public override IProperty[] GetProperties() => Properties.ToArray();

        public override IMesh[] GetGeometryForLayout() => null;
        public override IShape[] GetGeometryForScheme() => geom;
    }
}
