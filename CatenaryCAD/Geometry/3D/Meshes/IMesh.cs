using Catenary.Geometry.Interfaces;

namespace Catenary.Geometry.Meshes
{
    /// <summary>
    /// Интерфейс, описывающий контракты для мешей в 3D пространстве.
    /// </summary>
    public interface IMesh : ITransformable<Matrix3D, IMesh>
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
    }
}
