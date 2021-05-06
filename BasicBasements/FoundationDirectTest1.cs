﻿using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("FoundationDirect2")]
    [ModelDescription("Представляет базовую модель фундамента с анкерным креплением стойки")]
    public class FoundationDirectTest1 : FoundationDirect
    {
        public FoundationDirectTest1()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("direct") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp);
        }
    }
}
