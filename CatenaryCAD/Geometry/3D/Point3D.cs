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
    /// Структура, представляющая точку в 3D пространстве.
    /// </summary>
    [Serializable, DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public readonly struct Point3D : IPoint, IEquatable<Point3D>, ITransformable<Matrix3D, Point3D>
    {
        /// <value> X - составляющая компонента точки.</value>
        [FieldOffset(0)]
        public readonly double X;

        /// <value> Y - составляющая компонента точки.</value>
        [FieldOffset(8)]
        public readonly double Y;

        /// <value> Z - составляющая компонента точки.</value>
        [FieldOffset(16)]
        public readonly double Z;

        /// <param name="x">X - составляющая компонента новой точки.</param>
        /// <param name="y">Y - составляющая компонента новой точки.</param>
        /// <param name="z">Z - составляющая компонента новой точки.</param>
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Static members

        /// <value>Точка с нулевыми координатами по двум осям.</value>
        public static ref readonly Point3D Origin => ref origin;
        private static readonly Point3D origin = new Point3D(0, 0, 0);

        #endregion

        #region Operators
        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат сложения точки <paramref name="a"/> и вектора <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point3D operator +(Point3D a, Vector3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат разности точки <paramref name="a"/> и вектора <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point3D operator -(Point3D a, Vector3D b) => new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <param name="a">Первый операнд.</param>
        /// <param name="b">Второй операнд.</param>
        /// <returns>Результат разности точки <paramref name="a"/> и точки <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Vector3D operator -(Point3D a, Point3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <param name="p">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат произведения точки <paramref name="p"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point3D operator *(Point3D p, double s) => new Point3D(p.X * s, p.Y * s, p.Z * s);

        /// <param name="p">Первый операнд.</param>
        /// <param name="s">Второй операнд.</param>
        /// <returns>Результат деления точки <paramref name="p"/> и числа <paramref name="s"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static Point3D operator /(Point3D p, double s) => new Point3D(p.X / s, p.Y / s, p.Z / s);

        /// <param name="p1">Первый операнд.</param>
        /// <param name="p2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="p1"/> и <paramref name="p2"/>
        /// представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(Point3D p1, Point3D p2) => p1.Equals(p2);

        /// <param name="p1">Первый операнд.</param>
        /// <param name="p2">Второй операнд.</param>
        /// <returns><see langword="true"/> если <paramref name="p1"/> и <paramref name="p2"/>
        /// представляют разные значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(Point3D p1, Point3D p2) => !p1.Equals(p2);

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
        public override bool Equals(object other) => other is Point3D p && this.Equals(p);

        /// <summary>
        /// Указывает, эквивалентен ли текущий объект другому объекту того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с текущим экземпляром.</param>
        /// <returns><see langword="true"/> если <paramref name="other"/> и данный экземпляр 
        /// представляют одинаковые значения, в противном случае - <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(Point3D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        /// <summary>
        /// Возвращает хэш-код данного экземпляра.
        /// </summary>
        /// <returns>Хэш-код данного экземпляра.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        /// <inheritdoc/>
        /// <param name="matrix">Матрица для трансформации.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Point3D TransformBy(in Matrix3D matrix) => 
            new Point3D(X * matrix.M11 + Y * matrix.M12 + Z * matrix.M13 + matrix.M14, 
                        X * matrix.M21 + Y * matrix.M22 + Z * matrix.M23 + matrix.M24, 
                        X * matrix.M31 + Y * matrix.M32 + Z * matrix.M33 + matrix.M34);

        /// <summary>
        /// Возвращает <see cref="Vector3D"/> между текущим экземпляром <see cref="Point3D"/> и <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Точка по отношению к которой расчитывается вектор.</param>
        /// <returns>Вектор по направлению от текущего экземпляра <see cref="Point3D"/> и стремящийся к <paramref name = "other"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Vector3D GetVectorTo(in Point3D other) => other - this;

        /// <summary>
        /// Возвращает расстояние между текущим экземпляром <see cref="Point3D"/> и <paramref name = "other"/>.
        /// </summary>
        /// <param name="other">Точка по отношению к которой расчитывается расстояние.</param>
        /// <returns>Расстояние между текущим экземпляром <see cref="Point3D"/> и <paramref name = "other"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
        TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public double GetDistanceTo(in Point3D other) => GetVectorTo(other).GetLength();

        #endregion

    }
    internal static partial class Extensions
    {
        internal static Point3d ToMultiCAD(this in Point3D p) => new Point3d(p.X, p.Y, p.Z);
        internal static Point3D ToCatenaryCAD_3D(this in Point3d p) => new Point3D(p.X, p.Y, p.Z);
    }
}
