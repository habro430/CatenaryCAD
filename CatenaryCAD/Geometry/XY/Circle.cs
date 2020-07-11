using CatenaryCAD.Geometry.Core;
using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Circle : AbstractGeometry<XY>
    {
        public Circle(Point<XY> center, double radius, int resolution)
        {
            Vertices = new Point<XY>[resolution];
            Edges = new (Point<XY>, Point<XY>)[resolution];

            for (int i = 0; i < resolution; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / resolution) * radius;
                double y = Math.Sin(2 * Math.PI * i / resolution) * radius;

                Vertices[i] = new Point<XY>((x, y));
            }

            for (int i = 0; i < resolution; i++)
            {;
                if (i < resolution - 1)
                    Edges[i] = (Vertices[i], Vertices[i + 1]);
                else
                    Edges[i] = (Vertices[i], Vertices[0]);
            }

        }
    }
}
