using System;

namespace CatenaryCAD.Geometry.Shapes
{
    [Serializable]
    public sealed class Line : Shape
    {
        public Line(in Point2D p0, in Point2D p1)
        {
            Vertices = new Point2D[] { p0, p1 };
            Indices = new int[][] { new int[] { 0, 1 } };
        }

        /// <summary>
        /// Проверяет находится ли <paramref name = "p" /> на линии реализуемой текущим экземпляра <see cref="Line"/>.
        /// </summary>
        /// <param name="p">Точка для проверки на соответсвие.</param>
        /// <returns>Возвраящает true если <paramref name = "p" /> находиться на линии реализуемой текущим экземпляром <see cref="Line"/>, в противном случае возращает false.</returns>
        public override bool IsInside(in Point2D p)
        {
            throw new NotImplementedException(0);
        }
    }
}
