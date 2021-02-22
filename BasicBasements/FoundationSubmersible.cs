using BasicBasements;
using CatenaryCAD.Components;
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
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("TSP5.0") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp); 
        }
    }
}
