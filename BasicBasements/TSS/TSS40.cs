using BasicBasements;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Attributes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСС-4.0")]
    [ModelDescription("Представляет базовую модель фундамента со стаканным креплением стойки")]
    public class TSS40 : TSS
    {
        public TSS40()
        {
            Component tss = new Component(new IMesh[] { GetOrCreateFromCache("tss-4.0") });
            ComponentsDictionary.AddOrUpdate("foundation", tss, (name, component) => tss); 
        }
    }
}
