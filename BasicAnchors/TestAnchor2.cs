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
    [ModelName("TestAnchor2")]
    [ModelDescription("")]
    public class TestAnchor2 : Anchor
    {
        IShape[] geom = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                       new Triangle(new Point2D(900,0), new Point2D(1050, 100), new Point2D(1050,-100)) };

        public TestAnchor2()
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
        }
    }
}
