using BasicAnchors;
using CatenaryCAD.ComponentParts;
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
            ComponentPartsDictionary.AddOrUpdate("anchor", new ComponentPart(new IMesh[] { GetOrCreateFromCache("test") }),
                                                (name, value) => value = new ComponentPart(new IMesh[] { GetOrCreateFromCache("test") }));

        }
    }
}
