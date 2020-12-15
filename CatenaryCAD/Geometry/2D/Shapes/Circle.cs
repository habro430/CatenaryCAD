using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Circle : AbstractShape
    {
        public Circle(in Point2D center, double radius, int resolution)
        {
            Vertices = new Point2D[resolution];
            Indices = new int[resolution][];

            for (int i = 0; i < resolution; i++)
            {
                double x = Math.Cos(2 * Math.PI * i / resolution) * radius + center.X;
                double y = Math.Sin(2 * Math.PI * i / resolution) * radius + center.Y;

                Vertices[i] = new Point2D(x, y);
            }

            for (int i = 0; i < resolution; i++)
            {
                if (i < resolution - 1)
                    Indices[i] = new int[] { i, i + 1};
                else
                    Indices[i] = new int[] { i, 0 };
            }

        }
    }

}
