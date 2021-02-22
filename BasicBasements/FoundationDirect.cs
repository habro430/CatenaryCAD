using BasicBasements;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("FoundationDirect")]
    [ModelDescription("Представляет базовую модель фундамента с анкерным креплением стойки")]
    public class FoundationDirect : AbstractFoundation
    {
        public FoundationDirect()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("TSP4.5") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp);
        }
    }
}
