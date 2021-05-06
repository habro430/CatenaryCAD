using BasicBasements;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСС-4.5")]
    [ModelDescription("Представляет базовую модель фундамента со стаканным креплением стойки")]
    public class TSS45 : TSS
    {
        public TSS45()
        {
            Component tss = new Component(new IMesh[] { GetOrCreateFromCache("tss-4.5") });
            ComponentsDictionary.AddOrUpdate("foundation", tss, (name, component) => tss); 
        }
    }
}
