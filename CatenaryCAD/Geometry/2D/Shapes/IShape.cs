namespace CatenaryCAD.Geometry.Shapes
{
    public interface IShape
    {
        /// <summary>
        /// Массив вершин фигуры
        /// </summary>
        Point2D[] Vertices { get; }

        /// <summary>
        /// Массив индексов вершин образующих грани фигуры, 
        /// где первый уровень массива - индекс грани, а второй - индекс вершины 
        /// </summary>
        int[][] Indices { get; }


        IShape TransformBy(in Matrix2D m);
        bool IsInside(in Point2D p);

    }
}
