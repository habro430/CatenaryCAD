using BasicMasts.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Helpers;
using CatenaryCAD.Components;
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

        public override IMesh[] GetLayoutGeometry()
        {
            var basement = Childrens.Where(child => child is IFoundation).Single();
            var matrix = Matrix3D.CreateTranslation(Point3D.Origin.GetVectorTo(basement.GetDockingPoint(this, new Ray3D()).Value));

            return Components.SelectMany(p => p.Geometry.Select(g=> g.TransformBy(matrix))).ToArray();
        }

        public override bool CheckAvailableDocking(IModel from) => true;

        public override Point2D? GetDockingPoint(IModel from, Ray2D ray)
        {
            var mast_position = new Point2D(Position.X, Position.Y);
            var mast_direction = new Vector2D(Direction.X, Direction.Y);

            Matrix2D matrix = Matrix2D.CreateRotation(-mast_direction.GetAngleTo(Vector2D.AxisX), mast_position) *
                              Matrix2D.CreateTranslation(Point2D.Origin.GetVectorTo(mast_position));

            var intersections = GetSchemeGeometry().SelectMany(shape => ray.GetIntersections(shape.TransformBy(matrix)));

            Point2D control_point = ray.Origin + ray.Direction;
            return intersections.Aggregate((first, second) => first.GetDistanceTo(control_point) < second.GetDistanceTo(control_point) ? first : second);
        }

        public override Point3D? GetDockingPoint(IModel from, Ray3D ray)
        {
            throw new NotImplementedException();
        }

    }
}
