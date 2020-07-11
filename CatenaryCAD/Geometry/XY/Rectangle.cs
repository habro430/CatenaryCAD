using CatenaryCAD.Geometry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD.Geometry
{
    [Serializable]
    public sealed class Rectangle : AbstractGeometry<XY>
    {
        public Rectangle(Point<XY> center, double width, double height)
        {
            Vertices = new Point<XY>[]
            {
                new Point<XY>((center.Value.X - height / 2, center.Value.Y + width / 2)),
                new Point<XY>((center.Value.X + height / 2, center.Value.Y + width / 2)),
                new Point<XY>((center.Value.X + height / 2, center.Value.Y - width / 2)),
                new Point<XY>((center.Value.X - height / 2, center.Value.Y - width / 2))
            };

            Edges = new (Point<XY>, Point<XY>)[] { (Vertices[0], Vertices[1]), 
                                                   (Vertices[1], Vertices[2]), 
                                                   (Vertices[2], Vertices[3]), 
                                                   (Vertices[3], Vertices[0]) };
        }
    }

}
