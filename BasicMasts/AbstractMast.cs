using BasicMasts.Properties;

using CatenaryCAD;
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
using static CatenaryCAD.Extensions;

namespace BasicMasts
{
    [Serializable]
    public abstract class AbstractMast : IMast
    {
        public virtual event Action Updated;

        private Point3D position;
        private Vector3D direction;

        public Point3D Position 
        { 
            get => position;
            set
            {
                position = value;
                Updated?.Invoke();
            }
        }
        public Vector3D Direction 
        {
            get => direction;
            set
            {
                direction = value.Normalize();
                Updated?.Invoke();
            }
        }

        protected IShape[] Geometry2D;
        protected IMesh[] Geometry3D;

        protected ConcurrentHashSet<IProperty> Properties = new ConcurrentHashSet<IProperty>();

        [NonSerialized]
        private static ObjectCache Cache = new MemoryCache(typeof(Armored).Name);


        internal static Mesh GetOrCreateFromCache(string key)
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

        public IProperty[] GetProperties() => Properties.ToArray();

        public IPart[] GetParts() => throw new NotImplementedException();

        public IMesh[] GetGeometryForLayout() => Geometry3D;
        public IShape[] GetGeometryForScheme() => Geometry2D;
    }
}
