﻿using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}")]
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public readonly struct Point2D : IEquatable<Point2D>, IPoint
    {
        [FieldOffset(0)]
        public readonly double X;
        [FieldOffset(8)]
        public readonly double Y;

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Operators

        public static Point2D operator +(Point2D a, Vector2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
        public static Point2D operator -(Point2D a, Vector2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator -(Point2D a, Point2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Point2D operator *(Point2D p, double s) => new Point2D(p.X * s, p.Y * s);
        public static Point2D operator /(Point2D p, double s) => new Point2D(p.X / s, p.Y / s);
        public static bool operator ==(Point2D p1, Point2D p2) => p1.Equals(p2);
        public static bool operator !=(Point2D p1, Point2D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        public override bool Equals(object obj) => obj is Point2D p && this.Equals(p);

        public bool Equals(Point2D n) => X.Equals(n.X) && Y.Equals(n.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2D TransformBy(in Matrix2D m) => 
            new Point2D(X * m.M11 + Y * m.M21 + m.M31, X * m.M12 + Y * m.M22 + m.M32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D VectorTo(in Point2D p2) => p2 - this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DistanceTo(in Point2D p2) => VectorTo(p2).Length;

        #endregion

    }
    public static partial class Extensions
    {
        internal static Point3d ToMultiCAD(this in Point2D p) => new Point3d(p.X, p.Y, 0);
        internal static Point2D ToCatenaryCAD_2D(this in Point3d p) => new Point2D(p.X, p.Y);
    }
}