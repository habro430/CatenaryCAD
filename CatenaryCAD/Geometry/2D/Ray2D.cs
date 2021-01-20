using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    /// <summary>
    /// Структура, представляющая луч в 2D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("Origin = {Origin}, Direction = {Direction}")]
    [StructLayout(LayoutKind.Explicit, Size = 40)]
    public readonly struct Ray2D : IRay, IEquatable<Ray2D>, IOriginable<Point2D>, IDirectionable<Vector2D>, ITransformable<Matrix2D, Ray2D>
    {
        [FieldOffset(0)]
        private readonly Point2D origin;
        [FieldOffset(16)]
        private readonly Vector2D direction;

        /// <value>Начальная точка луча.</value>
        public Point2D Origin 
        { 
            get => origin; 
            set => throw new NotSupportedException(); 
        }

        /// <value>Вектор направления луча.</value>
        public Vector2D Direction 
        { 
            get => direction; 
            set => throw new NotSupportedException(); 
        }

        /// <param name="origin">Начальная точка нового луча.</param>
        /// <param name="direction">Вектор направления нового луча.</param>
        public Ray2D(in Point2D origin, in Vector2D direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        #region Functions

        /// <summary>
        /// Возвращает точки пересечения <paramref name="shape"/> и данного луча.
        /// </summary>
        /// <param name="shape"><see cref="IShape"/> для расчета пересечений.</param>
        /// <returns>Массив <see cref="Point2D"/>[] с точками пересечений, 
        /// если пересечения отсутствуют - <see langword="null"/>.</returns>
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
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр относятся к одному типу
        /// и представляют одинаковые значения, в противному случаее - <see langword="false"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override bool Equals(object other) => other is Ray2D r && this.Equals(r);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противному случаее - <see langword="false"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(Ray2D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        /// <returns>Хэш-код данного экземпляра.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        /// <inheritdoc/>
        /// <param name="matrix">Матрица для трансформации.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Ray2D TransformBy(in Matrix2D matrix) => 
            new Ray2D(Origin.TransformBy(matrix), Direction.TransformBy(matrix));

        #endregion
    }
}