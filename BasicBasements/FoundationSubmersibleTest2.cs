using BasicBasements;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("FoundationSubmersible2")]
    [ModelDescription("Представляет базовую модель фундамента со стаканным креплением стойки")]
    public class FoundationSubmersibleTest2 : FoundationSubmersible
    {
        public FoundationSubmersibleTest2()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("TSP5.0") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp); 
        }
    }
}
