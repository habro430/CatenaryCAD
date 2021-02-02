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
            ComponentPartsDictionary.AddOrUpdate("anchor", new ComponentPart(new IMesh[] { GetOrCreateFromCache("test") }),
                                                (name, value) => value = new ComponentPart(new IMesh[] { GetOrCreateFromCache("test") }));
        }
    }
}
