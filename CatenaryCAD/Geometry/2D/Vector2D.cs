using CatenaryCAD.Helpers;
using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}")]
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public readonly struct Vector2D : IEquatable<Vector2D>, IVector
    {
        /// <summary>
        /// X - составляющая компонента вектора.
        /// </summary>
        [FieldOffset(0)]
        public readonly double X;
        /// <summary>
        /// Y - составляющая компонента вектора.
        /// </summary>
        [FieldOffset(8)]
        public readonly double Y;

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Static members

        /// <summary>
        /// Нормализованный <see cref="Vector2D"/> по направлению оси X.
        /// </summary>
        public static ref readonly Vector2D AxisX => ref axisx;
        private static readonly Vector2D axisx = new Vector2D(1, 0);

        /// <summary>
        /// Нормализованный <see cref="Vector2D"/> по направлению оси Y.
        /// </summary>
        public static ref readonly Vector2D AxisY => ref axisy;
        private static readonly Vector2D axisy = new Vector2D(0, 1);

        #endregion

        #region Operators

        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D p, double s) => new Vector2D(p.X * s, p.Y * s);
        public static Vector2D operator /(Vector2D p, double s) => new Vector2D(p.X / s, p.Y / s);
        public static bool operator ==(Vector2D p1, Vector2D p2) => p1.Equals(p2);
        public static bool operator !=(Vector2D p1, Vector2D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Vector2D p && this.Equals(p);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2D n) => X.Equals(n.X) && Y.Equals(n.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y);


        /// <summary>
        /// Трансформирует этот экземпляр <see cref="Vector2D"/>, умножая его на <paramref name = "m" />.
        /// </summary>
        /// <param name="m">Матрица для умножения.</param>
        /// <returns>Трансформированный экземпляр <see cref="Vector2D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D TransformBy(in Matrix2D m) =>
            new Vector2D(X * m.M11 + Y * m.M12 + m.M13, X * m.M21 + Y * m.M22 + m.M23);

        /// <summary>
        /// Возвращает результат перекрестного произведения этого <see cref="Vector2D"/> и <paramref name = "v" />.
        /// </summary>
        /// <param name="v">Вектор для умножения.</param>
        /// <returns>Результат кросс-произведения.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CrossProduct(in Vector2D v) => (X * v.Y) - (Y * v.X);

        /// <summary>
        /// Возвращает скалярное произведение этого <see cref="Vector2D"/> и <paramref name = "v"/>.
        /// </summary>
        /// <param name="v">Вектор для умножения.</param>
        /// <returns>Результат скалярного произведения.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DotProduct(in Vector2D v) => (X * v.X) + (Y * v.Y);

        /// <summary>
        /// Возвращает угол между текущим <see cref="Vector2D"/> и <paramref name = "v"/>.
        /// </summary>
        /// <param name="v">Вектор по отношению к которому расчитывается угол.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetAngleTo(in Vector2D v) => Math.Atan2(CrossProduct(v), DotProduct(v));

        /// <summary>
        /// Возвращает длинну этого <see cref="Vector2D"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetLength() => Math.Sqrt((X * X) + (Y * Y));

        /// <summary>
        /// Инвертирует направление этого <see cref="Vector2D"/>, эквивалентно умножению на -1.
        /// </summary>
        /// <returns>Экземпляр <see cref="Vector2D"/> указывающий в противоположное направление.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D GetNegate() => new Vector2D(-1 * X, -1 * Y);

        /// <summary>
        /// Возвращает нормализованный <see cref="Vector2D"/> из этого <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>Нормализованный экземпляр <see cref="Vector2D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D GetNormalize()
        {
            var length = GetLength();
            return new Vector2D(X / length, Y / length);
        }

        #endregion
    }

    public static partial class Extensions
    {
        internal static Vector3d ToMultiCAD(this in Vector2D v) => new Vector3d(v.X, v.Y, 0);
        internal static Vector2D ToCatenaryCAD_2D(this in Vector3d v) => new Vector2D(v.X, v.Y);
    }
}