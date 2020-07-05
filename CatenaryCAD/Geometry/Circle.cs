using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Circle : AbstractGeometry
    {
        public Circle(Point3 center, double radius, int resolution)
        {
            Vertices = new Point3[resolution];
            Edges = new (int, int)[resolution];

            for (int i = 0; i < resolution; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / resolution) * radius;
                double y = Math.Sin(2 * Math.PI * i / resolution) * radius;

                Vertices[i] = new Point3(x, y, 0);

                if (i < resolution - 1)
                    Edges[i] = (i, i + 1 );
                else
                    Edges[i] = (i, 0);
            }
        }
    }
}
