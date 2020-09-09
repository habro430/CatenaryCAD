using CatenaryCAD.Objects.Attributes;
using CatenaryCAD.Properties;
using Multicad.Geometry;
using Multicad.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Objects
{
    [Serializable]
    [CustomEntity("{36E13AC1-DF87-4158-8C7E-221A17AEB6E7}", "BASEMENT", "Фундамент опоры контактной сети")]
    internal sealed class BasementHandler : AbstractHandler
    {
        [NonSerialized]
        private static readonly Type[] Basements;
        static BasementHandler() => Basements = Main.GetCatenaryObjects(typeof(IBasement));

        public BasementHandler()
        {
            Property<Type> basement_type = new Property<Type>("01_basement_type", "Тип фундамента", "Фундамент", ConfigFlags.RefreshAfterChange);

            basement_type.DictionaryValues = Basements
                .Where((type) => type.GetCustomAttributes(typeof(NonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    name = (type.GetCustomAttributes(typeof(NameAttribute), false)
                               .FirstOrDefault() as NameAttribute ?? new NameAttribute(type.Name)).Name,
                }).ToDictionary(p => p.name, p => p.type);

            basement_type.Updated += (type) =>
            {
                if (!TryModify()) return;
                CatenaryObject = (IBasement)Activator.CreateInstance(type);
                CatenaryObject.Updated += () => { if (!TryModify()) return; };
            };

            basement_type.Value = basement_type.DictionaryValues.Values.FirstOrDefault();

            Properties.Add(basement_type);
        }

        public override void OnTransform(Matrix3d tfm)
        {
            if (Parent == null)
            {
                if (ID.IsNull) 
                    return;

                TransformBy(tfm);
            }
        }

    }
}
