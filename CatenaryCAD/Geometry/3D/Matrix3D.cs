using CatenaryCAD.Geometry.Interfaces;
using Multicad.Geometry;
using System;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    /// <summary>
    /// Структура, представляющая матрицу 4х4 для перобразований в 3D пространстве.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public readonly struct Matrix3D: IMatrix
    {
        /// <value>Первый элемент первой строки.</value>
        [FieldOffset(0)]
        public readonly double M11;
        /// <value>Второй элемент первой строки.</value>
        [FieldOffset(8)]
        public readonly double M12;
        /// <value>Третий элемент первой строки.</value>
        [FieldOffset(16)]
        public readonly double M13;
        /// <value>Четвертый элемент первой строки.</value>
        [FieldOffset(24)]
        public readonly double M14;

        /// <value>Первый элемент второй строки.</value>
        [FieldOffset(32)]
        public readonly double M21;
        /// <value>Второй элемент второй строки.</value>
        [FieldOffset(40)]
        public readonly double M22;
        /// <value>Третий элемент второй строки.</value>
        [FieldOffset(48)]
        public readonly double M23;
        /// <value>Четвертый элемент второй строки.</value>
        [FieldOffset(56)]
        public readonly double M24;

        /// <value>Первый элемент третьей строки.</value>
        [FieldOffset(64)]
        public readonly double M31;
        /// <value>Второй элемент третьей строки.</value>
        [FieldOffset(72)]
        public readonly double M32;
        /// <value>Третий элемент третьей строки.</value>
        [FieldOffset(80)]
        public readonly double M33;
        /// <value>Четвертый элемент третьей строки.</value>
        [FieldOffset(88)]
        public readonly double M34;

        /// <value>Первый элемент четвертой строки.</value>
        [FieldOffset(96)]
        public readonly double M41;
        /// <value>Второй элемент четвертой строки.</value>
        [FieldOffset(104)]
        public readonly double M42;
        /// <value>Третий элемент четвертой строки.</value>
        [FieldOffset(112)]
        public readonly double M43;
        /// <value>Четвертый элемент четвертой строки.</value>
        [FieldOffset(120)]
        public readonly double M44;

        /// <param name="m11">Первый элемент первой строки.</param>
        /// <param name="m12">Второй элемент первой строки.</param>
        /// <param name="m13">Третий элемент первой строки.</param>
        /// <param name="m14">Четвертый элемент первой строки.</param>
        /// <param name="m21">Первый элемент второй строки.</param>
        /// <param name="m22">Второй элемент второй строки.</param>
        /// <param name="m23">Третий элемент второй строки.</param>
        /// <param name="m24">Четвертый элемент второй строки.</param>
        /// <param name="m31">Первый элемент третьей строки.</param>
        /// <param name="m32">Третий элемент третьей строки.</param>
        /// <param name="m33">Четвертый элемент третьей строки.</param>
        /// <param name="m34">Четвертый элемент третьей строки.</param>
        /// <param name="m41">Первый элемент четвертой строки.</param>
        /// <param name="m42">Второй элемент четвертой строки.</param>
        /// <param name="m43">Третий элемент четвертой строки.</param>
        /// <param name="m44">Четвертый элемент четвертой строки.</param>
        public Matrix3D(double m11, double m12, double m13, double m14,
                        double m21, double m22, double m23, double m24,
                        double m31, double m32, double m33, double m34,
                        double m41, double m42, double m43, double m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        /// <value>
        /// Возвращает матрицу мультипликативного тождества.
        /// </value>
        public static ref readonly Matrix3D Identity => ref identity;
        private static readonly Matrix3D identity = new Matrix3D
        (
            1d, 0d, 0d, 0d,
            0d, 1d, 0d, 0d,
            0d, 0d, 1d, 0d,
            0d, 0d, 0d, 1d
        );

        /// <summary>
        /// Создает матрицу трансляции на основе заданного вектора <paramref name="translation"/>.
        /// </summary>
        /// <param name="translation">Значение трансляции по каждой оси.</param>
        /// <returns>Матрица трансляции.</returns>
        public static Matrix3D CreateTranslation(in Vector3D translation)
        {
            Matrix3D result = new Matrix3D(1d, 0d, 0d, translation.X,
                                           0d, 1d, 0d, translation.Y,
                                           0d, 0d, 1d, translation.Z,
                                           0d, 0d, 0d, 1d);

            return result;
        }

        /// <summary>
        /// Создает матрицу масштабирования на основе заданного масштаба вектора <paramref name="scale"/>.
        /// </summary>
        /// <param name="scale">Вектор, который содержит значение масштабирования по каждой оси.</param>
        /// <returns>Матрица масштабирования.</returns>
        public static Matrix3D CreateScale(in Vector3D scale)
        {
            Matrix3D result = new Matrix3D(scale.X, 0d, 0d, 0d,
                                           0d, scale.Y, 0d, 0d,
                                           0d, 0d, scale.Z, 0d,
                                           0d, 0d, 0d, 1d);
            return result;
        }

        /// <summary>
        /// Создает матрицу масштабирования на основе заданного масштаба вектора <paramref name="scale"/>
        /// и центральной точкой <paramref name="center"/>.
        /// </summary>
        /// <param name="scale">Вектор, который содержит значение масштабирования по каждой оси.</param>
        /// <param name="center">Центральная точка.</param>
        /// <returns>Матрица масштабирования.</returns>
        public static Matrix3D CreateScale(in Vector3D scale, in Point3D center)
        {
            double tx = center.X * (1 - scale.X);
            double ty = center.Y * (1 - scale.Y);
            double tz = center.Z * (1 - scale.Z);

            Matrix3D result = new Matrix3D(scale.X, 0d, 0d, tx,
                                           0d, scale.Y, 0d, ty,
                                           0d, 0d, scale.Z, tz,
                                           0d, 0d, 0d, 1d);
            return result;
        }

        /// <summary>
        /// Cоздает матрицу для поворота точек под заданным углом <paramref name="radians"/>, 
        /// вокруг оси <paramref name="axis"/> и относительно центральной точки <paramref name="center"/>.
        /// </summary>
        /// <param name="radians">Значение угла поворота вокруг оси в радианах.</param>
        /// <param name="center">Центральная точка.</param>
        /// <param name="axis">Ось поворота.</param>
        /// <returns>Матрица поворота.</returns>
        public static Matrix3D CreateRotation(double radians, in Point3D center, in Vector3D axis)
        {
            var normalized = axis.GetNormalize();

            double u = normalized.X, v = normalized.Y, w = normalized.Z,
                   a = center.X, b = center.Y, c = center.Z;

            double uu = u * u, vv = v * v, ww = w * w,
                   uv = u * v, uw = u * w,
                   vw = v * w;

            double au = a * u, av = a * v, aw = a * w, 
                   bu = b * u, bv = b * v, bw = b * w, 
                   cu = c * u, cv = c * v, cw = c * w; 

            double cos0 = Math.Cos(radians);
            double sin0 = Math.Sin(radians);

            var result = new Matrix3D(uu + (vv + ww) * cos0,         uv * (1 - cos0) - w * sin0,     uw * (1 - cos0) + v * sin0,     (a * (vv + ww) - u * (bv + cw)) * (1 - cos0) + (bw - cv) * sin0,
                                      uv * (1 - cos0) + w * sin0,    vv + (uu + ww) * cos0,          vw * (1 - cos0) - u * sin0,     (b * (uu + ww) - v * (au + cw)) * (1 - cos0) + (cu - aw) * sin0,
                                      uw * (1 - cos0) - v * sin0,    vw * (1 - sin0) + u * sin0,     ww + (uu + vv) * cos0,          (c * (uu + vv) - w * (au + bv)) * (1 - cos0) + (av - bu) * sin0,
                                      0d,                            0d,                             0d,                             1d);
            return result;
        }

    }
    internal static partial class Extensions
    {
        internal static Matrix3d ToMultiCAD(this in Matrix3D m) => new Matrix3d(new double[] { m.M11, m.M12, m.M13, m.M14,
                                                                                               m.M21, m.M22, m.M23, m.M24,
                                                                                               m.M31, m.M32, m.M33, m.M34,
                                                                                               m.M41, m.M42, m.M43, m.M44});
        internal static Matrix3D ToCatenaryCAD(this in Matrix3d m) => new Matrix3D(m[0, 0], m[0, 1], m[0, 2], m[0, 3],
                                                                                   m[1, 0], m[1, 1], m[1, 2], m[1, 3],
                                                                                   m[2, 0], m[2, 1], m[2, 2], m[2, 3],
                                                                                   m[3, 0], m[3, 1], m[3, 2], m[3, 3]);

    }
}
