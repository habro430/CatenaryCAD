using System;

namespace CatenaryCAD.Geometry
{
    public interface IParticle<T> : IEquatable<T>
    {
        T Addition(T val);
        T Subtraction(T val);

        T Division(T val);
        T Division(double scalar);

        T Multiply(T val);
        T Multiply(double scalar);

        double DotProduct(T val);
        T Function(Func<double, double> scalar);

        bool IsNaN();

        T TransformBy(Matrix m);
    }
}
