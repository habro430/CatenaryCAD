using CatenaryCAD;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Linq;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("TestAnchor1")]
    [ModelDescription("")]
    public class TestAnchor1 : Anchor
    {
        IShape[] geom = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                       new Triangle(new Point2D(900,0), new Point2D(1050, 100), new Point2D(1050,-100)) };

        public TestAnchor1()
        {
            
        }

        public override IMesh[] GetGeometryForLayout() => null;
        public override IShape[] GetGeometryForScheme()
        {
            var mast_position = new Point2D(Parent.Position.X, Parent.Position.Y);
            var anchor_position = new Point2D(Position.X, Position.Y);

            Point2D dockingjoint = (Parent as Mast).GetPointForDockingJoint(
                        new Ray2D(anchor_position, anchor_position.VectorTo(mast_position))) ?? mast_position;

            double angle = mast_position.VectorTo(dockingjoint).AngleTo(Vector2D.AxisX);

            dockingjoint = dockingjoint.TransformBy(Matrix2D.CreateRotation(angle, mast_position));
            anchor_position = anchor_position.TransformBy(Matrix2D.CreateRotation(angle, mast_position));

            return geom.Select(s => s.TransformBy(Matrix2D.CreateTranslation(anchor_position.VectorTo(dockingjoint)))).ToArray();

            //var mast_position = new Point2D(Parent.Position.X, Parent.Position.Y);
            //var anchor_position = new Point2D(Position.X, Position.Y);

            //Point2D docking_position = (Parent as Mast).GetPointForDockingJoint(
            //            new Ray2D(anchor_position, anchor_position.VectorTo(mast_position))) ?? mast_position;

            //double angle_docking = mast_position.VectorTo(docking_position).AngleTo(Vector2D.AxisX);
            //double angle_anchor = mast_position.VectorTo(anchor_position).AngleTo(Vector2D.AxisX);

            //double angle_correction = anchor_position.VectorTo(mast_position).AngleTo(anchor_position.VectorTo(docking_position));

            //docking_position = docking_position.TransformBy(Matrix2D.CreateRotation(angle_docking, mast_position));
            //anchor_position = anchor_position.TransformBy(Matrix2D.CreateRotation(angle_anchor, mast_position));

            //double c = 2 * anchor_position.VectorTo(docking_position).Length * (Math.Abs(angle_correction) / 2);
            //double h = anchor_position.VectorTo(docking_position).Length * (1 - Math.Cos(angle_correction  / 2));
            //double l = Math.Abs(angle_correction) * anchor_position.VectorTo(docking_position).Length;

            //return geom
            //            .Select(s => s.TransformBy(Matrix2D.CreateTranslation(anchor_position.VectorTo(docking_position)-new Vector2D(Math.Abs(angle_correction) * 100, 0))))
            //            .Select(s => s.TransformBy(Matrix2D.CreateRotation(angle_correction, Point2D.Origin)))

            //            .ToArray();
        }
    }
}
