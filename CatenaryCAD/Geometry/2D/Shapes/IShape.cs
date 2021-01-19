using CatenaryCAD.Geometry.Interfaces;

namespace CatenaryCAD.Geometry.Shapes
{
    /// <summary>
    /// Интерфейс, описывающий контракты для фигур в 2D пространстве.
    /// </summary>
    public interface IShape : ITransformable<Matrix2D, IShape>
    {
        /// <summary>
        /// Вершины описывающие фигуру.
        /// </summary>
        /// <value>Массив <see cref="Point2D"/> с вершинами.</value>
        Point2D[] Vertices { get; }

        /// <summary>
        /// Индексы грайней и вершин описывающие фигуру.
        /// </summary>
        /// <value>Массив индексов, где первый уровень массива - индекс грани, а второй - индекс вершины .</value>
        int[][] Indices { get; }

        /// <summary>
        /// Проверяет находится ли <paramref name = "point" /> внутри данного экземпляра <see cref="IShape"/>.
        /// </summary>
        /// <param name="point">Точка для проверки на соответсвие.</param>
        /// <returns><see langword="true"/> если <paramref name = "point" /> находиться внутри данного экземпляра <see cref="IShape"/>, в противном случае возращает - <see langword="false"/>.</returns>
        bool IsInside(in Point2D point);

        /// <summary>
        /// Проверяет находится ли <paramref name = "point" /> снаружи данного экземпляра <see cref="IShape"/>.
        /// </summary>
        /// <param name="point">Точка для проверки на соответсвие.</param>
        /// <returns><see langword="true"/> если <paramref name = "point" /> находиться снаружи данного экземпляра <see cref="IShape"/>, в противном случае возращает - <see langword="false"/>.</returns>
        bool IsOutside(in Point2D point);
    }
}
