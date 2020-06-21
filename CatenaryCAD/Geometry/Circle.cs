using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Circle : AbstractGeometry
    {
        public Circle(Point center, double radius, int resolution)
        {
            Points = new Point[resolution];
            Edges = new (int, int)[resolution];

            for (int i = 0; i < resolution; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / resolution) * radius;
                double y = Math.Sin(2 * Math.PI * i / resolution) * radius;

                Points[i] = new Point(x, y, 0);

                if (i < resolution - 1)
                    Edges[i] = (i, i + 1 );
                else
                    Edges[i] = (i, 0);
            }
        }
    }
}
