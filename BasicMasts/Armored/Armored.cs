using CatenaryCAD.ComponentParts;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BasicMasts
{
    [Serializable]
    [ModelName("Железобетонная")]
    [ModelDescription("Представляет базовую модель железобетонной стойки")]
    
    public class Armored : AbstractMast
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts;

        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        static Armored() => InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Armored));

        public Armored()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new IShape[] { new Circle(new Point2D(), 300, 20) };

            if (InheritedMasts.Count > 0)
            {
                Property<Type> mast_type = new Property<Type>("02_mast_armored_type", "Марка стойки", "Стойка",  ConfigFlags.RefreshAfterChange);
                
                mast_type.DictionaryValues = InheritedMasts;
                mast_type.Value = mast_type.DictionaryValues.Values.FirstOrDefault();

                PropertiesDictionary.TryAdd("mast_type", mast_type);
            }
            else
            {
                Property<string> mast_type = new Property<string>("02_mast_armored_type", "Марка стойки", "Стойка");
                mast_type.Value = string.Empty;

                PropertiesDictionary.TryAdd("mast_type", mast_type);
            }

            Property<int> mast_lenght = new Property<int>("03_mast_len", "Длинна", "Стойка", ConfigFlags.RefreshAfterChange);

            mast_lenght.DictionaryValues = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000,

            };

            mast_lenght.Updated += (val) =>
            {
                if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false) return;

                //читаем и генериуем геометрию для 3D режима в зависимости от длинны
                switch (val)
                {
                    case 10000:
                        ComponentPartsDictionary.AddOrUpdate("mast", new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_10") }),
                            (name, value) => new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_10") }));
                        break;
                    case 12000:
                        ComponentPartsDictionary.AddOrUpdate("mast", new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_12") }),
                            (name, value) => new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_12") }));
                        break;
                    case 15000:
                        ComponentPartsDictionary.AddOrUpdate("mast", new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_15") }),
                            (name, value) => new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_15") }));
                        break;
                }
            };
            mast_lenght.Value = mast_lenght.DictionaryValues.Values.FirstOrDefault();

            PropertiesDictionary.TryAdd("mast_lenght", mast_lenght);
        }
    }
}
