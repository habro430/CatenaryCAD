using BasicMasts.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Caching;
using System.Text;

namespace BasicMasts
{
    [Serializable]
    public abstract class AbstractMast : Mast
    {
        protected IShape[] Geometry2D;
        protected IMesh[] Geometry3D;

        [NonSerialized]
        private static ObjectCache Cache = new MemoryCache(typeof(AbstractMast).Name);

        protected internal static Mesh GetOrCreateFromCache(string key)
        {
            //есть ли 3d модель в кэше
            if (!Cache.Contains(key))
            {
                //если нет то читаем 3d модель из ресурсов, генерируем и кэшируем 
                ResourceManager rm = new ResourceManager(typeof(Resources));

                //декодируем модель из base64
                string model = Encoding.Default.GetString(Convert.FromBase64String(rm.GetString(key)));

                //генерируем модель и пишем в кэш
                Cache.Set(key, Mesh.FromObj(model), new CacheItemPolicy());
            }

            //читаем модель из кэша и возвращаем
            return Cache.Get(key) as Mesh;
        }
        protected internal static Dictionary<string, Type> GetInheritedMastsFor(Type abst_mast_type)
        {
            //получаем все не абстрактные обьекты производные от 'abst_mast_type' и имеющие атрибут MastTypeAttribute
            var masts = Assembly.GetExecutingAssembly().GetTypes()
                .Where(abstr => !abstr.IsAbstract)
                .Where(bastype => bastype.BaseType == abst_mast_type)
                .Where(attr => Attribute.IsDefined(attr, typeof(MastAttribute), false))
                .ToArray();

            //получем словарь из MastTypeAttribute и типа производного от 'abst_mast_type' класса
            return masts.Select((type) => new
            {
                type,
                atrr = type.GetCustomAttributes(typeof(MastAttribute), false)
                            .FirstOrDefault() as MastAttribute
            }).ToDictionary(p => p.atrr.Type, p => p.type);
        }


        public override IMesh[] GetGeometryForLayout() => Geometry3D;
        public override IShape[] GetGeometryForScheme() => Geometry2D;

        public override Point2D? GetPointForDockingJoint(Ray2D ray)
        {
            var position = new Point2D(Position.X, Position.Y);
            var direction = new Vector2D(Direction.X, Direction.Y);

            List<Point2D> intersections = new List<Point2D>();

            foreach (var shape in Geometry2D)
            {
                var sh = shape.TransformBy(Matrix2D.CreateTranslation(Point2D.Origin.GetVectorTo(position)))
                              .TransformBy(Matrix2D.CreateRotation(-direction.GetAngleTo(Vector2D.AxisX), position));

                Point2D[] tmp_intersections = ray.GetIntersections(sh);

                if (tmp_intersections != null)
                    intersections.AddRange(tmp_intersections);
            }

            if (intersections.Count > 0)
            {
                Point2D intersection = intersections[0];
                Point2D control_point = ray.Origin + ray.Direction;

                foreach (var point in intersections)
                    if (point.GetDistanceTo(control_point) < intersection.GetDistanceTo(control_point))
                        intersection = point;

                return intersection;
            }
            else
                return null;
        }
        public override Point3D? GetPointForDockingJoint(Ray3D ray)
        {
            throw new NotImplementedException();
        }

    }
}
