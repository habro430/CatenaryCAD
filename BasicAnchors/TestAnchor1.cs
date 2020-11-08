using BasicAnchors;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestAnchor1")]
    [ModelDescription("")]
    public class TestAnchor1 : AbstractAnchor
    {
        public TestAnchor1()
        {
            Geometry3D = new IMesh[] { GetOrCreateFromCache("test") };
        }
    }
}
