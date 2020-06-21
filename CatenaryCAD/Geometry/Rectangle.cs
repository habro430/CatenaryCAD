using CatenaryCAD.Geometry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Rectangle : AbstractGeometry
    {
        public Rectangle(Point center, double width, double height)
        {

            Vertices = new Point[] 
            {
                new Point(center.X - height / 2, center.Y + width / 2, 0),
                new Point(center.X + height / 2, center.Y + width / 2, 0),
                new Point(center.X + height / 2, center.Y - width / 2, 0),
                new Point(center.X - height / 2, center.Y - width / 2, 0)
            };
            Edges = new (int, int)[] { (0, 1), (1, 2), (2, 3), (3, 0) };
        }
    }

}
