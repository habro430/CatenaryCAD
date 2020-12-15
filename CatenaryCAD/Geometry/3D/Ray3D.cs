using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable, DebuggerDisplay("Origin = {Origin}, Direction = {Direction}")]
    [StructLayout(LayoutKind.Explicit, Size = 56)]
    public readonly struct Ray3D : IEquatable<Ray3D>
    {
        [FieldOffset(0)]
        public readonly Point3D Origin;
        [FieldOffset(24)]
        public readonly Vector3D Direction;

        public Ray3D(Point3D origin, Vector3D direction)
        {
            Origin = origin;
            Direction = direction.GetNormalize();
        }

        #region Functions

        public Point3D[] GetIntersections(IMesh mesh)
        {
            List<Point3D> intesections = new List<Point3D>();

            foreach (var face in mesh.Indices)
            {
                var a = mesh.Vertices[face[0]];
                var b = mesh.Vertices[face[1]];
                var c = mesh.Vertices[face[2]];

                var e1 = b - a;
                var e2 = c - a;
                var P = Direction.CrossProduct(e2);
                var det = e1.DotProduct(P);

                if (det > -double.Epsilon && det < double.Epsilon) 
                    continue;

                var inv = 1d / det;

                var T = Origin - a;
                var u = T.DotProduct(P) * inv;

                if (u < 0f || u > 1f) 
                    continue;

                var Q = T.CrossProduct(e1);
                var v = (Direction.DotProduct(Q * inv));

                if (v < 0f || u + v > 1f) 
                    continue;

                var t = e2.DotProduct(Q) * inv;
                if (t > double.Epsilon)
                    intesections.Add(Origin + Direction * t);
            }

            return intesections.ToArray();
        }


        public override bool Equals(object obj) => obj is Ray3D r && this.Equals(r);

        public bool Equals(Ray3D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray3D TransformBy(in Matrix3D m) => new Ray3D(Origin.TransformBy(m), Direction.TransformBy(m));
    }

    #endregion
}
