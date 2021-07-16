using Catenary.Geometry;
using Catenary.Attributes;
using Catenary.Properties;
using Multicad.Geometry;
using Multicad.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Catenary.Models.Handlers
{
    [Serializable]
    [CustomEntity("{87D4547D-0941-4CE5-812F-7B300CDEAC6F}", "ANCHOR TIE", "Анкерная оттяжка")]
    internal sealed class AnchorTieHandler : Handler
    {
        [NonSerialized]
        private static readonly Dictionary<string, Type> DefaultAvailableAnchorTies;
        private readonly Property<Type> AnchorTieProperty;

        static AnchorTieHandler()
        {
            DefaultAvailableAnchorTies = Main.GetCatenaryObjects(typeof(IAnchorTie))
                .Where((t) => !t.IsAbstract)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null)
                .ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p);
        }

        public AnchorTieHandler()
        {
            AnchorTieProperty = new Property<Type>("Тип анкерной оттяжки", "CatenaryCAD", attr: Catenary.Properties.Attributes.RefreshAfterChange);
            AnchorTieProperty.Updated += (type) =>
            {
                var anchortie = Activator.CreateInstance(type) as AnchorTie;
                anchortie.AvailableAnchorTiesUpdated += (types) =>
                {
                    //модели имеющие атрибут ModelNonBrowsableAttribute уже отсеяны в AvailableFoundationsUpdated
                    AnchorTieProperty.DropDownValues = types
                        .ToDictionary(dict => Attribute.GetCustomAttribute(dict, typeof(ModelNameAttribute), false)?.ToString() ?? dict.Name, p => p);
                };

                Model = anchortie;
            };

            AnchorTieProperty.DropDownValues = DefaultAvailableAnchorTies;
            PropertiesDictionary.TryAdd("anchortie_type", AnchorTieProperty);
        }

        public override void OnTransform(Matrix3d tfm) { return; }

    }
}
