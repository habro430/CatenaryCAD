using CatenaryCAD.Geometry.Interfaces;
using System;

namespace CatenaryCAD.Geometry.Meshes
{
    [Serializable]
    public class Face3D
    {
        public Edge3D[] Edges { private set; get; }
        public Face3D(params Edge3D[] edges) => Edges = edges;

        public Face3D TransformBy(in Matrix3D m)
        {
            int count = Edges.Length;

            Edge3D[] new_edges = new Edge3D[count];

            for(int i = 0; i < count; i++)
                new_edges[i] = Edges[i].TransformBy(m);

            return new Face3D(new_edges);
        }
    }
}
