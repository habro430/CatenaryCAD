using BasicBasements;
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
            Geometry3D = new IMesh[] { GetOrCreateFromCache("TSP5.0") };
        }
    }
}
