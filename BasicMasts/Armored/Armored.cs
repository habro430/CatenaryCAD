using BasicFoundations;

using CatenaryCAD.Components;
using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Attributes;
using CatenaryCAD.Models.Events;
using CatenaryCAD.Properties;

using System;
using System.Collections.Generic;

namespace BasicMasts
{
    [Serializable]
    [ModelName("Железобетонная")]
    [ModelDescription("Представляет базовую модель железобетонной стойки")]
    
    public class Armored : AbstractMast
    {
        //при первом вызове класса кэшируем в словарь производные от него опоры в статику
        [NonSerialized]
        private static readonly Dictionary<string, Type> InheritedMasts = AbstractMast.GetInheritedMastsFor(typeof(Armored));
        private static readonly Type[] allowablefoundations = new Type[] { typeof(FoundationSubmersible) };
        //генерируем геометрию для 2D режима
        private static readonly IShape[] geometry2d = new IShape[] { new Circle(new Point2D(), 300, 20) };
        //генерируем стандартные значения длинны стойки
        private static readonly Dictionary<string, int> defaultlenghts = new Dictionary<string, int> { ["10.0 м"] = 10000, ["12.0 м"] = 12000, ["15.0 м"] = 15000 };

        public override Type[] AllowableFoundations => allowablefoundations;
        public override IShape[] GetGeometry() => geometry2d;

        public Armored()
        {
            if (InheritedMasts.Count > 0)
                PropertiesDictionary.TryAdd("mast_type", new Property<Type>("Марка стойки", "Стойка", InheritedMasts, attr: Attributes.RefreshAfterChange));
            else
                PropertiesDictionary.TryAdd("mast_type", new Property<string>("Марка стойки", "Стойка", string.Empty));

            PropertiesDictionary.TryAdd("mast_lenght", new Property<int>("Длинна", "Стойка", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
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
