﻿
using BasicFoundations;
using Catenary.Attributes;
using Catenary.Components;
using Catenary.Geometry;
using Catenary.Geometry.Meshes;
using Catenary.Geometry.Shapes;
using Catenary.Models.Events;
using Catenary.Properties;
using System;
using System.Collections.Generic;

namespace BasicMasts
{
    [Serializable]
    [ModelName("СС")]
    [ModelDescription("Cтойка железобетонная с напрягаемой проволочной арматурой со смешанным армированием")]
    
    public class SS : AbstractMast
    {
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(SS));

        private static readonly Type[] allowablefoundations = new Type[] { typeof(TSS) };

        //генерируем геометрию для 2D режима
        private static readonly IShape[] geometry2d = new IShape[] { new Circle(new Point2D(), 1.5d, 20) };
        //генерируем стандартные значения длинны стойки
        private static readonly Dictionary<string, int> defaultlenghts = new Dictionary<string, int> { ["10.0 м"] = 10000, ["12.0 м"] = 12000, ["15.0 м"] = 15000 };

        public override Type[] AllowableFoundations => allowablefoundations;
        public override IShape[] GetSchemeGeometry() => geometry2d;
        public SS()
        {
            PropertiesDictionary.TryAdd("mast_type", InheritedMasts.Count > 0 ? new Property<Type>("Марка стойки", "Стойка", InheritedMasts, attr: Attributes.RefreshAfterChange) as IProperty :
                                                                                new Property<string>("Марка стойки", "Стойка", string.Empty) as IProperty);

            PropertiesDictionary.TryAdd("mast_lenght", new Property<int>("Длинна стойки", "Стойка", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
        }

        private void UpdateLenght(int lenght)
        {
            if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false)
                return;

            switch (lenght)
            {
                case 10000:
                    Component a10 = new Component(new IMesh[] { GetOrCreateFromCache("a_10") });
                    ComponentsDictionary.AddOrUpdate("mast", a10, (name, component) => a10); 
                    break;
                case 12000:
                    Component a12 = new Component(new IMesh[] { GetOrCreateFromCache("a_12") });
                    ComponentsDictionary.AddOrUpdate("mast", a12, (name, component) => a12); 
                    break;
                case 15000:
                    Component a15 = new Component(new IMesh[] { GetOrCreateFromCache("a_15") });
                    ComponentsDictionary.AddOrUpdate("mast", a15, (name, component) => a15);
                    break;
            }

        }
    }
}
