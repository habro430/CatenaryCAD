using BasicAnchors;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestAnchor2")]
    [ModelDescription("")]
    public class TestAnchor2 : AbstractAnchor
    {
        public TestAnchor2()
        {
            Geometry3D = new IMesh[] { GetOrCreateFromCache("test") };
        }
    }
}
