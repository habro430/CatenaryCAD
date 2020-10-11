﻿using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
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

        #region Static members

        public static ref readonly Point3D Origin => ref origin;
        private static readonly Point3D origin = new Point3D(0, 0, 0);

        #endregion

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
        public Point3D TransformBy(Matrix3D m) => 
            new Point3D(X * m.M11 + Y * m.M12 + Z * m.M13 + m.M14, X * m.M21 + Y * m.M22 + Z * m.M23 + m.M24, X * m.M31 + Y * m.M32 + Z * m.M33 + m.M34);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D VectorTo(in Point3D p2) => p2 - this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DistanceTo(in Point3D p2) => VectorTo(p2).Length;

        #endregion

    }
    public static partial class Extensions
    {
        internal static Point3d ToMultiCAD(this in Point3D p) => new Point3d(p.X, p.Y, p.Z);
        internal static Point3D ToCatenaryCAD_3D(this in Point3d p) => new Point3D(p.X, p.Y, p.Z);
    }
}
