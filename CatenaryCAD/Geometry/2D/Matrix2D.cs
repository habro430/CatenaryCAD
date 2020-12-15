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
        [FieldOffset(0)]
        public readonly double M11;
        [FieldOffset(8)]
        public readonly double M12;
        [FieldOffset(16)]
        public readonly double M13;

        [FieldOffset(24)]
        public readonly double M21;
        [FieldOffset(32)]
        public readonly double M22;
        [FieldOffset(40)]
        public readonly double M23;

        [FieldOffset(48)]
        public readonly double M31;
        [FieldOffset(56)]
        public readonly double M32;
        [FieldOffset(64)]
        public readonly double M33;

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

        public static ref readonly Matrix2D Identity => ref identity;
        private static readonly Matrix2D identity = new Matrix2D
        (
            1d, 0d, 0d,
            0d, 1d, 0d,
            0d, 0d, 1d
        );

        public static Matrix2D CreateTranslation(in Vector2D p)
        {
            Matrix2D result = new Matrix2D(1d, 0d, p.X,
                                           0d, 1d, p.Y,
                                           0d, 0d, 1d);
            return result;
        }
        public static Matrix2D CreateScale(in Vector2D s)
        {
            Matrix2D result = new Matrix2D(s.X, 0d, 0d,
                                           0d, s.Y, 0d,
                                           0d, 0d, 1d);
            return result;
        }
        public static Matrix2D CreateScale(in Vector2D s, in Point2D c)
        {

            Matrix2D result = new Matrix2D(s.X, 0d, c.X * (1 - s.X),
                                           0d, s.Y, c.Y * (1 - s.Y),
                                           0d, 0d, 1d);

            return result;
        }
        public static Matrix2D CreateRotation(double radians, in Point2D center)
        {
            double a = center.X, b = center.Y;


            double cos0 = Math.Cos(radians);
            double sin0 = Math.Sin(radians);

            Matrix2D result = new Matrix2D(cos0, -sin0, a * (1 - cos0) + b * sin0,
                                           sin0, cos0, b * (1 - cos0) - a * sin0,
                                           0d, 0d, 1d);

            return result;
        }
    }
}
