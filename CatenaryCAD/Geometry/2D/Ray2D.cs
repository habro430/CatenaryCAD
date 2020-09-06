using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 40)]
    public readonly struct Ray2D : IEquatable<Ray2D>, IRay
    {
        [FieldOffset(0)]
        public readonly Point2D Origin;
        [FieldOffset(16)]
        public readonly Vector2D Direction;

        public Ray2D(Point2D origin, Vector2D direction)
        {
            Origin = origin;
            Direction = direction.Normalize();
        }

        #region Functions

        public override bool Equals(object obj) => obj is Ray2D r && this.Equals(r);

        public bool Equals(Ray2D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2D TransformBy(in Matrix2D m) => new Ray2D(Origin.TransformBy(m), Direction.TransformBy(m));

        #endregion
    }
}