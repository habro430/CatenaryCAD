using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public struct XYZ : IParticle<XYZ>
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public XYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Deconstruct(out double x, out double y, out double z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"X:{X}, Y:{Y}, Z:{Z}";

        public bool Equals(XYZ other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public override bool Equals(object obj) => obj is XYZ p && this.Equals(p);

        public XYZ Addition(XYZ val) => this + val;
        public XYZ Subtraction(XYZ val) => this - val;

        public XYZ Division(XYZ val) => this / val;
        public XYZ Division(double scalar) => this / new XYZ(scalar, scalar, scalar);

        public XYZ Multiply(XYZ val) => this * val;
        public XYZ Multiply(double scalar) => this * new XYZ(scalar, scalar, scalar);

        public double DotProduct(XYZ other) => (this.X * other.X) + (this.Y * other.Y) + (this.Z * other.Z);

        public XYZ Function(Func<double, double> scalar) => new XYZ(scalar(X), scalar(Y), scalar(Z));

        public bool IsNaN() => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);

        public XYZ TransformBy(Matrix m)
        {
            throw new NotImplementedException();
        }


        public static XYZ operator +(XYZ p1, XYZ p2) => new XYZ(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        public static XYZ operator -(XYZ p1, XYZ p2) => new XYZ(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        public static XYZ operator *(XYZ p1, XYZ p2) => new XYZ(p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
        public static XYZ operator /(XYZ p1, XYZ p2) => new XYZ(p1.X / p2.X, p1.Y / p2.Y, p1.Z / p2.Z);

        public static bool operator ==(XYZ p1, XYZ p2) => p1.Equals(p2);
        public static bool operator !=(XYZ p1, XYZ p2) => !p1.Equals(p2);


        public static implicit operator XYZ((double x, double y, double z) value) => new XYZ(value.x, value.y, value.z);
        public static explicit operator (double x, double y, double z)(XYZ value) => (value.X, value.Y, value.Z);

    }

}
