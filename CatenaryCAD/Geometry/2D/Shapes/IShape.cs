using System;

namespace CatenaryCAD.Geometry.Shapes
{
    public interface IShape
    {
        /// <summary>
        /// Вершины описывающие фигуру.
        /// </summary>
        /// <value>Массив <see cref="Point2D"/> с вершинами.</value>
        Point2D[] Vertices { get; }

        /// <summary>
        /// Индексы вешин и граней описывающие фигуру.
        /// </summary>
        /// <value>Массив индексов, где первый уровень массива - индекс грани, а второй - индекс вершины .</value>
        int[][] Indices { get; }


        /// <summary>
        /// Трансформирует этот экземпляр <see cref="IShape"/>, умножая все его вершины на <paramref name = "m" />.
        /// </summary>
        /// <param name="m">Матрица для умножения.</param>
        /// <returns>Трансформированный экземпляр <see cref="IShape"/>.</returns>
        IShape TransformBy(in Matrix2D m);

        /// <summary>
        /// Проверяет находится ли <paramref name = "p" /> внутри текущего экземпляра <see cref="IShape"/>.
        /// </summary>
        /// <param name="p">Точка для проверки на соответсвие.</param>
        /// <returns>Возвраящает true если <paramref name = "p" /> находиться внутри текущего экземпляра <see cref="IShape"/>, в противном случае возращает false.</returns>
        bool IsInside(in Point2D p);
    }
}
