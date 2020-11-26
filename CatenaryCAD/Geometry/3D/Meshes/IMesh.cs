namespace CatenaryCAD.Geometry.Meshes
{
    public interface IMesh
    {
        /// <summary>
        /// Массив вершин меша
        /// </summary>
        Point3D[] Vertices { get; }
        /// <summary>
        /// Массив нормалей меша
        /// </summary>
        Vector3D[] Normals { get; }
        /// <summary>
        /// Массив индексов вершин образующих полигоны и грани меша, 
        /// где первый уровень массива - индекс полигона, а второй - индекс вершины 
        /// </summary>
        int[][] Indices { get; }

        IMesh TransformBy(in Matrix3D m);

    }
}
