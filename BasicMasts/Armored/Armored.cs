﻿using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Core;
using CatenaryCAD.Objects;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicMasts
{
    [Serializable]
    [CatenaryObject("Железобетонная", "")]
    public class Armored : AbstractMast
    {
        public override event Action Updated;

        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Armored() => InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Armored));

        public Armored()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new AbstractGeometry[] { new Circle(new Point3(), 300, 20) };

            var tmp_props = new List<IProperty>();

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

            m_len.Updated += (val) =>
            {
                //читаем и генериуем геометрию для 3D режима в зависимости от длинны
                switch (val)
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
            };
            m_len.Value = m_len.DictionaryValues.Values.FirstOrDefault();

            tmp_props.Add(m_len);

            //сохраняем сгенерированный список параметров 
            Propertes = tmp_props.ToArray();
        }
    }
}
