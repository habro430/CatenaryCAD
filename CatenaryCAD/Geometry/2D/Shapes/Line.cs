using System;

namespace CatenaryCAD.Geometry.Shapes
{
    /// <summary>
    /// Класс, реализующий линию между двумя точками в 2D пространстве.
    /// </summary>
    [Serializable]
    public sealed class Line : Shape
    {
        /// <param name="p0">Первая точка линии.</param>
        /// <param name="p1">Вторая точка линии.</param>
        public Line(in Point2D p0, in Point2D p1)
        {
            Vertices = new Point2D[] { p0, p1 };
            Indices = new int[][] { new int[] { 0, 1 } };
        }

        /// <summary>
        /// Проверяет находится ли <paramref name = "point" /> на линии реализуемой текущим экземпляра <see cref="Line"/>.
        /// </summary>
        /// <param name="point">Точка для проверки на соответсвие.</param>
        /// <returns><see langword="true"/> если <paramref name = "point" /> находиться на линии реализуемой текущим экземпляром <see cref="Line"/>, в противном случае возращает - <see langword="false"/>.</returns>
        public override bool IsInside(in Point2D point)
        {
            throw new NotImplementedException();
        }
    }
}
