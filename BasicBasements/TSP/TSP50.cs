using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСП-5.0")]
    [ModelDescription("Представляет базовую модель фундамента с анкерным креплением стойки")]
    public class TSP50 : TSP
    {
        public TSP50()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("tsp-5.0") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp);
        }
    }
}
