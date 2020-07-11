using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
