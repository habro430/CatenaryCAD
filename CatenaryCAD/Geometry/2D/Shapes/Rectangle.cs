using System;

namespace CatenaryCAD.Geometry.Shapes
{
    /// <summary>
    /// Класс, реализующий прямоугольник в 2D пространстве.
    /// </summary>
    [Serializable]
    public sealed class Rectangle : Shape
    {
        ///<summary>
        /// Инициализирует новый экземпляр <see cref="Rectangle"/>, с указанным 
        /// центром <paramref name="center"/>, шириной <paramref name="width"/> и высотой <paramref name="height"/>.
        ///</summary>
        ///<param name="center">Центр прямоугольника.</param>
        ///<param name="width">Ширина прямоугольника.</param>
        ///<param name="height">Высота прямоугольника.</param>
        public Rectangle(in Point2D center, double width, double height)
        {
            Vertices = new Point2D[]
            {
                new Point2D(center.X - height / 2, center.Y + width / 2),
                new Point2D(center.X + height / 2, center.Y + width / 2),
                new Point2D(center.X + height / 2, center.Y - width / 2),
                new Point2D(center.X - height / 2, center.Y - width / 2)
            };
            Indices = new int[][] 
            {  
                new int[]{ 0, 1 },
                new int[]{ 1, 2 },
                new int[]{ 2, 3 },
                new int[]{ 3, 0 } 
            };
        }

        ///<summary>
        /// Инициализирует новый экземпляр <see cref="Rectangle"/>, с указанными 
        /// вершинами <paramref name="p0"/>, <paramref name="p1"/>, <paramref name="p2"/> и <paramref name="p3"/>.
        ///</summary>
        ///<param name="p0">Первая вершина треугольника.</param>
        ///<param name="p1">Вторая вершина треугольника.</param>
        ///<param name="p2">Третья вершина треугольника.</param>
        ///<param name="p3">Четвертая вершина треугольника.</param>
        public Rectangle(Point2D p0, Point2D p1, Point2D p2, Point2D p3)
        {
            Vertices = new Point2D[] { p0, p1, p2, p3 };
            Indices = new int[][]
            {
                new int[]{ 0, 1 },
                new int[]{ 1, 2 },
                new int[]{ 2, 3 },
                new int[]{ 3, 0 }
            };
        }
    }
}
