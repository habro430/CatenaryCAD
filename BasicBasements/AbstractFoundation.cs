using BasicBasements.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using System;
using System.Linq;
using System.Resources;
using System.Runtime.Caching;
using System.Text;

namespace BasicFoundations
{
    [Serializable]
    public abstract class AbstractFoundation : Foundation
    {
        [NonSerialized]
        private static ObjectCache Cache = new MemoryCache(typeof(AbstractFoundation).Name);

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

        public override IShape[] GetSchemeGeometry() => null;
        public override IMesh[] GetLayoutGeometry() => Components.SelectMany(p => p.Geometry).ToArray();

        public override bool CheckAvailableDocking(IModel from) => true;

        public override Point2D? GetDockingPoint(IModel from, Ray2D ray) => Point2D.Origin;
        public override Point3D? GetDockingPoint(IModel from, Ray3D ray) => Point3D.Origin;
    }
}
