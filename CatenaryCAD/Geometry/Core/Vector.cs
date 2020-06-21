using Multicad.Geometry;
using System;
using System.Threading.Tasks;
using static System.Math;

namespace CatenaryCAD.Geometry.Core
{
    [Serializable]
    public struct Vector : IEquatable<Vector>
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly static Vector NaN = new Vector(double.NaN, double.NaN, double.NaN);

        public readonly static Vector UnitX = new Vector(1, 0, 0);
        public readonly static Vector UnitY = new Vector(0, 1, 0);
        public readonly static Vector UnitZ = new Vector(0, 0, 1);

        public double Length => Sqrt((X * X) + (Y * Y) + (Z * Z));
        public Vector Negated => new Vector(-1 * X, -1 * Y, -1 * Z);
        public Vector Normalized => new Vector(X / Length, Y / Length, Z / Length);

        #region Region operators

        public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector operator -(Vector v) => v.Negated;
        public static Vector operator *(double d, Vector v) => new Vector(d * v.X, d * v.Y, d * v.Z);
        public static Vector operator *(Vector v, double d) => new Vector(d * v.X, d * v.Y, d * v.Z);
        public static Vector operator /(Vector v, double d) => new Vector(v.X / d, v.Y / d, v.Z / d);

        public static bool operator ==(Vector v1, Vector v2) => v1.Equals(v2);
        public static bool operator !=(Vector v1, Vector v2) => !v1.Equals(v2);

        #endregion

        public bool IsNaN() => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);

        public bool Equals(Vector other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Abs(other.X - X) < tolerance &&
                   Abs(other.Y - Y) < tolerance &&
                   Abs(other.Z - Z) < tolerance;
        }
        public bool Equals(Vector other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        public override bool Equals(object obj) => obj is Vector v && this.Equals(v);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"Vector3D {{ X:{X}, Y:{Y}, Z:{Round(Z, 3)} }}";
    }

    public static partial class Extensions
    {
        public static Vector TransformBy(this Vector v, Matrix m3d)
        {
            return new Vector(v.X * m3d.M11 + v.Y * m3d.M21 + v.Z * m3d.M31 + m3d.M41,
                                v.X * m3d.M12 + v.Y * m3d.M22 + v.Z * m3d.M32 + m3d.M42,
                                v.X * m3d.M13 + v.Y * m3d.M23 + v.Z * m3d.M33 + m3d.M43);
        }

        public static Vector[] TransformBy(this Vector[] vs, Matrix m3d)
        {
            Parallel.For(0, vs.Length, i => vs[i] = vs[i].TransformBy(m3d));
            return vs;
        }

        internal static Vector3d ToNanoCAD(this Vector v) => new Vector3d(v.X, v.Y, v.Z);
        internal static Vector ToCatenaryCAD(this Vector3d v) => new Vector(v.X, v.Y, v.Z);

    }
}
