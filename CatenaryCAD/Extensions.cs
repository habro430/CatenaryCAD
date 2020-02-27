using Multicad.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD
{
    public static class Extensions
    {
        public static Vector3d Normalize(this Vector3d vector)
        {
            return vector / vector.Length;
        }
    }
}
