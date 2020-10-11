using Multicad.Geometry;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public readonly struct Vector2D : IEquatable<Vector2D>
    {
        [FieldOffset(0)]
        public readonly double X;
        [FieldOffset(8)]
        public readonly double Y;

        [FieldOffset(16)]
        public readonly double Length;

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;

            Length = Math.Sqrt((X * X) + (Y * Y));
        }

        #region Operators

        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D p, double s) => new Vector2D(p.X * s, p.Y * s);
        public static Vector2D operator /(Vector2D p, double s) => new Vector2D(p.X / s, p.Y / s);
        public static bool operator ==(Vector2D p1, Vector2D p2) => p1.Equals(p2);
        public static bool operator !=(Vector2D p1, Vector2D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        public override bool Equals(object obj) => obj is Vector2D p && this.Equals(p);

        public bool Equals(Vector2D n) => X.Equals(n.X) && Y.Equals(n.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D TransformBy(in Matrix2D m) => 
            new Vector2D(X * m.M11 + Y * m.M21 + m.M31, X * m.M12 + Y * m.M22 + m.M32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CrossProduct(in Vector2D v2) => (X * v2.Y) - (Y * v2.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DotProduct(in Vector2D v2) => (X * v2.X) + (Y * v2.Y);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D Negate() => new Vector2D(-1 * X, -1 * Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D Normalize() => new Vector2D(X / Length, Y / Length);

    }

    public static partial class Extensions
    {
        internal static Vector3d ToMultiCAD(this in Vector2D v) => new Vector3d(v.X, v.Y, 0);
        internal static Vector2D ToCatenaryCAD_2D(this in Vector3d v) => new Vector2D(v.X, v.Y);
    }
}