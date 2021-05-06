using CatenaryCAD.Attributes;
using CatenaryCAD.Components;
using CatenaryCAD.Geometry.Meshes;
using System;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСС-5.0")]
    [ModelDescription("Представляет базовую модель фундамента со стаканным креплением стойки")]
    public class TSS50 : TSS
    {
        public TSS50()
        {
            Component tss = new Component(new IMesh[] { GetOrCreateFromCache("tss-5.0") });
            ComponentsDictionary.AddOrUpdate("foundation", tss, (name, component) => tss); 
        }
    }
}
