using BasicMasts.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Objects;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        protected AbstractGeometry<XY>[] Geometry2D;
        protected AbstractGeometry<XYZ>[] Geometry3D;

        protected IProperty[] Propertes;

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
        internal static Dictionary<string, Type> GetInheritedMastsFor(Type abst_mast_type)
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

        public IProperty[] GetProperties() => Propertes;
        public void GetGeometry(out AbstractGeometry<XY>[] xy, out AbstractGeometry<XYZ>[] xyz)
        {
            xy = Geometry2D;
            xyz = Geometry3D;
        }

        public IPart[] GetParts() => throw new NotImplementedException();

    }
}
