using BasicMasts;
using BasicMasts.Properties;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Core;
using CatenaryCAD.Objects;
using CatenaryCAD.Objects.Masts;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Caching;
using System.Text;
using static CatenaryCAD.Extensions;

namespace BasicMasts
{
    [Serializable]
    [CatenaryObject("ЖелезобетоннаяBasic", "")]
    public class Armored : AbstractMast
    {
        public override event Action Updated;

        private AbstractGeometry[] Geometry2D;
        private AbstractGeometry[] Geometry3D;

        private AbstractProperty[] Propertes;

        //кэш 3d моделей загружаемых из ресурсов при первом доступе к ним
        [NonSerialized]
        private static ObjectCache GeometryCache = new MemoryCache(typeof(Armored).Name);

        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Armored() => InheritedMasts = BasicMasts.Extensions.GetInheritedMastsFor(typeof(Armored));

        private static Mesh GetOrCreateFromCache(string key)
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

        public Armored()
        {
            var tmp_props = new List<AbstractProperty>();

            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_subtype = new Property<Type>("02_mast_armored_type", "Марка стойки", "Стойка",  PropertyFlags.RefreshAfterChange);
                
                mast_subtype.DictionaryValues = InheritedMasts;
                mast_subtype.Value = mast_subtype.DictionaryValues.Values.FirstOrDefault();

                tmp_props.Add(mast_subtype);
            }
            else
            {
                Property<string> mast_subtype = new Property<string>("02_mast_armored_type", "Марка стойки", "Стойка");
                mast_subtype.Value = string.Empty;

                tmp_props.Add(mast_subtype);
            }

            Property<int> m_len = new Property<int>("03_mast_len", "Длинна", "Стойка", PropertyFlags.RefreshAfterChange);

            m_len.DictionaryValues = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000,

            };

            m_len.Updated += len_updated;
            m_len.Value = m_len.DictionaryValues.Values.FirstOrDefault();

            tmp_props.Add(m_len);


            //генериуем геометрию для 2D режима
            Geometry2D = new AbstractGeometry[] { new Circle(new Point(), 300, 20) };
            //сохраняем сгенерированный список параметров 
            Propertes = tmp_props.ToArray();
        }
        private void len_updated(int value)
        {
            //читаем и генериуем геометрию для 3D режима в зависимости от длинны
            switch (value)
            {
                case 10000:
                    Geometry3D = new AbstractGeometry[] { GetOrCreateFromCache("a_10") };
                    break;

                case 12000:
                    Geometry3D = new AbstractGeometry[] { GetOrCreateFromCache("a_12") }; 
                    break;

                case 15000:
                    Geometry3D = new AbstractGeometry[] { GetOrCreateFromCache("a_15") }; 
                    break;
            }

            Updated?.Invoke();
        }

        public override AbstractProperty[] GetProperties() => Propertes;
        public override AbstractGeometry[] GetGeometry(ViewType type)
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
