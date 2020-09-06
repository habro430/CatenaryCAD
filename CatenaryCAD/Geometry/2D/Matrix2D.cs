using System;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public readonly struct Matrix2D : IMatrix
    {
        [FieldOffset(0)]
        public readonly double M11;
        [FieldOffset(8)]
        public readonly double M12;

        [FieldOffset(16)]
        public readonly double M21;
        [FieldOffset(24)]
        public readonly double M22;

        [FieldOffset(32)]
        public readonly double M31;
        [FieldOffset(40)]
        public readonly double M32;

        public Matrix2D(double m11, double m12,
                        double m21, double m22,
                        double m31, double m32)
        {
            M11 = m11;
            M12 = m12;

            M21 = m21;
            M22 = m22;

            M31 = m31;
            M32 = m32;
        }

        public static ref readonly Matrix2D Identity => ref identity;
        private static readonly Matrix2D identity = new Matrix2D
        (
            1d, 0d,
            0d, 1d,
            0d, 0d
        );

        public static Matrix2D CreateTranslation(in Point2D p)
        {
            Matrix2D result = new Matrix2D(1d, 0d,
                                           0d, 1d,
                                           p.X, p.Y);
            return result;
        }
        public static Matrix2D CreateScale(in Vector2D s)
        {
            Matrix2D result = new Matrix2D(s.X, 0d,
                                           0d, s.Y,
                                           0d, 0d);
            return result;
        }
        public static Matrix2D CreateScale(in Vector2D s, in Point2D c)
        {
            double tx = c.X * (1 - s.X);
            double ty = c.Y * (1 - s.Y);

            Matrix2D result = new Matrix2D(s.X, 0d,
                                           0d, s.Y,
                                           tx, ty);

            return result;
        }
        public static Matrix2D CreateRotation(double radians)
        {
            throw new NotImplementedException();
        }
    }
}
