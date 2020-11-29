namespace CatenaryCAD.Geometry.Meshes
{
    public interface IMesh
    {
        /// <summary>
        /// Вершины описывающие меш.
        /// </summary>
        /// <value>Массив <see cref="Point2D"/> с вершинами.</value>
        Point3D[] Vertices { get; }

        /// <summary>
        /// Массив нормалей меша
        /// </summary>
        /// <value>Массив <see cref="Vector3D"/> с нормалями.</value>
        Vector3D[] Normals { get; }

        /// <summary>
        /// Индексы полигонов и вершин описывающие меш.
        /// </summary>
        /// <value>Массив индексов, где первый уровень массива - индекс полигона, а второй - индекс вершины .</value>
        int[][] Indices { get; }


        /// <summary>
        /// Трансформирует этот экземпляр <see cref="IMesh"/>, умножая все его вершины на <paramref name = "m" />.
        /// </summary>
        /// <param name="m">Матрица для умножения.</param>
        /// <returns>Трансформированный экземпляр <see cref="IMesh"/>.</returns>
        IMesh TransformBy(in Matrix3D m);

    }
}
