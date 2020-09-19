using CatenaryCAD.Geometry.Shapes;
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

                var t = (q - p).CrossProduct(s) / r.CrossProduct(s);

                if (0 <= t && t <= 1) points.Add(p + (r * t));
            }

            intersections = points.ToArray();

            if (points.Count != 0) return true;
            else return false;
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