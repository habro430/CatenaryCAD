using Multicad.Geometry;
using System;
using System.Threading.Tasks;
using static System.Math;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public struct Vector3D : IEquatable<Vector3D>
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly static Vector3D NaN = new Vector3D(double.NaN, double.NaN, double.NaN);

        public readonly static Vector3D UnitX = new Vector3D(1, 0, 0);
        public readonly static Vector3D UnitY = new Vector3D(0, 1, 0);
        public readonly static Vector3D UnitZ = new Vector3D(0, 0, 1);

        public double Length => Sqrt((X * X) + (Y * Y) + (Z * Z));
        public Vector3D Negated => new Vector3D(-1 * X, -1 * Y, -1 * Z);
        public Vector3D Normalized => new Vector3D(X / Length, Y / Length, Z / Length);

        #region Region operators

        public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector3D operator -(Vector3D v) => v.Negated;
        public static Vector3D operator *(double d, Vector3D v) => new Vector3D(d * v.X, d * v.Y, d * v.Z);
        public static Vector3D operator *(Vector3D v, double d) => new Vector3D(d * v.X, d * v.Y, d * v.Z);
        public static Vector3D operator /(Vector3D v, double d) => new Vector3D(v.X / d, v.Y / d, v.Z / d);

        public static bool operator ==(Vector3D v1, Vector3D v2) => v1.Equals(v2);
        public static bool operator !=(Vector3D v1, Vector3D v2) => !v1.Equals(v2);

        #endregion

        public bool IsNaN() => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);

        public bool Equals(Vector3D other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Abs(other.X - X) < tolerance &&
                   Abs(other.Y - Y) < tolerance &&
                   Abs(other.Z - Z) < tolerance;
        }
        public bool Equals(Vector3D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        public override bool Equals(object obj) => obj is Vector3D v && this.Equals(v);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"Vector3D {{ X:{X}, Y:{Y}, Z:{Round(Z, 3)} }}";
    }

    public static partial class Extensions
    {
        public static Vector3D TransformBy(this Vector3D v, Matrix3D m3d)
        {
            return new Vector3D(v.X * m3d.M11 + v.Y * m3d.M21 + v.Z * m3d.M31 + m3d.M41,
                                v.X * m3d.M12 + v.Y * m3d.M22 + v.Z * m3d.M32 + m3d.M42,
                                v.X * m3d.M13 + v.Y * m3d.M23 + v.Z * m3d.M33 + m3d.M43);
        }

        public static Vector3D[] TransformBy(this Vector3D[] vs, Matrix3D m3d)
        {
            Parallel.For(0, vs.Length, i => vs[i] = vs[i].TransformBy(m3d));
            return vs;
        }

        internal static Vector3d ToNanoCAD(this Vector3D v) => new Vector3d(v.X, v.Y, v.Z);
        internal static Vector3D ToCatenaryCAD(this Vector3d v) => new Vector3D(v.X, v.Y, v.Z);

    }
}
