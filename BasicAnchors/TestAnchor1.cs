﻿using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Attributes;
using System;

namespace BasicAnchors
{
    [Serializable]
    [ModelName("TestAnchor1")]
    [ModelDescription("")]
    public class TestAnchor1 : AbstractAnchor
    {
        public TestAnchor1()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("test") });
            ComponentsDictionary.AddOrUpdate("anchor", tsp, (name, component) => tsp);
        }
    }
}
