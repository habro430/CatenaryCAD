using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicAnchors
{
    [Serializable]
    [ModelName("TestAnchor2")]
    [ModelDescription("")]
    public class TestAnchor2 : AbstractAnchor
    {
        public TestAnchor2()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("test") });
            ComponentsDictionary.AddOrUpdate("anchor", tsp, (name, component) => tsp);
        }
    }
}
