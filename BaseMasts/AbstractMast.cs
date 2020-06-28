using BasicMasts.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Objects.Masts;
using CatenaryCAD.Properties;

using System;
using System.Resources;
using System.Runtime.Caching;
using System.Text;
using static CatenaryCAD.Extensions;

namespace BasicMasts
{
    [Serializable]
    public abstract class AbstractMast : IMast
    {
        public abstract event Action Updated;

        protected AbstractGeometry[] Geometry2D;
        protected AbstractGeometry[] Geometry3D;

        protected AbstractProperty[] Propertes;

        [NonSerialized]
        private static ObjectCache GeometryCache = new MemoryCache(typeof(Armored).Name);
        internal static Mesh GetOrCreateFromCache(string key)
        {
            //есть ли 3d модель в кэше
            if (!GeometryCache.Contains(key))
            {
                //если нет то читаем 3d модель из ресурсов, генерируем и кэшируем 
                ResourceManager rm = new ResourceManager(typeof(Resources));

                //декодируем модель из base64
                string model = Encoding.Default.GetString(Convert.FromBase64String(rm.GetString(key)));

                //генерируем модель и пишем в кэш
                GeometryCache.Set(key, Mesh.FromObj(model), new CacheItemPolicy());
            }

            //читаем модель из кэша и возвращаем
            return GeometryCache.Get(key) as Mesh;
        }

        public AbstractProperty[] GetProperties() => Propertes;
        public AbstractGeometry[] GetGeometry(ViewType type)
        {
            switch (type)
            {
                case ViewType.Geometry2D: return Geometry2D;
                case ViewType.Geometry3D: return Geometry3D;

                default: return Geometry2D;
            }
        }
    }
}
