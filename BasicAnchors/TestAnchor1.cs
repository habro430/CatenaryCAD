using BasicAnchors;
using CatenaryCAD.ComponentParts;
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
            ComponentPart tsp = new ComponentPart(new IMesh[] { GetOrCreateFromCache("test") });
            ComponentPartsDictionary.AddOrUpdate("anchor", tsp, (name, component) => tsp);
        }
    }
}
