using Multicad.Geometry;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public class Vector<T> : IEquatable<T> where T : struct, IParticle<T>
    {
        public T Value = default(T);
        public double Lenght => Math.Sqrt(Value.DotProduct(Value));

        public Vector() { }
        public Vector(T val) => Value = val;


        public static Vector<T> operator +(Vector<T> p, Vector<T> v) => new Vector<T>(p.Value.Addition(v.Value));
        public static Vector<T> operator -(Vector<T> p, Vector<T> v) => new Vector<T>(p.Value.Subtraction(v.Value));
        public static Vector<T> operator *(Vector<T> p, double v) => new Vector<T>(p.Value.Multiply(v));
        public static Vector<T> operator /(Vector<T> p, double v) => new Vector<T>(p.Value.Division(v));


        public static bool operator ==(Vector<T> p1, Vector<T> p2) => p1.Equals(p2);
        public static bool operator !=(Vector<T> p1, Vector<T> p2) => !p1.Equals(p2);

        public Vector<T> ProjectOn(Vector<T> other) => other * (Value.DotProduct(other.Value) / other.Value.DotProduct(other.Value));

        public bool IsNaN() => Value.IsNaN();

        public Vector<T> TransformBy(Matrix m)
        {
            Value = Value.TransformBy(m);
            return this;
        }

        public bool Equals(T other) => Value.Equals(other);
        public override bool Equals(object obj) => obj is T p && this.Equals(p);

        public override int GetHashCode() => Value.GetHashCode();

    }
    internal static partial class Extensions
    {
        internal static Vector3d ToMCAD(this Vector<XY> p) => new Vector3d(p.Value.X, p.Value.Y, 0);
        internal static Vector3d ToMCAD(this Vector<XYZ> p) => new Vector3d(p.Value.X, p.Value.Y, p.Value.Z);
    }

}
