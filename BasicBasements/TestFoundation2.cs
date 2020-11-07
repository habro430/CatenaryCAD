using BasicBasements;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestFoundation2")]
    [ModelDescription("")]
    public class TestFoundation2 : AbstractFoundation
    {
        public TestFoundation2()
        {
            Geometry3D = new IMesh[] { GetOrCreateFromCache("test") };
        }
    }
}
