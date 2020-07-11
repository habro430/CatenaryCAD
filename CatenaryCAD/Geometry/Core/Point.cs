using Multicad.Geometry;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public class Point<T> : IEquatable<T> where T : struct, IParticle<T>
    {
        public T Value = default(T);

        public Point() { }
        public Point(T val) => Value = val;


        public static Point<T> operator +(Point<T> p, Vector<T> v) => new Point<T>(p.Value.Addition(v.Value));
        public static Point<T> operator -(Point<T> p, Vector<T> v) => new Point<T>(p.Value.Subtraction(v.Value));
        public static Vector<T> operator -(Point<T> p1, Point<T> p2) => new Vector<T>(p1.Value.Subtraction(p2.Value));


        public static bool operator ==(Point<T> p1, Point<T> p2) => p1.Equals(p2);
        public static bool operator !=(Point<T> p1, Point<T> p2) => !p1.Equals(p2);

        public Vector<T> GetVectorTo(Point<T> p) => p - this;
        public bool IsNaN() => Value.IsNaN();


        public bool Equals(T other) => Value.Equals(other);
        public override bool Equals(object obj) => obj is T p && this.Equals(p);

        public override int GetHashCode() => Value.GetHashCode();
    }

    internal static partial class Extensions
    {
        internal static Point3d ToMCAD(this Point<XY> p) => new Point3d(p.Value.X, p.Value.Y, 0);
        internal static Point3d ToMCAD(this Point<XYZ> p) => new Point3d(p.Value.X, p.Value.Y, p.Value.Z);
    }
}
