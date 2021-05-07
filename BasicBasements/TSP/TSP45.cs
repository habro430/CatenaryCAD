using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСП-4.5")]
    [ModelDescription("Представляет базовую модель фундамента с анкерным креплением стойки")]
    public class TSP45 : TSP
    {
        public TSP45()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("tsp-4.5") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp);
        }
    }
}
