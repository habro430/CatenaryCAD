﻿using Multicad.Geometry;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Math;

namespace CatenaryCAD.Geometry.Core
{
    [Serializable]
    public struct Vector3 : IEquatable<Vector3>
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly static Vector3 NaN = new Vector3(double.NaN, double.NaN, double.NaN);

        public readonly static Vector3 UnitX = new Vector3(1, 0, 0);
        public readonly static Vector3 UnitY = new Vector3(0, 1, 0);
        public readonly static Vector3 UnitZ = new Vector3(0, 0, 1);

        public double Length => Sqrt((X * X) + (Y * Y) + (Z * Z));
        public Vector3 Negated => new Vector3(-1 * X, -1 * Y, -1 * Z);
        public Vector3 Normalize => new Vector3(X / Length, Y / Length, Z / Length);

        #region Region operators

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector3 operator -(Vector3 v) => v.Negated;
        public static Vector3 operator *(double d, Vector3 v) => new Vector3(d * v.X, d * v.Y, d * v.Z);
        public static Vector3 operator *(Vector3 v, double d) => new Vector3(d * v.X, d * v.Y, d * v.Z);
        public static Vector3 operator /(Vector3 v, double d) => new Vector3(v.X / d, v.Y / d, v.Z / d);

        public static bool operator ==(Vector3 v1, Vector3 v2) => v1.Equals(v2);
        public static bool operator !=(Vector3 v1, Vector3 v2) => !v1.Equals(v2);

        #endregion

        public bool IsNaN() => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);

        public bool Equals(Vector3 other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Abs(other.X - X) < tolerance &&
                   Abs(other.Y - Y) < tolerance &&
                   Abs(other.Z - Z) < tolerance;
        }
        public bool Equals(Vector3 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        public override bool Equals(object obj) => obj is Vector3 v && this.Equals(v);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"Vector3D {{ X:{X}, Y:{Y}, Z:{Round(Z, 3)} }}";
    }

    public static partial class Extensions
    {
        public static Vector3 TransformBy(this Vector3 v, Matrix3 m3d)
        {
            return new Vector3(v.X * m3d.M11 + v.Y * m3d.M21 + v.Z * m3d.M31 + m3d.M41,
                              v.X * m3d.M12 + v.Y * m3d.M22 + v.Z * m3d.M32 + m3d.M42,
                              v.X * m3d.M13 + v.Y * m3d.M23 + v.Z * m3d.M33 + m3d.M43);
        }

        public static Vector3[] TransformBy(this Vector3[] vs, Matrix3 m3d)
        {
            Parallel.For(0, vs.Length, i => vs[i] = vs[i].TransformBy(m3d));
            return vs;
        }

        internal static Vector3d ToMCAD(this Vector3 v) => new Vector3d(v.X, v.Y, v.Z);
        internal static Vector3d[] ToMCAD(this Vector3[] vs)
        {
            int count = vs.Count();
            Vector3d[] vector3Ds = new Vector3d[count];

            Parallel.For(0, count, i => vector3Ds[i] = vs[i].ToMCAD());

            return vector3Ds;
        }
        internal static Vector3 ToCCAD(this Vector3d v) => new Vector3(v.X, v.Y, v.Z);

    }
}
