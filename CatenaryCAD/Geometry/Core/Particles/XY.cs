using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CatenaryCAD.Geometry
{
    public struct XY : IParticle<XY>
    {
        public double X;
        public double Y;

        public XY(double x, double y)
        {
            X = x;
            Y = y;
        }
        public void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"X:{X}, Y:{Y}";

        public bool Equals(XY other) => X.Equals(other.X) && Y.Equals(other.Y);
        public override bool Equals(object obj) => obj is XY p && this.Equals(p);

        public XY Addition(XY val) => this + val;
        public XY Subtraction(XY val) => this - val;

        public XY Division(XY val) => this / val;
        public XY Division(double val) => this / new XY(val, val);

        public XY Multiply(XY val) => this * val;
        public XY Multiply(double val) => this * new XY(val, val);

        public static XY operator +(XY p1, XY p2) => new XY(p1.X + p2.X, p1.Y + p2.Y);
        public static XY operator -(XY p1, XY p2) => new XY(p1.X - p2.X, p1.Y - p2.Y);
        public static XY operator *(XY p1, XY p2) => new XY(p1.X * p2.X, p1.Y * p2.Y);
        public static XY operator /(XY p1, XY p2) => new XY(p1.X / p2.X, p1.Y / p2.Y);

        public static bool operator ==(XY p1, XY p2) => p1.Equals(p2);
        public static bool operator !=(XY p1, XY p2) => !p1.Equals(p2);


        public static implicit operator XY((double x, double y) value) => new XY(value.x, value.y);
        public static explicit operator (double x, double y)(XY value) => (value.X, value.Y);

    }

}
