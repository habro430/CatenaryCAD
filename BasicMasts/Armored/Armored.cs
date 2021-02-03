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
                PropertiesDictionary.TryAdd("mast_type", new Property<Type>("Марка стойки", "Стойка", InheritedMasts, attr: PropertyAttributes.RefreshAfterChange));
            else
                PropertiesDictionary.TryAdd("mast_type", new Property<string>("Марка стойки", "Стойка", string.Empty));

            Dictionary<string, int> defaultlenghts = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000
            };

            PropertiesDictionary.TryAdd("mast_lenght", new Property<int>("Длинна", "Стойка", defaultlenghts, UpdateLenght, PropertyAttributes.RefreshAfterChange));
        }

        private void UpdateLenght(int lenght)
        {
            if (!SendMessageToHandler(HandlerMessages.TryModify) ?? false)
                return;

            switch (lenght)
            {
                case 10000:
                    ComponentPartsDictionary.AddOrUpdate("mast", new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_10") }),
                        (name, component) => new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_10") }));
                    break;
                case 12000:
                    ComponentPartsDictionary.AddOrUpdate("mast", new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_12") }),
                        (name, component) => new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_12") }));
                    break;
                case 15000:
                    ComponentPartsDictionary.AddOrUpdate("mast", new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_15") }),
                        (name, component) => new ComponentPart(new IMesh[] { GetOrCreateFromCache("a_15") }));
                    break;
            }

        }
    }
}
