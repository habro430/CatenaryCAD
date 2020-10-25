﻿using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 40)]
    public readonly struct Ray2D : IEquatable<Ray2D>, IRay
    {
        [FieldOffset(0)]
        public readonly Point2D Origin;
        [FieldOffset(16)]
        public readonly Vector2D Direction;

        public Ray2D(Point2D origin, Vector2D direction)
        {
            Origin = origin;
            Direction = direction;
        }

        #region Functions

        public bool GetIntersections(IShape shape, out Point2D[] intersections)
        {
            List<Point2D> points = new List<Point2D>();

            Point2D first = Origin;
            Point2D second = Origin + Direction;

            foreach (var edge in shape.Edges)
            {
                var p = first;
                var q = edge.First;
                var r = first.VectorTo(second);
                var s = edge.First.VectorTo(edge.Second);

                var rxs = r.CrossProduct(s);
                var qpxr = (q - p).CrossProduct(r);

                // Если r * s = 0 и (q - p) * r = 0, тогда два вектора коллинеарны.
                if (rxs == 0 && qpxr == 0)
                    continue;

                // Если r * s = 0 и (q - p) * r != 0, тогда два вектора параллельны и не пересекаются.
                if (rxs==0 && qpxr != 0)
                    continue;

                var t = (q - p).CrossProduct(s) / rxs;
                var u = (q - p).CrossProduct(r) / rxs;

                //Если r * s != 0 и 0 <= t <= 1 и 0 <= u <= 1,
                //тогда два вектора пересекаються в точке p + t * r = q + u * s.
                if (rxs != 0 && (0 <= t && t <= 1) && (0 <= u && u <= 1))
                    points.Add(p + (r * t));
            }

            intersections = points.ToArray();
            return intersections.Length > 0 ? true : false;
        }

        public override bool Equals(object obj) => obj is Ray2D r && this.Equals(r);

        public bool Equals(Ray2D other) => other.Origin.Equals(Origin) && other.Direction.Equals(Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2D TransformBy(in Matrix2D m) => new Ray2D(Origin.TransformBy(m), Direction.TransformBy(m));

        #endregion
    }
}