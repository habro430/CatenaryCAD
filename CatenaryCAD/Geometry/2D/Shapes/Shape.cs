using CatenaryCAD.Helpers;
using System;

namespace CatenaryCAD.Geometry.Shapes
{
    /// <summary>
    /// Класс, реализующий произвольную фигуру в 2D пространстве.
    /// </summary>
    [Serializable]
    public abstract class Shape : IShape
    {
        /// <inheritdoc/>
        public virtual Point2D[] Vertices { get; protected set; }

        /// <inheritdoc/>
        public virtual int[][] Indices { get; protected set; }

        /// <inheritdoc/>
        /// <param name="matrix">Матрица для преобразования объекта.</param>

        public virtual IShape TransformBy(in Matrix2D matrix)
        {
            var clone = this.DeepClone();
            int count = clone.Vertices.Length;

            for (int i = 0; i < count; i++)
                clone.Vertices[i] = clone.Vertices[i].TransformBy(matrix);

            return clone;
        }

        /// <inheritdoc/>
        public virtual bool IsInside(in Point2D point)
        {
            int count = Vertices.Length;

            bool c = false;
            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                var vi = Vertices[i];
                var vj = Vertices[j];

                if ((((vi.Y <= point.Y) && (point.Y < vj.Y)) || ((vj.Y <= point.Y) && (point.Y < vi.Y))) &&
                  (((vj.Y - vi.Y) != 0) && (point.X > ((vj.X - vi.X) * (point.Y - vi.Y) / (vj.Y - vi.Y) + vi.X))))
                    c = !c;
            }
            return c;
        }

        /// <inheritdoc/>
        public bool IsOutside(in Point2D point) => !IsInside(point);
    }
}
