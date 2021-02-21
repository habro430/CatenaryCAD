using BasicBasements;
using CatenaryCAD.ComponentParts;
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
            ComponentPart tsp = new ComponentPart(new IMesh[] { GetOrCreateFromCache("TSP4.5") });
            ComponentPartsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp);
        }
    }
}
