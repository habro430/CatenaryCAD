using Catenary.Geometry.Interfaces;
using Catenary.Helpers;
using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Catenary.Geometry
{
    /// <summary>
    /// Структура, представляющая точку в 2D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}")]
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public readonly struct Point2D : IPoint, IEquatable<Point2D>, ITransformable<Matrix2D, Point2D>
    {
        /// <value> X - составляющая компонента точки.</value>
        [FieldOffset(0)]
        public readonly double X;

        /// <value> Y - составляющая компонента точки.</value>
        [FieldOffset(8)]
        public readonly double Y;

        /// <param name="x">X - составляющая компонента новой точки.</param>
        /// <param name="y">Y - составляющая компонента новой точки.</param>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        #region Static members

        /// <value>Точка с нулевыми координатами по двум осям.</value>
        public static ref readonly Point2D Origin => ref origin;
        private static readonly Point2D origin = new Point2D(0, 0);

        #endregion

        #region Operators
        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат сложения точки <paramref name="a"/> и вектора <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point2D operator +(Point2D a, Vector2D b) => new Point2D(a.X + b.X, a.Y + b.Y);

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат разности точки <paramref name="a"/> и вектора <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point2D operator -(Point2D a, Vector2D b) => new Point2D(a.X - b.X, a.Y - b.Y);

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат разности точки <paramref name="a"/> и точки <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector2D operator -(Point2D a, Point2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);

        /// <param name="p">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат произведения точки <paramref name="p"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point2D operator *(Point2D p, double s) => new Point2D(p.X * s, p.Y * s);

        /// <param name="p">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат деления точки <paramref name="p"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point2D operator /(Point2D p, double s) => new Point2D(p.X / s, p.Y / s);

        /// <param name="p1">Первый операнд.</param>
        /// <param name="p2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="p1"/> и <paramref name="p2"/>
        /// представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(Point2D p1, Point2D p2) => p1.Equals(p2);

        /// <param name="p1">Первый операнд.</param>
        /// <param name="p2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="p1"/> и <paramref name="p2"/>
        /// представляют разные значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(Point2D p1, Point2D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        /// <summary>
        /// Указывает, равен ли этот экземпляр заданному объекту.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр относятся к одному типу
        /// и представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override bool Equals(object other) => other is Point2D p && this.Equals(p);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(Point2D other) => X.Equals(other.X) && Y.Equals(other.Y);

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        /// <returns>Хэш-код данного экземпляра.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode() => HashCode.Combine(X, Y);

        /// <inheritdoc/>
        /// <param name="matrix">Матрица для трансформации.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Point2D TransformBy(in Matrix2D matrix) => 
            new Point2D(X * matrix.M11 + Y * matrix.M12 + matrix.M13, X * matrix.M21 + Y * matrix.M22 + matrix.M23);

        /// <summary>
        /// Возвращает <see cref="Vector3D"/> между текущим экземпляром <see cref="Point3D"/> и <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Точка по отношению к которой расчитывается вектор.</param>
        /// <returns>Вектор по направлению от текущего экземпляра <see cref="Point2D"/> и стремящийся к <paramref name = "other"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector2D GetVectorTo(in Point2D other) => other - this;

        /// <summary>
        /// Возвращает расстояние между текущим экземпляром <see cref="Point2D"/> и <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Точка по отношению к которой расчитывается расстояние.</param>
        /// <returns>Расстояние между текущим экземпляром <see cref="Point2D"/> и <paramref name = "other"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double GetDistanceTo(in Point2D other) => GetVectorTo(other).GetLength();

        #endregion

    }

    internal static partial class Extensions
    {
        internal static Point3d ToMultiCAD(this in Point2D p) => new Point3d(p.X, p.Y, 0);
        internal static Point2D ToCatenaryCAD_2D(this in Point3d p) => new Point2D(p.X, p.Y);
    }

}