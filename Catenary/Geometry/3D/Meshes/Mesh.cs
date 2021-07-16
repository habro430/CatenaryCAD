using Catenary.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Catenary.Geometry.Meshes
{
    /// <summary>
    /// Класс, реализующий меш в 3D пространстве.
    /// </summary>
    [Serializable]
    public class Mesh : IMesh
    {
        /// <inheritdoc/>
        public Point3D[] Vertices { get; private set; }

        /// <inheritdoc/>
        public Vector3D[] Normals { get; private set; }

        /// <inheritdoc/>
        public int[][] Indices { get; private set; }

        ///<summary>
        /// Инициализирует новый экземпляр <see cref="Mesh"/>, с указанными 
        /// вершинами <paramref name="vertices"/>, нормалями <paramref name="normals"/> 
        /// и индексами <paramref name="indices"/>.
        ///</summary>
        ///<param name="vertices">Вершины образующие меш.</param>
        ///<param name="normals">Массив нормалей меша.</param>
        ///<param name="indices">Индексы полигонов и вершин описывающие меш.</param>

        public Mesh(Point3D[] vertices, Vector3D[] normals, int[][] indices)

        {
            Vertices = vertices;
            Normals = normals;
            Indices = indices;
        }

        /// <inheritdoc/>
        /// <param name="matrix">Матрица для преобразования объекта.</param>
        public virtual IMesh TransformBy(in Matrix3D matrix)
        {
            var clone = this.DeepClone();
            int count = clone.Vertices.Length;

            for (int i = 0; i < count; i++)
                clone.Vertices[i] = clone.Vertices[i].TransformBy(matrix);

            return clone;
        }

        public static Mesh FromObj(string obj)
        {
            string[] lines = obj.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var vertices = new List<Point3D>();
            var normals = new List<Vector3D>();
            var indices = new List<int[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;

                string[] data = lines[i].Split(' ');

                bool success;
                switch (data[0])
                {
                    case "v": //парсер вершин
                        double xv, yv, zv;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out xv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out yv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out zv);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        vertices.Add(new Point3D(xv, yv, zv));

                        break;

                    case "vn": //парсер нормалей
                        double xn, yn, zn;

                        success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out xn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр X как число double");

                        success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out yn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Y как число double");

                        success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out zn);
                        if (!success) throw new ArgumentException("Невозможно преобразовать параметр Z как число double");

                        normals.Add(new Vector3D(xn, yn, zn));
                        break;

                    case "f": // парсер многоугольника
                        int vcount = data.Length - 1;
                        int[] vert = new int[vcount];

                        for (int ivert = 0; ivert < vcount; ivert++)
                        {
                            string[] parts = data[ivert + 1].Split('/');

                            int vindex;
                            success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                            if (!success) throw new ArgumentException("Невозможно преобразовать параметр как число int");

                            vert[ivert] = vindex - 1;
                        }

                        indices.Add(vert);
                        break;
                }
            }

            return new Mesh(vertices.ToArray(), normals.ToArray(), indices.ToArray());
        }
    }
}
