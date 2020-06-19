using System;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public struct Matrix3D : IEquatable<Matrix3D>
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

        public static readonly Matrix3D Identity = new Matrix3D
        (
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        public static bool operator ==(Matrix3D m1, Matrix3D m2)
        {
            return (m1.M11 == m2.M11 && m1.M22 == m2.M22 && m1.M33 == m2.M33 && m1.M44 == m2.M44 &&
                    m1.M12 == m2.M12 && m1.M13 == m2.M13 && m1.M14 == m2.M14 && m1.M21 == m2.M21 &&
                    m1.M23 == m2.M23 && m1.M24 == m2.M24 && m1.M31 == m2.M31 && m1.M32 == m2.M32 &&
                    m1.M34 == m2.M34 && m1.M41 == m2.M41 && m1.M42 == m2.M42 && m1.M43 == m2.M43);
        }
        public static bool operator !=(Matrix3D m1, Matrix3D m2)
        {
            return (m1.M11 != m2.M11 || m1.M12 != m2.M12 || m1.M13 != m2.M13 || m1.M14 != m2.M14 ||
                    m1.M21 != m2.M21 || m1.M22 != m2.M22 || m1.M23 != m2.M23 || m1.M24 != m2.M24 ||
                    m1.M31 != m2.M31 || m1.M32 != m2.M32 || m1.M33 != m2.M33 || m1.M34 != m2.M34 ||
                    m1.M41 != m2.M41 || m1.M42 != m2.M42 || m1.M43 != m2.M43 || m1.M44 != m2.M44);
        }

        public bool Equals(Matrix3D m) => this == m;
        public override bool Equals(object obj) => (obj is Matrix3D m) && (this == m);

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
