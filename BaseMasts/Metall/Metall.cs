using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Core;
using CatenaryCAD.Objects;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicMasts
{
    [Serializable]
    [CatenaryObject("Металлическая", "")]
    public class Metall : AbstractMast
    {
        public override event Action Updated;

        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Metall() => InheritedMasts = Extensions.GetInheritedMastsFor(typeof(Metall));

        public Metall()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new AbstractGeometry[] { new Rectangle(new Point(), 600, 600) };

            var tmp_props = new List<AbstractProperty>();

            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_subtype = new Property<Type>("02_mast_metall_type", "Марка стойки", "Стойка", PropertyFlags.RefreshAfterChange);

                mast_subtype.DictionaryValues = InheritedMasts;
                mast_subtype.Value = mast_subtype.DictionaryValues.Values.FirstOrDefault();

                tmp_props.Add(mast_subtype);
            }
            else
            {
                Property<string> mast_subtype = new Property<string>("02_mast_metall_type", "Марка стойки", "Стойка");
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

            m_len.Updated += (val) =>
            {
                //читаем и генериуем геометрию для 3D режима в зависимости от длинны
                switch (val)
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
            };

            m_len.Value = m_len.DictionaryValues.Values.FirstOrDefault();

            tmp_props.Add(m_len);

            //сохраняем сгенерированный список параметров 
            Propertes = tmp_props.ToArray();
        }
    }
}
