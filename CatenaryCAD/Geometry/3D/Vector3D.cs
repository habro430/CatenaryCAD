using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public readonly struct Vector3D : IEquatable<Vector3D>, IVector
    {
        [FieldOffset(0)]
        public readonly double X;
        [FieldOffset(8)]
        public readonly double Y;
        [FieldOffset(16)]
        public readonly double Z;

        [FieldOffset(24)]
        public readonly double Length;

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;

            Length = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        #region Static members

        public static ref readonly Vector3D AxisX => ref axisx;
        private static readonly Vector3D axisx = new Vector3D(1, 0, 0);

        public static ref readonly Vector3D AxisY => ref axisy;
        private static readonly Vector3D axisy = new Vector3D(0, 1, 0);

        public static ref readonly Vector3D AxisZ => ref axisz;
        private static readonly Vector3D axisz = new Vector3D(0, 0, 1);

        #endregion

        #region Opearators

        public static Vector3D operator +(Vector3D a, Vector3D b) => new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        
        public static Vector3D operator -(Vector3D a, Vector3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        
        public static Vector3D operator *(Vector3D p, double s) => new Vector3D(p.X * s, p.Y * s, p.Z * s);
        
        public static Vector3D operator /(Vector3D p, double s) => new Vector3D(p.X / s, p.Y / s, p.Z / s);
        
        public static bool operator ==(Vector3D p1, Vector3D p2) => p1.Equals(p2);
        
        public static bool operator !=(Vector3D p1, Vector3D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        public override bool Equals(object obj) => obj is Vector3D p && this.Equals(p);

        public bool Equals(Vector3D n) => X.Equals(n.X) && Y.Equals(n.Y) && Z.Equals(n.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D TransformBy(in Matrix3D m) => 
            new Vector3D(X * m.M11 + Y * m.M12 + Z * m.M13, X * m.M21 + Y * m.M22 + Z * m.M23, X * m.M31 + Y * m.M32 + Z * m.M33);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D CrossProduct(in Vector3D v2) => 
            new Vector3D((Y * v2.Z) - (Z * v2.Y), (Z * v2.X) - (X * v2.Z), (X * v2.Y) - (Y * v2.X));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DotProduct(in Vector3D v2) => 
            (X * v2.X) + (Y * v2.Y) + (Z * v2.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D Negate() => new Vector3D(-1 * X, -1 * Y, -1 * Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D Normalize() => new Vector3D(X / Length, Y / Length, Z / Length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double AngleTo(in Vector3D destination, in Vector3D refrence) => 
            Math.Atan2(refrence.DotProduct(CrossProduct(destination)), DotProduct(destination));

        #endregion

    }

    public static partial class Extensions
    {
        internal static Vector3d ToMultiCAD(this in Vector3D v) => new Vector3d(v.X, v.Y, v.Z);
        internal static Vector3D ToCatenaryCAD_3D(this in Vector3d v) => new Vector3D(v.X, v.Y, v.Z);
    }
}
