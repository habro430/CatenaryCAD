using System;

namespace CatenaryCAD.Geometry.Shapes
{
    /// <summary>
    /// Класс, реализующий линию между двумя точками в 2D пространстве.
    /// </summary>
    [Serializable]
    public sealed class Line : Shape
    {
        ///<summary>
        /// Инициализирует новый экземпляр <see cref="Line"/>, с указанными 
        /// начальной точкой <paramref name="p0"/> и конечной точкой <paramref name="p1"/>.
        ///</summary>
        /// <param name="p0">Первая точка линии.</param>
        /// <param name="p1">Вторая точка линии.</param>
        public Line(in Point2D p0, in Point2D p1)
        {
            Vertices = new Point2D[] { p0, p1 };
            Indices = new int[][] { new int[] { 0, 1 } };
        }

        /// <summary>
        /// Проверяет с точностью 1E-03d, находится ли <paramref name = "p" /> на линии реализуемой текущим экземпляром <see cref="Line"/>.
        /// </summary>
        /// <param name="p">Точка для проверки на соответствие.</param>
        /// <returns><see langword="true"/> если <paramref name = "p" /> находиться на линии реализуемой текущим экземпляром <see cref="Line"/>, в противном случае возращает - <see langword="false"/>.</returns>
        public override bool IsInside(in Point2D p) => IsInside(p, 1E-03d);

        /// <summary>
        /// Проверяет находится ли <paramref name = "p" /> на линии реализуемой текущим экземпляром <see cref="Line"/>.
        /// </summary>
        /// <param name="p">Точка для проверки на соответствие.</param>
        /// <param name="t">Точность проверки на соответствие.</param>
        /// <returns><see langword="true"/> если <paramref name = "p" /> находиться на линии реализуемой текущим экземпляром <see cref="Line"/>, в противном случае возращает - <see langword="false"/>.</returns>
        public bool IsInside(in Point2D p, double t)
        {
            var a = Vertices[0];
            var b = Vertices[1];

            var zero = (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            if (zero > t || zero < -t) return false;

            if (a.X - b.X > t || b.X - a.X > t)
                return a.X > b.X
                    ? p.X + t > b.X && p.X - t < a.X
                    : p.X + t > a.X && p.X - t < b.X;

            return a.Y > b.Y
                ? p.Y + t > b.Y && p.Y - t < a.Y
                : p.Y + t > a.Y && p.Y - t < b.Y;
        }

        /// <summary>
        /// Возвращает среднюю точку на линии реализуемой текущим экземпляром <see cref="Line"/>.
        /// </summary>
        /// <returns>Средняя точка на линии.</returns>
        public Point2D GetMiddlePoint() => new Point2D((Vertices[0].X + Vertices[1].X) / 2, (Vertices[0].Y + Vertices[1].Y) / 2);

    }
}
