using CatenaryCAD.Geometry.Interfaces;
using System;

namespace CatenaryCAD.Geometry.Meshes
{
    [Serializable]
    public class Edge3D
    {
        public readonly Point3D First;
        public readonly Point3D Second;

        public Edge3D(Point3D first, Point3D second)
        {
            First = first;
            Second = second;
        }

        public Edge3D TransformBy(in Matrix3D m) => new Edge3D(First.TransformBy(m), Second.TransformBy(m));
    }
}
