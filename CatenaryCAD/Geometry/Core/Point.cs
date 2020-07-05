using Multicad.Geometry;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Math;

namespace CatenaryCAD.Geometry.Core
{
    [Serializable]
    public struct Point : IEquatable<Point>
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly static Point NaN = new Point(double.NaN, double.NaN, double.NaN);

        #region Region operators

        public static Point operator +(Point p, Vector v) => new Point(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        public static Point operator -(Point p, Vector v) => new Point(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        public static Vector operator -(Point p1, Point p2) => new Vector(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

        public static bool operator ==(Point p1, Point p2) => p1.Equals(p2);
        public static bool operator !=(Point p1, Point p2) => !p1.Equals(p2);

        #endregion

        public bool IsNaN() => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);

        public bool Equals(Point other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Abs(other.X - X) < tolerance &&
                   Abs(other.Y - Y) < tolerance &&
                   Abs(other.Z - Z) < tolerance;
        }
        public bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public override bool Equals(object obj) => obj is Point p && this.Equals(p);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"Point3D {{ X:{X}, Y:{Y}, Z:{Z} }}";
    }

    public static partial class Extensions
    {
        public static Point TransformBy(this Point p, Matrix m3d)
        {
            return new Point(p.X * m3d.M11 + p.Y * m3d.M21 + p.Z * m3d.M31 + m3d.M41,
                               p.X * m3d.M12 + p.Y * m3d.M22 + p.Z * m3d.M32 + m3d.M42,
                               p.X * m3d.M13 + p.Y * m3d.M23 + p.Z * m3d.M33 + m3d.M43);
        }

        public static Point[] TransformBy(this Point[] ps, Matrix m3d)
        {
            Parallel.For(0, ps.Length, i => ps[i] = ps[i].TransformBy(m3d));
            return ps;
        }

        public static Vector GetVectorTo(this Point p1, Point p2) => p2 - p1;
        public static double GetDistanceTo(this Point p1, Point p2) => p1.GetVectorTo(p2).Length;

        internal static Point3d ToMCAD(this Point p) => new Point3d(p.X, p.Y, p.Z);
        internal static Point3d[] ToMCAD(this Point[] ps)
        {
            int count = ps.Count();

            Point3d[] point3Ds = new Point3d[count];

            for(int i =0; i< count; i++)
                point3Ds[i] = ps[i].ToMCAD();

            return point3Ds;
        }

        internal static Point ToCCAD(this Point3d p) => new Point(p.X, p.Y, p.Z);

    }
}
