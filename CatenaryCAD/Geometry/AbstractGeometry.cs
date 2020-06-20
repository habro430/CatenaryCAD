using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CatenaryCAD.Geometry.Shapes
{
    public abstract class AbstractGeometry
    {
        public abstract AbstractGeometry Transform(Matrix3D m);
    }
}
