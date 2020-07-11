using System;

namespace CatenaryCAD.Geometry
{
    public interface IParticle<T> : IEquatable<T>
    {
        T Addition(T val);
        T Subtraction(T val);

        T Division(T val);
        T Division(double val);

        T Multiply(T val);
        T Multiply(double val);

        bool IsNaN();

        T TransformBy(Matrix m);
    }
}
