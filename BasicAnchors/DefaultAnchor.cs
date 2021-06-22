using BasicAnchors.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Properties;
using System;
using System.Linq;
using System.Resources;
using System.Runtime.Caching;
using System.Text;

namespace BasicAnchors
{
    [Serializable]
    public abstract class DefaultAnchor: Anchor
    {
        private IShape[] geometry = new IShape[] { new Line(new Point2D(0, 0), new Point2D(8, 0)),
                                                       new Triangle(new Point2D(8,0), new Point2D(10, 1), new Point2D(10,-1)) };
        private IShape[] geometry_notavalible = new IShape[] { new Line(new Point2D(3, -1.5d), new Point2D(6, 1.5d)),
                                                        new Line(new Point2D(3, 1.5d), new Point2D(6, -1.5d)) };
        [NonSerialized]
        private static ObjectCache Cache = new MemoryCache(typeof(DefaultAnchor).Name);

        protected internal static Mesh GetOrCreateFromCache(string key)
        {
            if (!Cache.Contains(key))
            {
                ResourceManager rm = new ResourceManager(typeof(Resources));
                string model = Encoding.Default.GetString(Convert.FromBase64String(rm.GetString(key)));
                Cache.Set(key, Mesh.FromObj(model).TransformBy(Matrix3D.CreateRotation(90 / (180/Math.PI), Point3D.Origin, Vector3D.AxisZ)), new CacheItemPolicy());
            }
            return Cache.Get(key) as Mesh;
        }
        
        public DefaultAnchor()
        {
            PropertiesDictionary.TryAdd("anchorguy_first_height", new Property<int>("Высота анкеровки НТ", "Анкеровка", 8650, null));
            PropertiesDictionary.TryAdd("anchorguy_second_height", new Property<int>("Высота анкеровки КП", "Анкеровка", 7650, null));
        }

        public override IShape[] GetSchemeGeometry()
        {
            var mast_position = new Point2D(Parent.Position.X, Parent.Position.Y);
            var anchor_position = new Point2D(Position.X, Position.Y);

            var anchor_position_SCALED = anchor_position.TransformBy(Matrix2D.CreateScale(Scale, mast_position));
            var docking_point = Parent.GetDockingPoint(this, new Ray2D(anchor_position_SCALED, anchor_position_SCALED.GetVectorTo(mast_position)));


            if (docking_point.HasValue)
            {
                var anchor_to_mast = anchor_position_SCALED.GetVectorTo(mast_position).GetNormalize();
                var anchor_to_docking = anchor_position.GetVectorTo(docking_point.Value);

                var vect = Vector2D.AxisX * -anchor_to_docking.GetLength() / Scale;

                var matrix = Matrix2D.CreateRotation(anchor_to_mast.GetAngleTo(anchor_to_docking), Point2D.Origin) *
                             Matrix2D.CreateTranslation(vect);

                if (Parent.CheckAvailableDocking(this))
                    return geometry.Select(g => g.TransformBy(matrix)).ToArray();
                else
                    return geometry.Union(geometry_notavalible).Select(g => g.TransformBy(matrix)).ToArray();
            }
            return geometry.Union(geometry_notavalible).ToArray();
        }

        public override IMesh[] GetLayoutGeometry() => Components.SelectMany(p => p.Geometry).ToArray();

        public override bool CheckAvailableDocking(IModel from)
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
