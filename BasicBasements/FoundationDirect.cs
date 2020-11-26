using BasicBasements;
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
            Geometry3D = new IMesh[] { GetOrCreateFromCache("TSP4.5") };
        }
    }
}
