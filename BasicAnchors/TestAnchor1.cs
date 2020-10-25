﻿using CatenaryCAD;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestAnchor1")]
    [ModelDescription("")]
    public class TestAnchor1 : Anchor
    {
        IShape[] geom = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                       new Rectangle(new Point2D(1000, 0), 200, 200) };

        public TestAnchor1()
        {
            
        }

        public override IMesh[] GetGeometryForLayout() => null;
        public override IShape[] GetGeometryForScheme()
        {
            var mast_position = new Point2D(Parent.Position.X, Parent.Position.Y);
            var anchor_position = new Point2D(Position.X, Position.Y);

            Point2D dockingpoint = (Parent as Mast).GetDockingJointPoint(
                        new Ray2D(anchor_position, anchor_position.VectorTo(mast_position))) ?? mast_position;

            double angle = mast_position.VectorTo(dockingpoint).AngleTo(Vector2D.AxisX);

            dockingpoint = dockingpoint.TransformBy(Matrix2D.CreateRotation(angle, mast_position));
            anchor_position = anchor_position.TransformBy(Matrix2D.CreateRotation(angle, mast_position));

            return geom.Select(s => s.TransformBy(Matrix2D.CreateTranslation(anchor_position.VectorTo(dockingpoint)))).ToArray();
        }
    }
}
