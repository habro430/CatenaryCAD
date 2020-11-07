using BasicBasements;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestFoundation1")]
    [ModelDescription("")]
    public class TestFoundation1 : AbstractFoundation
    {
        public TestFoundation1()
        {
            Geometry3D = new IMesh[] { GetOrCreateFromCache("test") };
        }
    }
}
