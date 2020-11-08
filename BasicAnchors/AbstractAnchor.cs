using BasicAnchors.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using System;
using System.Linq;
using System.Resources;
using System.Runtime.Caching;
using System.Text;

namespace BasicAnchors
{
    [Serializable]
    public abstract class AbstractAnchor : Anchor
    {
        protected IShape[] Geometry2D = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                                       new Triangle(new Point2D(900,0), new Point2D(1050, 100), new Point2D(1050,-100)) };
        protected IMesh[] Geometry3D;

        [NonSerialized]
        private static ObjectCache Cache = new MemoryCache(typeof(AbstractAnchor).Name);

        protected internal static Mesh GetOrCreateFromCache(string key)
        {
            if (!Cache.Contains(key))
            {
                ResourceManager rm = new ResourceManager(typeof(Resources));
                string model = Encoding.Default.GetString(Convert.FromBase64String(rm.GetString(key)));
                Cache.Set(key, Mesh.FromObj(model), new CacheItemPolicy());
            }
            return Cache.Get(key) as Mesh;
        }
        public override IMesh[] GetGeometryForLayout() => Geometry3D;
        public override IShape[] GetGeometryForScheme()
        {
            var mast_position = new Point2D(Parent.Position.X, Parent.Position.Y);
            var anchor_position = new Point2D(Position.X, Position.Y);

            Point2D dockingjoint = (Parent as Mast).GetPointForDockingJoint(
                        new Ray2D(anchor_position, anchor_position.VectorTo(mast_position))) ?? mast_position;

            double angle = mast_position.VectorTo(dockingjoint).AngleTo(Vector2D.AxisX);

            dockingjoint = dockingjoint.TransformBy(Matrix2D.CreateRotation(angle, mast_position));
            anchor_position = anchor_position.TransformBy(Matrix2D.CreateRotation(angle, mast_position));

            return Geometry2D.Select(s => s.TransformBy(Matrix2D.CreateTranslation(anchor_position.VectorTo(dockingjoint)))).ToArray();
        }
    }
}
