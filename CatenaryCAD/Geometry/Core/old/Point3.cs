using Multicad.Geometry;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Math;

namespace CatenaryCAD.Geometry.Core
{
    [Serializable]
    public struct Point3 : IEquatable<Point3>
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Point3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly static Point3 NaN = new Point3(double.NaN, double.NaN, double.NaN);

        #region Region operators

        public static Point3 operator +(Point3 p, Vector3 v) => new Point3(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        public static Point3 operator -(Point3 p, Vector3 v) => new Point3(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        public static Vector3 operator -(Point3 p1, Point3 p2) => new Vector3(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

        public static bool operator ==(Point3 p1, Point3 p2) => p1.Equals(p2);
        public static bool operator !=(Point3 p1, Point3 p2) => !p1.Equals(p2);

        #endregion

        public bool IsNaN() => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);

        public bool Equals(Point3 other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Abs(other.X - X) < tolerance &&
                   Abs(other.Y - Y) < tolerance &&
                   Abs(other.Z - Z) < tolerance;
        }
        public bool Equals(Point3 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public override bool Equals(object obj) => obj is Point3 p && this.Equals(p);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"Point3D {{ X:{X}, Y:{Y}, Z:{Z} }}";
    }

    public static partial class Extensions
    {
        public static Point3 TransformBy(this Point3 p, Matrix3 m3d)
        {
            return new Point3(p.X * m3d.M11 + p.Y * m3d.M21 + p.Z * m3d.M31 + m3d.M41,
                             p.X * m3d.M12 + p.Y * m3d.M22 + p.Z * m3d.M32 + m3d.M42,
                             p.X * m3d.M13 + p.Y * m3d.M23 + p.Z * m3d.M33 + m3d.M43);
        }

        public static Point3[] TransformBy(this Point3[] ps, Matrix3 m3d)
        {
            Parallel.For(0, ps.Length, i => ps[i] = ps[i].TransformBy(m3d));
            return ps;
        }

        public static Vector3 GetVectorTo(this Point3 p1, Point3 p2) => p2 - p1;
        public static double GetDistanceTo(this Point3 p1, Point3 p2) => p1.GetVectorTo(p2).Length;

        internal static Point3d ToMCAD(this Point3 p) => new Point3d(p.X, p.Y, p.Z);
        internal static Point3d[] ToMCAD(this Point3[] ps)
        {
            int count = ps.Count();
            Point3d[] point3Ds = new Point3d[count];

            Parallel.For(0, count, i => point3Ds[i] = ps[i].ToMCAD());

            return point3Ds;
        }

        internal static Point3 ToCCAD(this Point3d p) => new Point3(p.X, p.Y, p.Z);

    }
}
