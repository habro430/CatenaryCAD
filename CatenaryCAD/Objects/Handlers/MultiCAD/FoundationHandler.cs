using CatenaryCAD.Geometry;
using CatenaryCAD.Attributes;
using CatenaryCAD.Properties;
using Multicad.Geometry;
using Multicad.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    [CustomEntity("{36E13AC1-DF87-4158-8C7E-221A17AEB6E7}", "BASEMENT", "Фундамент опоры контактной сети")]
    internal sealed class FoundationHandler : Handler
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> DefaultAvailableFoundations;
        private readonly Property<Type> FoudationProperty;

        static FoundationHandler()
        {
            DefaultAvailableFoundations = Main.GetCatenaryObjects(typeof(IFoundation))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null)
                .ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p);
        }

        public FoundationHandler() 
        {
            FoudationProperty = new Property<Type>("Тип фундамента", "CatenaryCAD", attr: CatenaryCAD.Properties.Attributes.RefreshAfterChange);
            FoudationProperty.Updated += (type) =>
            {
                var foundation = Activator.CreateInstance(type) as Foundation;
                foundation.AvailableFoundationsUpdated += (types) =>
                {
                    //модели имеющие атрибут ModelNonBrowsableAttribute уже отсеяны в AvailableFoundationsUpdated
                    FoudationProperty.DropDownValues = types
                        .ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p);
                };

                Model = foundation;
            };

            FoudationProperty.DropDownValues = DefaultAvailableFoundations;
            PropertiesDictionary.TryAdd("basement_type", FoudationProperty);
        }

        public override void OnTransform(Matrix3d tfm) { return; }

    }
}
