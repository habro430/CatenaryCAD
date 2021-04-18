using BasicBasements;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("FoundationSubmersible1")]
    [ModelDescription("Представляет базовую модель фундамента со стаканным креплением стойки")]
    public class FoundationSubmersibleTest1 : FoundationSubmersible
    {
        public FoundationSubmersibleTest1()
        {
            Component tsp = new Component(new IMesh[] { GetOrCreateFromCache("TSP5.0") });
            ComponentsDictionary.AddOrUpdate("foundation", tsp, (name, component) => tsp); 
        }
    }
}
