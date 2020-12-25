using System;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    /// <summary>
    /// Структура, представляющая матрицу 3х3 для перобразований в 2D пространстве.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public readonly struct Matrix2D
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

        /// <value>Первый элемент второй строки.</value>
        [FieldOffset(24)]
        public readonly double M21;
        /// <value>Второй элемент второй строки.</value>
        [FieldOffset(32)]
        public readonly double M22;
        /// <value>Третий элемент второй строки.</value>
        [FieldOffset(40)]
        public readonly double M23;
        /// <value>Первый элемент третьей строки.</value>
        
        [FieldOffset(48)]
        public readonly double M31;
        /// <value>Второй элемент третьей строки.</value>
        [FieldOffset(56)]
        public readonly double M32;
        /// <value>Третий элемент третьей строки.</value>
        [FieldOffset(64)]
        public readonly double M33;

        /// <param name="m11">Первый элемент первой строки.</param>
        /// <param name="m12">Второй элемент первой строки.</param>
        /// <param name="m13">Третий элемент первой строки.</param>
        /// <param name="m21">Первый элемент второй строки.</param>
        /// <param name="m22">Второй элемент второй строки.</param>
        /// <param name="m23">Третий элемент второй строки.</param>
        /// <param name="m31">Первый элемент третьей строки.</param>
        /// <param name="m32">Второй элемент третьей строки.</param>
        /// <param name="m33">Третий элемент третьей строки.</param>
        public Matrix2D(double m11, double m12, double m13,
                        double m21, double m22, double m23,
                        double m31, double m32, double m33)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;

            M21 = m21;
            M22 = m22;
            M23 = m23;

            M31 = m31;
            M32 = m32;
            M33 = m33;
        }

        /// <value>
        /// Возвращает матрицу мультипликативного тождества.
        /// </value>
        public static ref readonly Matrix2D Identity => ref identity;
        private static readonly Matrix2D identity = new Matrix2D
        (
            1d, 0d, 0d,
            0d, 1d, 0d,
            0d, 0d, 1d
        );

        /// <summary>
        /// Создает матрицу трансляции на основе заданного вектора <paramref name="translation"/>.
        /// </summary>
        /// <param name="translation">Значение трансляции по каждой оси.</param>
        /// <returns>Матрица трансляции.</returns>
        public static Matrix2D CreateTranslation(in Vector2D translation)
        {
            Matrix2D result = new Matrix2D(1d,      0d,         translation.X,
                                           0d,      1d,         translation.Y,
                                           0d,      0d,         1d);
            return result;
        }

        /// <summary>
        /// Создает матрицу масштабирования на основе заданного масштаба вектора <paramref name="scale"/>.
        /// </summary>
        /// <param name="scale">Вектор, который содержит значение масштабирования по каждой оси.</param>
        /// <returns>Матрица масштабирования.</returns>
        public static Matrix2D CreateScale(in Vector2D scale)
        {
            Matrix2D result = new Matrix2D(scale.X,     0d,         0d,
                                           0d,          scale.Y,    0d,
                                           0d,          0d,         1d);
            return result;
        }

        /// <summary>
        /// Создает матрицу масштабирования на основе заданного масштаба вектора <paramref name="scale"/>
        /// и относительно центральной точки <paramref name="center"/>.
        /// </summary>
        /// <param name="scale">Вектор, который содержит значение масштабирования по каждой оси.</param>
        /// <param name="center">Центральная точка.</param>
        /// <returns>Матрица масштабирования.</returns>
        public static Matrix2D CreateScale(in Vector2D scale, in Point2D center)
        {

            Matrix2D result = new Matrix2D(scale.X,     0d,         center.X * (1 - scale.X),
                                           0d,          scale.Y,    center.Y * (1 - scale.Y),
                                           0d,          0d,         1d);

            return result;
        }

        /// <summary>
        /// Cоздает матрицу для поворота точек вокруг оси под заданным углом <paramref name="radians"/>
        /// и относительно центральной точки <paramref name="center"/>.
        /// </summary>
        /// <param name="radians">Значение угла поворота вокруг оси в радианах.</param>
        /// <param name="center">Центральная точка.</param>
        /// <returns>Матрица поворота.</returns>
        public static Matrix2D CreateRotation(double radians, in Point2D center)
        {
            double a = center.X, b = center.Y;


            double cos0 = Math.Cos(radians);
            double sin0 = Math.Sin(radians);

            Matrix2D result = new Matrix2D(cos0,        -sin0,      a * (1 - cos0) + b * sin0,
                                           sin0,        cos0,       b * (1 - cos0) - a * sin0,
                                           0d,          0d,         1d);

            return result;
        }
    }
}
