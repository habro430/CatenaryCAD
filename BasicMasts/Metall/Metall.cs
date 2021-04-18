using BasicFoundations;

using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Attributes;
using CatenaryCAD.Models.Events;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;

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

        public override Type[] AllowableFoundations => new Type[] { typeof(FoundationDirect) };

        public Metall()
        {
            //генериуем геометрию для 2D режима
            Geometry2D = new IShape[] { new Rectangle(new Point2D(), 600, 600) };

            if (InheritedMasts.Count > 0)
                PropertiesDictionary.TryAdd("mast_type", new Property<Type>("Марка стойки", "Стойка", InheritedMasts, attr: Attributes.RefreshAfterChange));
            else
                PropertiesDictionary.TryAdd("mast_type", new Property<string>("Марка стойки", "Стойка", string.Empty));


            Dictionary<string, int> defaultlenghts = new Dictionary<string, int>
            {
                ["10.0 м"] = 10000,
                ["12.0 м"] = 12000,
                ["15.0 м"] = 15000
            };

            PropertiesDictionary.TryAdd("mast_lenght", new Property<int>("Длинна", "Стойка", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
        }

        private void UpdateLenght(int lenght)
        {
            if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;

            switch (lenght)
            {
                case 10000:
                    Component m10 = new Component(new IMesh[] { GetOrCreateFromCache("m_10") });
                    ComponentsDictionary.AddOrUpdate("mast", m10, (name, component) => m10);
                    break;
                case 12000:
                    Component m12 = new Component(new IMesh[] { GetOrCreateFromCache("m_12") });
                    ComponentsDictionary.AddOrUpdate("mast", m12, (name, component) => m12);
                    break;
                case 15000:
                    Component m15 = new Component(new IMesh[] { GetOrCreateFromCache("m_15") });
                    ComponentsDictionary.AddOrUpdate("mast", m15, (name, component) => m15);
                    break;
            }

        }
    }
}
