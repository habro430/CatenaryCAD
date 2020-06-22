using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Caching;
using System.Text;

namespace CatenaryCAD.Objects.Masts
{
    [Serializable]
    [Mast("Металлическая")]
    public class Metall : AbstractMast
    {
        public override event Action Updated;

        //кэш 3d моделей загружаемых из ресурсов при первом доступе к ним
        [NonSerialized]
        private static ObjectCache GeometryCache = new MemoryCache(typeof(Metall).Name);

        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Metall() => InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Metall));

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

        public Metall()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new AbstractGeometry[] { new Rectangle(new Geometry.Core.Point(), 600, 600) };

            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_subtype = new Property<Type>("02_mast_metall_type", "Марка стойки", "Стойка", PropertyFlags.RefreshAfterChange);

                mast_subtype.DictionaryValues = InheritedMasts;
                mast_subtype.Value = mast_subtype.DictionaryValues.Values.FirstOrDefault();

                propertes.Add(mast_subtype);
            }
            else
            {
                propertes.Add(new Property<string>("02_mast_metall_type", "Тип", "Стойка"));
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

            propertes.Add( m_len);



        }
        private void len_updated(int value)
        {
            //читаем и генериуем геометрию для 3D режима в зависимости от длинны
            switch (value)
            {
                case 10000:
                    Geometry3D = new AbstractGeometry[] { GetOrCreateFromCache("m_10") };
                    break;

                case 12000:
                    Geometry3D = new AbstractGeometry[] { GetOrCreateFromCache("m_12") };
                    break;

                case 15000:
                    Geometry3D = new AbstractGeometry[] { GetOrCreateFromCache("m_15") };
                    break;
            }

            Updated?.Invoke();
        }

        private List<AbstractProperty> propertes = new List<AbstractProperty>();
        public override AbstractProperty[] GetProperties() => propertes.ToArray();
    }
}
