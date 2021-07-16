using System;

namespace Catenary.Geometry.Shapes
{
    /// <summary>
    /// Класс, реализующий треугольник в 2D пространстве.
    /// </summary>
    [Serializable]
    public class Triangle : Shape
    {
        ///<summary>
        /// Инициализирует новый экземпляр <see cref="Triangle"/>, с указанными 
        /// вершинами <paramref name="a"/>, <paramref name="b"/> и <paramref name="c"/>.
        ///</summary>
        ///<param name="a">Первая вершина треугольника.</param>
        ///<param name="b">Вторая вершина треугольника.</param>
        ///<param name="c">Третья вершина треугольника.</param>
        public Triangle(Point2D a, Point2D b, Point2D c)
        {
            Vertices = new Point2D[] { a, b, c };
            Indices = new int[][] { new int[] { 0, 1 },
                                    new int[] { 1, 2 },
                                    new int[] { 2, 0 }};
        }
    }
}
