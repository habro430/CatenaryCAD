using CatenaryCAD.Geometry.Interfaces;
using CatenaryCAD.Helpers;
using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    /// <summary>
    /// Структура, представляющая вектор в 3D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public readonly struct Vector3D : IVector, IEquatable<Vector3D>, ITransformable<Matrix3D, Vector3D>
    {
        /// <value> X - составляющая компонента вектора.</value>
        [FieldOffset(0)]
        public readonly double X;

        /// <value> Y - составляющая компонента вектора.</value>
        [FieldOffset(8)]
        public readonly double Y;

        /// <value> Z - составляющая компонента вектора.</value>
        [FieldOffset(16)]
        public readonly double Z;

        /// <param name="x">X - составляющая компонента нового вектора.</param>
        /// <param name="y">Y - составляющая компонента нового вектора.</param>
        /// <param name="z">Z - составляющая компонента нового вектора.</param>
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Static members
        /// <value>Нормализованный, единичный <see cref="Vector3D"/> по направлению оси X.</value>
        public static ref readonly Vector3D AxisX => ref axisx;
        private static readonly Vector3D axisx = new Vector3D(1, 0, 0);

        /// <value>Нормализованный, единичный <see cref="Vector3D"/> по направлению оси Y.</value>
        public static ref readonly Vector3D AxisY => ref axisy;
        private static readonly Vector3D axisy = new Vector3D(0, 1, 0);

        /// <value>Нормализованный, единичный <see cref="Vector3D"/> по направлению оси Z.</value>
        public static ref readonly Vector3D AxisZ => ref axisz;
        private static readonly Vector3D axisz = new Vector3D(0, 0, 1);

        #endregion

        #region Opearators
        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат сложения векторов <paramref name="a"/> и <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector3D operator +(Vector3D a, Vector3D b) => new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат разности векторов <paramref name="a"/> и <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector3D operator -(Vector3D a, Vector3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <param name="v">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат произведения вектора <paramref name="v"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector3D operator *(Vector3D v, double s) => new Vector3D(v.X * s, v.Y * s, v.Z * s);

        /// <param name="v">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат деления вектора <paramref name="v"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector3D operator /(Vector3D v, double s) => new Vector3D(v.X / s, v.Y / s, v.Z / s);

        /// <param name="v1">Первый операнд.</param>
        /// <param name="v2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="v1"/> и <paramref name="v2"/>
        /// представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(Vector3D v1, Vector3D v2) => v1.Equals(v2);

        /// <param name="v1">Первый операнд.</param>
        /// <param name="v2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="v1"/> и <paramref name="v2"/>
        /// представляют разные значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(Vector3D v1, Vector3D v2) => !v1.Equals(v2);

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
        public override bool Equals(object other) => other is Vector3D p && this.Equals(p);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противному случае - <see langword="false"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(Vector3D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        /// <returns>Хэш-код данного экземпляра.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        /// <summary>
        /// Трансформирует данный экземпляр <see cref="Vector3D"/>, умножая его на <paramref name = "matrix" />.
        /// </summary>
        /// <param name="matrix">Матрица для трансформации.</param>
        /// <returns>Трансформированный экземпляр <see cref="Vector3D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector3D TransformBy(in Matrix3D matrix) =>
            new Vector3D(X * matrix.M11 + Y * matrix.M12 + Z * matrix.M13, X * matrix.M21 + Y * matrix.M22 + Z * matrix.M23, X * matrix.M31 + Y * matrix.M32 + Z * matrix.M33);

        /// <summary>
        /// Возвращает результат перекрестного произведения данного экземпляра 
        /// <see cref="Vector3D"/> и <paramref name = "other" />.
        /// </summary>
        /// <param name="other">Вектор для кросс-произведения на данный экземпляр <see cref="Vector3D"/>.</param>
        /// <returns>Результат кросс-произведения данного экземпляра 
        /// <see cref="Vector3D"/> и <paramref name = "other" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector3D CrossProduct(in Vector3D other) =>
            new Vector3D((Y * other.Z) - (Z * other.Y), (Z * other.X) - (X * other.Z), (X * other.Y) - (Y * other.X));

        /// <summary>
        /// Возвращает скалярное произведение данного экземпляра 
        /// <see cref="Vector3D"/> и <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Вектор для скалярного произведения на данный экземпляр <see cref="Vector3D"/>.</param>
        /// <returns>Результат скалярного произведения данного экземпляра 
        /// <see cref="Vector3D"/> и <paramref name = "other"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double DotProduct(in Vector3D other) => (X * other.X) + (Y * other.Y) + (Z * other.Z);

        /// <summary>
        /// Возвращает угол между данным экземпляром <see cref="Vector3D"/> и
        /// <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Вектор по отношению к которому расчитывается угол.</param>
        /// <param name="refrence">Вектор оси для которой расчитывается угол.</param>
        /// <returns>Угол между данным экземпляром <see cref="Vector3D"/> и
        /// <paramref name = "other"/> в радианах.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double GetAngleTo(in Vector3D other, in Vector3D refrence) =>
            Math.Atan2(refrence.DotProduct(CrossProduct(other)), DotProduct(other));

        /// <summary>
        /// Возвращает длинну данного <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>Длинна вектора.</returns> 
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double GetLength() => Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        /// <summary>
        /// Инвертирует направление данного <see cref="Vector2D"/>, эквивалентно умножению на -1.
        /// </summary>
        /// <returns>Экземпляр <see cref="Vector2D"/> указывающий в противоположное направление.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector3D GetNegate() => new Vector3D(-1 * X, -1 * Y, -1 * Z);

        /// <summary>
        /// Возвращает нормализованный <see cref="Vector3D"/> из данного <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>Нормализованный экземпляр <see cref="Vector3D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector3D GetNormalize()
        {
            var lenght = GetLength();
            return new Vector3D(X / lenght, Y / lenght, Z / lenght);
        }


        #endregion

    }

    internal static partial class Extensions
    {
        internal static Vector3d ToMultiCAD(this in Vector3D v) => new Vector3d(v.X, v.Y, v.Z);
        internal static Vector3D ToCatenaryCAD_3D(this in Vector3d v) => new Vector3D(v.X, v.Y, v.Z);
    }
}
