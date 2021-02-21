using BasicBasements;
using CatenaryCAD.ComponentParts;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Models.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("FoundationSubmersible")]
    [ModelDescription("Представляет базовую модель фундамента со стаканным креплением стойки")]
    public class FoundationSubmersible : AbstractFoundation
    {
        public FoundationSubmersible()
        {
            ComponentPart tsp = new ComponentPart(new IMesh[] { GetOrCreateFromCache("TSP5.0") });
            ComponentPartsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp); 
        }
    }
}
