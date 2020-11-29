using Multicad.Geometry;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public readonly struct Vector3D : IEquatable<Vector3D>, IVector
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
        /// <summary>
        /// Z - составляющая компонента вектора.
        /// </summary>
        [FieldOffset(16)]
        public readonly double Z;

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Static members
        /// <value>
        /// Нормализованный <see cref="Vector3D"/> по направлению оси X.
        /// </value>
        public static ref readonly Vector3D AxisX => ref axisx;
        private static readonly Vector3D axisx = new Vector3D(1, 0, 0);

        /// <value>
        /// Нормализованный <see cref="Vector3D"/> по направлению оси Y.
        /// </value>
        public static ref readonly Vector3D AxisY => ref axisy;
        private static readonly Vector3D axisy = new Vector3D(0, 1, 0);

        /// <value>
        /// Нормализованный <see cref="Vector3D"/> по направлению оси Z.
        /// </value>
        public static ref readonly Vector3D AxisZ => ref axisz;
        private static readonly Vector3D axisz = new Vector3D(0, 0, 1);

        #endregion

        #region Opearators

        public static Vector3D operator +(Vector3D a, Vector3D b) => new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3D operator -(Vector3D a, Vector3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3D operator *(Vector3D p, double s) => new Vector3D(p.X * s, p.Y * s, p.Z * s);

        public static Vector3D operator /(Vector3D p, double s) => new Vector3D(p.X / s, p.Y / s, p.Z / s);

        public static bool operator ==(Vector3D p1, Vector3D p2) => p1.Equals(p2);

        public static bool operator !=(Vector3D p1, Vector3D p2) => !p1.Equals(p2);

        #endregion

        #region Functions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Vector3D p && this.Equals(p);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3D n) => X.Equals(n.X) && Y.Equals(n.Y) && Z.Equals(n.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        /// <summary>
        /// Трансформирует этот <see cref="Vector3D"/>, умножая его на <paramref name = "m" />.
        /// </summary>
        /// <param name="m">Матрица для умножения.</param>
        /// <returns>Трансформированный экземпляр <see cref="Vector3D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D TransformBy(in Matrix3D m) =>
            new Vector3D(X * m.M11 + Y * m.M12 + Z * m.M13, X * m.M21 + Y * m.M22 + Z * m.M23, X * m.M31 + Y * m.M32 + Z * m.M33);

        /// <summary>
        /// Возвращает результат перекрестного произведения этого <see cref="Vector3D"/> и <paramref name = "v" />.
        /// </summary>
        /// <param name="v">Вектор для умножения.</param>
        /// <returns>Новый <see cref="Vector3D"/> с результатом кросс-произведения.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D CrossProduct(in Vector3D v) =>
            new Vector3D((Y * v.Z) - (Z * v.Y), (Z * v.X) - (X * v.Z), (X * v.Y) - (Y * v.X));

        /// <summary>
        /// Возвращает скалярное произведение этого <see cref="Vector3D"/> и <paramref name = "v" />.
        /// </summary>
        /// <param name="v">Вектор для умножения.</param>
        /// <returns>Результат скалярного произведения.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DotProduct(in Vector3D v) => (X * v.X) + (Y * v.Y) + (Z * v.Z);

        /// <summary>
        /// Возвращает угол между текущим <see cref="Vector3D"/> и <paramref name = "v"/>.
        /// </summary>
        /// <param name="v">Вектор по отношению к которому расчитывается угол.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetAngleTo(in Vector3D v, in Vector3D r) =>
            Math.Atan2(r.DotProduct(CrossProduct(v)), DotProduct(v));

        /// <summary>
        /// Возвращяет длинну этого <see cref="Vector3D"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetLength() => Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        /// <summary>
        /// Инвертирует направление этого <see cref="Vector3D"/>, эквивалентно умножению на -1.
        /// </summary>
        /// <returns>Экземпляр <see cref="Vector3D"/> указывающий в противоположное направление.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D GetNegate() => new Vector3D(-1 * X, -1 * Y, -1 * Z);

        /// <summary>
        /// Возвращает нормализованный <see cref="Vector3D"/> из этого <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>Нормализованный экземпляр <see cref="Vector3D"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3D GetNormalize()
        {
            var lenght = GetLength();
            return new Vector3D(X / lenght, Y / lenght, Z / lenght);
        }


        #endregion

    }

    public static partial class Extensions
    {
        internal static Vector3d ToMultiCAD(this in Vector3D v) => new Vector3d(v.X, v.Y, v.Z);
        internal static Vector3D ToCatenaryCAD_3D(this in Vector3d v) => new Vector3D(v.X, v.Y, v.Z);
    }
}
