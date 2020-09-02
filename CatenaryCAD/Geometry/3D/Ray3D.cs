using CatenaryCAD.Geometry.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 56)]
    public readonly struct Ray3D : IEquatable<Ray3D>, IRay
    {
        [FieldOffset(0)]
        public readonly Point3D Origin;
        [FieldOffset(24)]
        public readonly Vector3D Direction;

        public Ray3D(Point3D origin, Vector3D direction)
        {
            Origin = origin;
            Direction = direction.Normalize();
        }

        #region Functions

        public override bool Equals(object obj) => obj is Ray3D r && this.Equals(r);

        public bool Equals(Ray3D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray3D TransformBy(in Matrix3D m) => new Ray3D(Origin.TransformBy(m), Direction.TransformBy(m));
    }

    #endregion
}
