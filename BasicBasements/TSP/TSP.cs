using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using System;

namespace BasicFoundations
{
    [Serializable]
    //[ModelName("FoundationDirect")]
    //[ModelDescription("Представляет базовую модель фундамента с анкерным креплением стойки")]
    public abstract class TSP : AbstractFoundation
    {
        //public TSP()
        //{
        //    Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("direct") });
        //    ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp);
        //}
    }
}
