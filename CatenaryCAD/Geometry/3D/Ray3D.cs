using CatenaryCAD.Geometry.Meshes;
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
    /// Структура, представляющая луч в 3D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("Origin = {Origin}, Direction = {Direction}")]
    [StructLayout(LayoutKind.Explicit, Size = 56)]
    public readonly struct Ray3D : IEquatable<Ray3D>
    {
        /// <value>Начальная точка луча.</value>
        [FieldOffset(0)]
        public readonly Point3D Origin;

        /// <value>Вектор направления луча.</value>
        [FieldOffset(24)]
        public readonly Vector3D Direction;

        /// <param name="origin">Начальная точка нового луча.</param>
        /// <param name="direction">Вектор направления нового луча.</param>
        public Ray3D(Point3D origin, Vector3D direction)
        {
            Origin = origin;
            Direction = direction.GetNormalize();
        }

        #region Functions

        /// <summary>
        /// Возвращает точки пересечения <paramref name="mesh"/> и данного луча.
        /// </summary>
        /// <param name="mesh"><see cref="IMesh"/> для расчета пересечений.</param>
        /// <returns>Массив <see cref="Point3D"/>[] с точками пересечений, 
        /// если пересечения отсутствуют - <see langword="null"/>.</returns>
        public Point3D[] GetIntersections(IMesh mesh)
        {
            List<Point3D> intersections = new List<Point3D>();

            foreach (var face in mesh.Indices)
            {
                var a = mesh.Vertices[face[0]];
                var b = mesh.Vertices[face[1]];
                var c = mesh.Vertices[face[2]];

                var e1 = b - a;
                var e2 = c - a;
                var P = Direction.CrossProduct(e2);
                var det = e1.DotProduct(P);

                if (det > -double.Epsilon && det < double.Epsilon) 
                    continue;

                var inv = 1d / det;

                var T = Origin - a;
                var u = T.DotProduct(P) * inv;

                if (u < 0f || u > 1f) 
                    continue;

                var Q = T.CrossProduct(e1);
                var v = (Direction.DotProduct(Q * inv));

                if (v < 0f || u + v > 1f) 
                    continue;

                var t = e2.DotProduct(Q) * inv;
                if (t > double.Epsilon)
                    intersections.Add(Origin + Direction * t);
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
        public override bool Equals(object other) => other is Ray3D r && this.Equals(r);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противному случаее - <see langword="false"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(Ray3D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        /// <returns>Хэш-код данного экземпляра.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        /// <summary>
        /// Трансформирует данный экземпляр <see cref="Ray3D"/>, умножая его на <paramref name = "matrix" />.
        /// </summary>
        /// <param name="matrix">Матрица для умножения.</param>
        /// <returns>Трансформированный экземпляр <see cref="Ray3D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Ray3D TransformBy(in Matrix3D matrix) => new Ray3D(Origin.TransformBy(matrix), Direction.TransformBy(matrix));
    }

    #endregion
}
