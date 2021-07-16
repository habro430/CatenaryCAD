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
    /// Структура, представляющая вектор в 2D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}")]
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public readonly struct Vector2D : IVector, IEquatable<Vector2D>, ITransformable<Matrix2D, Vector2D>
    {
        /// <value> X - составляющая компонента вектора.</value>
        [FieldOffset(0)]
        public readonly double X;

        /// <value> Y - составляющая компонента вектора.</value>
        [FieldOffset(8)]
        public readonly double Y;

        /// <param name="x">X - составляющая компонента нового вектора.</param>
        /// <param name="y">Y - составляющая компонента нового вектора.</param>
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Static members

        /// <value>Нормализованный, единичный <see cref="Vector2D"/> по направлению оси X.</value>
        public static ref readonly Vector2D AxisX => ref axisx;
        private static readonly Vector2D axisx = new Vector2D(1, 0);

        /// <value>Нормализованный, единичный <see cref="Vector2D"/> по направлению оси Y.</value>
        public static ref readonly Vector2D AxisY => ref axisy;
        private static readonly Vector2D axisy = new Vector2D(0, 1);

        #endregion

        #region Operators

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат сложения векторов <paramref name="a"/> и <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат разности векторов <paramref name="a"/> и <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);

        /// <param name="v">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат произведения вектора <paramref name="v"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector2D operator *(Vector2D v, double s) => new Vector2D(v.X * s, v.Y * s);

        /// <param name="v">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат деления вектора <paramref name="v"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector2D operator /(Vector2D v, double s) => new Vector2D(v.X / s, v.Y / s);

        /// <param name="v1">Первый операнд.</param>
        /// <param name="v2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="v1"/> и <paramref name="v2"/>
        /// представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(Vector2D v1, Vector2D v2) => v1.Equals(v2);

        /// <param name="v1">Первый операнд.</param>
        /// <param name="v2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="v1"/> и <paramref name="v2"/>
        /// представляют разные значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(Vector2D v1, Vector2D v2) => !v1.Equals(v2);

        #endregion

        #region Functions
        /// <summary>
        /// Указывает, равен ли этот экземпляр заданному объекту.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр относятся к одному типу
        /// и представляют одинаковые значения, в противному случае - <see langword="false"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override bool Equals(object other) => other is Vector2D p && this.Equals(p);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противному случае - <see langword="false"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(Vector2D other) => X.Equals(other.X) && Y.Equals(other.Y);

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
        public Vector2D TransformBy(in Matrix2D matrix) =>
            new Vector2D(X * matrix.M11 + Y * matrix.M12 + matrix.M13, X * matrix.M21 + Y * matrix.M22 + matrix.M23);

        /// <summary>
        /// Возвращает результат перекрестного произведения данного экземпляра 
        /// <see cref="Vector2D"/> и <paramref name = "other" />.
        /// </summary>
        /// <param name="other">Вектор для кросс-произведения на данный экземпляр <see cref="Vector2D"/>.</param>
        /// <returns>Результат кросс-произведения данного экземпляра 
        /// <see cref="Vector2D"/> и <paramref name = "other" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double CrossProduct(in Vector2D other) => (X * other.Y) - (Y * other.X);

        /// <summary>
        /// Возвращает скалярное произведение данного экземпляра 
        /// <see cref="Vector2D"/> и <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Вектор для скалярного произведения на данный экземпляр <see cref="Vector2D"/>.</param>
        /// <returns>Результат скалярного произведения данного экземпляра 
        /// <see cref="Vector2D"/> и <paramref name = "other"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double DotProduct(in Vector2D other) => (X * other.X) + (Y * other.Y);

        /// <summary>
        /// Возвращает угол между данным экземпляром <see cref="Vector2D"/> и
        /// <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Вектор по отношению к которому расчитывается угол.</param>
        /// <returns>Угол между данным экземпляром <see cref="Vector2D"/> и
        /// <paramref name = "other"/> в радианах.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double GetAngleTo(in Vector2D other) => Math.Atan2(CrossProduct(other), DotProduct(other));

        /// <summary>
        /// Возвращает длинну данного <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>Длинна вектора.</returns> 
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double GetLength() => Math.Sqrt((X * X) + (Y * Y));

        /// <summary>
        /// Инвертирует направление данного <see cref="Vector2D"/>, эквивалентно умножению на -1.
        /// </summary>
        /// <returns>Экземпляр <see cref="Vector2D"/> указывающий в противоположное направление.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector2D GetNegate() => new Vector2D(-1 * X, -1 * Y);

        /// <summary>
        /// Возвращает нормализованный <see cref="Vector2D"/> из данного <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>Нормализованный экземпляр <see cref="Vector2D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector2D GetNormalize()
        {
            var length = GetLength();
            return new Vector2D(X / length, Y / length);
        }

        #endregion
    }

    internal static partial class Extensions
    {
        internal static Vector3d ToMultiCAD(this in Vector2D v) => new Vector3d(v.X, v.Y, 0);
        internal static Vector2D ToCatenaryCAD_2D(this in Vector3d v) => new Vector2D(v.X, v.Y);
    }
}