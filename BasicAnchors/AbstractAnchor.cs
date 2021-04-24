using BasicAnchors.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Helpers;
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
        private IShape[] geometry = new IShape[] { new Line(new Point2D(0, 0), new Point2D(900, 0)),
                                                       new Triangle(new Point2D(900,0), new Point2D(1050, 100), new Point2D(1050,-100)) };
        private IShape[] geometry_notavalible = new IShape[] { new Line(new Point2D(300, -150), new Point2D(600, 150)),
                                                        new Line(new Point2D(300, 150), new Point2D(600, -150)) };
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
        public override IShape[] GetGeometry()
        {
            var mast_position = new Point2D(Parent.Position.X, Parent.Position.Y);
            var anchor_position = new Point2D(Position.X, Position.Y);

            var docking_point = Parent.GetDockingPoint(this, new Ray2D(anchor_position, anchor_position.GetVectorTo(mast_position)));

            var anchor_to_mast = anchor_position.GetVectorTo(mast_position);
            var anchor_to_docking = anchor_position.GetVectorTo(docking_point.Value);

            var matrix = Matrix2D.CreateRotation(anchor_to_mast.GetAngleTo(anchor_to_docking), Point2D.Origin) * 
                         Matrix2D.CreateTranslation(Vector2D.AxisX * -anchor_to_docking.GetLength());

            if(Parent.CheckAvailableDocking(this, new Ray2D(anchor_position, anchor_position.GetVectorTo(mast_position))))
                return geometry.DeepClone().Select(g => g.TransformBy(matrix)).ToArray();
            else
                return geometry.DeepClone().Union(geometry_notavalible.DeepClone()).Select(g => g.TransformBy(matrix)).ToArray();
        }

        public override bool CheckAvailableDocking(IModel from, Ray2D ray)
        {
            throw new NotImplementedException();
        }
        public override bool CheckAvailableDocking(IModel from, Ray3D ray)
        {
            throw new NotImplementedException();
        }

        public override Point2D? GetDockingPoint(IModel from, Ray2D ray)
        {
            throw new NotImplementedException();
        }
        public override Point3D? GetDockingPoint(IModel from, Ray3D ray)
        {
            throw new NotImplementedException();
        }
    }
}
