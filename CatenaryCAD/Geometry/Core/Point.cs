using CatenaryCAD.Geometry.Core;
using System;
using System.Runtime.CompilerServices;

namespace CatenaryCAD.Geometry
{
    public class Point<T> : IEquatable<T> where T : struct, IParticle<T>
    {
        public T Value = default(T);

        public Point() { }
        public Point(T val)
        {
            if (!(val is XY) || !(val is XYZ)) throw new ArgumentException();

            Value = val;
        }


        public static Point<T> operator +(Point<T> p, Vector<T> v) => new Point<T>(p.Value.Addition(v.Value));
        public static Point<T> operator -(Point<T> p, Vector<T> v) => new Point<T>(p.Value.Subtraction(v.Value));
        public static Vector<T> operator -(Point<T> p1, Point<T> p2) => new Vector<T>(p1.Value.Subtraction(p2.Value));


        public static bool operator ==(Point<T> p1, Point<T> p2) => p1.Equals(p2);
        public static bool operator !=(Point<T> p1, Point<T> p2) => !p1.Equals(p2);


        public bool Equals(T other) => Value.Equals(other);
        public override bool Equals(object obj) => obj is T p && this.Equals(p);

        public override int GetHashCode() => Value.GetHashCode();
    }

}
