using CatenaryCAD.Geometry.Interfaces;
using Multicad.Geometry;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public readonly struct Point3D : IEquatable<Point3D>, IPoint
    {
        [FieldOffset(0)]
        public readonly double X;
        [FieldOffset(8)]
        public readonly double Y;
        [FieldOffset(16)]
        public readonly double Z;

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Operators

        public static Point3D operator +(Point3D a, Vector3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point3D operator -(Point3D a, Vector3D b) => new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3D operator -(Point3D a, Point3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Point3D operator *(Point3D p, double s) => new Point3D(p.X * s, p.Y * s, p.Z * s);
        public static Point3D operator /(Point3D p, double s) => new Point3D(p.X / s, p.Y / s, p.Z / s);
        public static bool operator ==(Point3D p1, Point3D p2) => p1.Equals(p2);
        public static bool operator !=(Point3D p1, Point3D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        public override bool Equals(object obj) => obj is Point3D p && this.Equals(p);

        public bool Equals(Point3D n) => X.Equals(n.X) && Y.Equals(n.Y) && Z.Equals(n.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3D TransformBy(in Matrix3D m) => new Point3D(X * m.M11 + Y * m.M21 + Z * m.M31 + m.M41,
                                                              X * m.M12 + Y * m.M22 + Z * m.M32 + m.M42,
                                                              X * m.M13 + Y * m.M23 + Z * m.M33 + m.M43);


        #endregion

    }
    public static partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D VectorTo(this in Point3D p1, in Point3D p2) => p2 - p1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceTo(this in Point3D p1, in Point3D p2) => p1.VectorTo(p2).Length;


        //////////////////////////////////////////////////////////////////////////////////
        internal static Point3d ToMultiCAD(this in Point3D p) => new Point3d(p.X, p.Y, p.Z);
        internal static Point3D ToCatenaryCAD_3D(this in Point3d p) => new Point3D(p.X, p.Y, p.Z);

    }
}
