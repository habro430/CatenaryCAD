using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using CatenaryCAD.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CatenaryCAD.Geometry
{
    /// <summary>
    /// Структура, представляющая луч в 2D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("Origin = {Origin}, Direction = {Direction}")]
    [StructLayout(LayoutKind.Explicit, Size = 40)]
    public readonly struct Ray2D : IEquatable<Ray2D>
    {
        /// <summary>
        /// Начальная точка луча.
        /// </summary>
        [FieldOffset(0)]
        public readonly Point2D Origin;

        /// <summary>
        /// Направление луча
        /// </summary>
        [FieldOffset(16)]
        public readonly Vector2D Direction;

        public Ray2D(Point2D origin, Vector2D direction)
        {
            Origin = origin;
            Direction = direction;
        }

        #region Functions

        public Point2D[] GetIntersections(IShape shape)
        {
            List<Point2D> intersections = new List<Point2D>();

            foreach (var edge in shape.Indices)
            {
                var p = Origin;
                var q = shape.Vertices[edge[0]];
                var r = p.GetVectorTo(p + Direction);
                var s = q.GetVectorTo(shape.Vertices[edge[1]]);

                var rxs = r.CrossProduct(s);
                var qpxr = (q - p).CrossProduct(r);

                // Если r * s = 0 и (q - p) * r = 0, тогда два вектора коллинеарны.
                if (rxs == 0 && qpxr == 0)
                    continue;

                // Если r * s = 0 и (q - p) * r != 0, тогда два вектора параллельны и не пересекаются.
                if (rxs==0 && qpxr != 0)
                    continue;

                var t = (q - p).CrossProduct(s) / rxs;
                var u = (q - p).CrossProduct(r) / rxs;

                //Если r * s != 0 и 0 <= t <= 1 и 0 <= u <= 1,
                //тогда два вектора пересекаються в точке p + t * r = q + u * s.
                if (rxs != 0 && (0 <= t && t <= 1) && (0 <= u && u <= 1))
                    intersections.Add(p + (r * t));
            }

            return intersections.Count > 0 ? intersections.ToArray() : null;
        }

        /// <summary>
        /// Указывает, равен ли этот экземпляр заданному объекту.
        /// </summary>
        /// <param name="obj">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="obj"/> и данный экземпляр относятся к одному типу
        /// и представляют одинаковые значения, в противному случаее - <see langword="false"/></returns>
        public override bool Equals(object obj) => obj is Ray2D r && this.Equals(r);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противному случаее - <see langword="false"/></returns>
        public bool Equals(Ray2D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        /// <summary>
        /// Трансформирует данный экземпляр <see cref="Ray2D"/>, умножая его на <paramref name = "matrix" />.
        /// </summary>
        /// <param name="matrix">Матрица для умножения.</param>
        /// <returns>Трансформированный экземпляр <see cref="Ray2D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2D TransformBy(in Matrix2D matrix) => new Ray2D(Origin.TransformBy(matrix), Direction.TransformBy(matrix));

        #endregion
    }
}