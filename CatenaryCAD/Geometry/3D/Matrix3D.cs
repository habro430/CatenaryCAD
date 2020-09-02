using CatenaryCAD.Geometry.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public readonly struct Matrix3D : IMatrix
    {
        [FieldOffset(0)]
        public readonly double M11;
        [FieldOffset(8)]
        public readonly double M12;
        [FieldOffset(16)]
        public readonly double M13;
        [FieldOffset(24)]
        public readonly double M14;

        [FieldOffset(32)]
        public readonly double M21;
        [FieldOffset(40)]
        public readonly double M22;
        [FieldOffset(48)]
        public readonly double M23;
        [FieldOffset(56)]
        public readonly double M24;

        [FieldOffset(64)]
        public readonly double M31;
        [FieldOffset(72)]
        public readonly double M32;
        [FieldOffset(80)]
        public readonly double M33;
        [FieldOffset(88)]
        public readonly double M34;

        [FieldOffset(96)]
        public readonly double M41;
        [FieldOffset(104)]
        public readonly double M42;
        [FieldOffset(112)]
        public readonly double M43;
        [FieldOffset(120)]
        public readonly double M44;

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

        public static ref readonly Matrix3D Identity => ref identity;
        private static readonly Matrix3D identity = new Matrix3D
        (
            1d, 0d, 0d, 0d,
            0d, 1d, 0d, 0d,
            0d, 0d, 1d, 0d,
            0d, 0d, 0d, 1d
        );

        public static Matrix3D CreateTranslation(in Point3D p)
        {
            Matrix3D result = new Matrix3D(1d, 0d, 0d, 0d,
                                           0d, 1d, 0d, 0d,
                                           0d, 0d, 1d, 0d,
                                           p.X, p.Y, p.Z, 1d);

            return result;
        }

        public static Matrix3D CreateScale(in Vector3D s)
        {
            Matrix3D result = new Matrix3D(s.X, 0d, 0d, 0d,
                                           0d, s.Y, 0d, 0d,
                                           0d, 0d, s.Z, 0d,
                                           0d, 0d, 0d, 1d);
            return result;
        }

        public static Matrix3D CreateScale(in Vector3D s, in Point3D c)
        {
            double tx = c.X * (1 - s.X);
            double ty = c.Y * (1 - s.Y);
            double tz = c.Z * (1 - s.Z);

            Matrix3D result = new Matrix3D(s.X, 0d, 0d, 0d,
                                           0d, s.Y, 0d, 0d,
                                           0d, 0d, s.Z, 0d,
                                           tx, ty, tz, 1d);
            return result;
        }

        public static Matrix3D CreateRotation(in Vector3D axis, double radians)
        {
            double x = axis.X, y = axis.Y, z = axis.Z;
            double sa = Math.Sin(radians), ca = Math.Cos(radians);
            double xx = x * x, yy = y * y, zz = z * z;
            double xy = x * y, xz = x * z, yz = y * z;

            Matrix3D result = new Matrix3D(xx + ca * (1.0f - xx),       xy - ca * xy + sa * z,      xz - ca * xz - sa * y, 0d,
                                           xy - ca * xy - sa * z,       yy + ca * (1.0f - yy),      yz - ca * yz + sa * x, 0d,
                                           xz - ca * xz + sa * y,       yz - ca * yz - sa * x,      zz + ca * (1.0f - zz), 0d,
                                           0d, 0d, 0d, 1d);
            return result;
        }

    }
}
