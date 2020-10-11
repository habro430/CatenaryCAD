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
    public class TestFoundation2 : Foundation
    {
        IShape[] geom = new IShape[] { new Rectangle(new Point2D(), 100, 100) };

        public TestFoundation2()
        {
            var prop = new Property<int>("01_basement_size", "basement_size", "Фундамент", ConfigFlags.None);

            prop.Value = 100;
            prop.Updated += (val) =>
            {
                if (SendMessageToHandler(HandlerMessages.TryModify) ?? true)
                {
                    geom[0] = new Rectangle(new Point2D(), val, val);
                }
            };

            PropertiesSet.Add(prop);
        }

        public override IMesh[] GetGeometryForLayout() => null;
        public override IShape[] GetGeometryForScheme() => geom;
    }
}
