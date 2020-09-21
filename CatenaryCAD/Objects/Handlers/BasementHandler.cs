using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;
using Multicad.Geometry;
using Multicad.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Models
{
    [Serializable]
    [CustomEntity("{36E13AC1-DF87-4158-8C7E-221A17AEB6E7}", "BASEMENT", "Фундамент опоры контактной сети")]
    internal sealed class BasementHandler : AbstractHandler
    {
        [NonSerialized]
        private static readonly Type[] Basements;
        static BasementHandler() => Basements = Main.GetCatenaryObjects(typeof(IFoundation));

        public BasementHandler()
        {
            Property<Type> basement_type = new Property<Type>("01_basement_type", "Тип фундамента", "Фундамент", ConfigFlags.RefreshAfterChange);

            basement_type.DictionaryValues = Basements
                .Where((type) => type.GetCustomAttributes(typeof(ModelNonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    name = (type.GetCustomAttributes(typeof(ModelNameAttribute), false)
                               .FirstOrDefault() as ModelNameAttribute ?? new ModelNameAttribute(type.Name)).Name,
                }).ToDictionary(p => p.name, p => p.type);

            basement_type.Updated += (type) =>
            {
                if (!TryModify()) return;
                Model = (IFoundation)Activator.CreateInstance(type);

                Model.Update += () => DbEntity.Update();
                Model.TryModify += () => TryModify();
            };

            basement_type.Value = basement_type.DictionaryValues.Values.FirstOrDefault();

            properties.Add(basement_type);
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
