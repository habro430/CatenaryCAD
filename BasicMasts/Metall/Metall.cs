using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicMasts
{
    [Serializable]
    [ModelName("Металлическая")]
    [ModelDescription("Представляет базовую модель металлической стойки")]

    public class Metall : AbstractMast
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Metall() => InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Metall));

        public Metall()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new IShape[] { new Rectangle(new Point2D(), 600, 600) };

            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_subtype = new Property<Type>("02_mast_metall_type", "Марка стойки", "Стойка", ConfigFlags.RefreshAfterChange);

                mast_subtype.DictionaryValues = InheritedMasts;
                mast_subtype.Value = mast_subtype.DictionaryValues.Values.FirstOrDefault();

                Properties.Add(mast_subtype);
            }
            else
            {
                Property<string> mast_subtype = new Property<string>("02_mast_metall_type", "Марка стойки", "Стойка");
                mast_subtype.Value = string.Empty;

                Properties.Add(mast_subtype);
            }

            Property<int> m_len = new Property<int>("03_mast_len", "Длинна", "Стойка", ConfigFlags.RefreshAfterChange);

            m_len.DictionaryValues = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000,

            };

            m_len.Updated += (val) =>
            {
                if (Identifier != null)
                {
                    if (!SendMessageToHandler(HandlerMessages.TryModify))
                        return;
                }

                //читаем и генериуем геометрию для 3D режима в зависимости от длинны
                switch (val)
                {
                    case 10000:
                        Geometry3D = new IMesh[] { GetOrCreateFromCache("m_10") };
                        break;
                    case 12000:
                        Geometry3D = new IMesh[] { GetOrCreateFromCache("m_12") };
                        break;
                    case 15000:
                        Geometry3D = new IMesh[] { GetOrCreateFromCache("m_15") };
                        break;
                }
            };

            m_len.Value = m_len.DictionaryValues.Values.FirstOrDefault();

            Properties.Add(m_len);
        }
    }
}
