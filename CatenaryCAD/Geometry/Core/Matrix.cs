using System;

namespace CatenaryCAD.Geometry.Core
{
    [Serializable]
    public struct Matrix : IEquatable<Matrix>
    {
        public readonly double M11;
        public readonly double M12;
        public readonly double M13;
        public readonly double M14;

        public readonly double M21;
        public readonly double M22;
        public readonly double M23;
        public readonly double M24;

        public readonly double M31;
        public readonly double M32;
        public readonly double M33;
        public readonly double M34;

        public readonly double M41;
        public readonly double M42;
        public readonly double M43;
        public readonly double M44;

        public Matrix(double m11, double m12, double m13,
                      double m21, double m22, double m23,
                      double m31, double m32, double m33,
                      double m41, double m42, double m43)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = 0d;
            M21 = m21; M22 = m22; M23 = m23; M24 = 0d;
            M31 = m31; M32 = m32; M33 = m33; M34 = 0d;
            M41 = m41; M42 = m42; M43 = m43; M44 = 1d;
        }

        public static readonly Matrix Identity = new Matrix
        (
            m11: 1d, m12: 0d, m13: 0d,
            m21: 0d, m22: 1d, m23: 0d,
            m31: 0d, m32: 0d, m33: 1d,
            m41: 0d, m42: 0d, m43: 0d
        );

        public static Matrix CreateOffset(Vector offset)
        {
            return new Matrix
            (
               m11: 1d, m12: 0d, m13: 0d,
               m21: 0d, m22: 1d, m23: 0d,
               m31: 0d, m32: 0d, m33: 1d,
               m41: offset.X, m42: offset.Y, m43: offset.Z
            );
        }
        public static Matrix CreateScale(Vector scale)
        {
            return new Matrix
            (
               m11: scale.X, m12: 0d, m13: 0d,
               m21: 0d, m22: scale.Y, m23: 0d,
               m31: 0d, m32: 0d, m33: scale.Z,
               m41: 0d, m42: 0d, m43: 0d
            );
        }

        public static Matrix CreateRotate(Vector axis, double radians)
        {
            Vector v = axis.Normalize;
            double c = Math.Cos(radians);
            double s = Math.Sin(radians);

            return new Matrix
            (
                m11: c + Math.Pow(v.X, 2) * (1 - c),
                m12: v.X * v.Y * (1 - c) - v.Z * s,
                m13: v.X * v.Z * (1 - c) + v.Y * s,

                m21: v.Y * v.X * (1 - c) + v.Z * s,
                m22: c + Math.Pow(v.Y, 2) * (1 - c),
                m23: v.Y * v.Z * (1 - c) - v.X * s,

                m31: v.Z * v.X * (1 - c) - v.Y * s,
                m32: v.Z * v.Y * (1 - c) + v.X * s,
                m33: c + Math.Pow(v.Z, 2) * (1 - c),

                m41: 0d, m42: 0d, m43: 0d
            );
        }


        //public static Matrix operator +(Matrix m1, Matrix m2)
        //{
        //    return new Matrix(m1.M11 + m2.M11, m1.M22 + m2.M22, m1.M33 + m2.M33, m1.M44 + m2.M44,
        //                      m1.M12 + m2.M12, m1.M13 + m2.M13, m1.M14 + m2.M14, m1.M21 + m2.M21,
        //                      m1.M23 + m2.M23, m1.M24 + m2.M24, m1.M31 + m2.M31, m1.M32 + m2.M32,
        //                      m1.M34 + m2.M34, m1.M41 + m2.M41, m1.M42 + m2.M42, m1.M43 + m2.M43);
        //}
        //public static Matrix operator -(Matrix m1, Matrix m2)
        //{
        //    return new Matrix(m1.M11 - m2.M11, m1.M22 - m2.M22, m1.M33 - m2.M33, m1.M44 - m2.M44,
        //                      m1.M12 - m2.M12, m1.M13 - m2.M13, m1.M14 - m2.M14, m1.M21 - m2.M21,
        //                      m1.M23 - m2.M23, m1.M24 - m2.M24, m1.M31 - m2.M31, m1.M32 - m2.M32,
        //                      m1.M34 - m2.M34, m1.M41 - m2.M41, m1.M42 - m2.M42, m1.M43 - m2.M43);
        //}
        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return (m1.M11 == m2.M11 && m1.M22 == m2.M22 && m1.M33 == m2.M33 && m1.M44 == m2.M44 &&
                    m1.M12 == m2.M12 && m1.M13 == m2.M13 && m1.M14 == m2.M14 && m1.M21 == m2.M21 &&
                    m1.M23 == m2.M23 && m1.M24 == m2.M24 && m1.M31 == m2.M31 && m1.M32 == m2.M32 &&
                    m1.M34 == m2.M34 && m1.M41 == m2.M41 && m1.M42 == m2.M42 && m1.M43 == m2.M43);
        }
        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return (m1.M11 != m2.M11 || m1.M12 != m2.M12 || m1.M13 != m2.M13 || m1.M14 != m2.M14 ||
                    m1.M21 != m2.M21 || m1.M22 != m2.M22 || m1.M23 != m2.M23 || m1.M24 != m2.M24 ||
                    m1.M31 != m2.M31 || m1.M32 != m2.M32 || m1.M33 != m2.M33 || m1.M34 != m2.M34 ||
                    m1.M41 != m2.M41 || m1.M42 != m2.M42 || m1.M43 != m2.M43 || m1.M44 != m2.M44);
        }

        public bool Equals(Matrix m) => this == m;
        public override bool Equals(object obj) => (obj is Matrix m) && (this == m);

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.CombineMany(new double[] { M11, M12, M13, M14,
                                                           M21, M22, M23, M24,
                                                           M31, M32, M33, M34,
                                                           M41, M42, M43, M44});
            }
        }
        public override string ToString() => string.Format($"Matrix {{ M11:{M11}, M12:{M12}, M13:{M13}, M14:{M14}, " +
                                                                     $"M21:{M21}, M22:{M22}, M23:{M23}, M24:{M24}, " +
                                                                     $"M31:{M31}, M32:{M32}, M33:{M33}, M34:{M34}, " +
                                                                     $"M41:{M41}, M42:{M42}, M43:{M43}, M44:{M44} }}");
    }
    public static partial class Extensions
    {

    }
}
