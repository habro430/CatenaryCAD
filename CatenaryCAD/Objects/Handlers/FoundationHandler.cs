﻿using CatenaryCAD.Geometry;
using CatenaryCAD.Models.Attributes;
using CatenaryCAD.Properties;

using Multicad.Geometry;
using Multicad.Runtime;

using System;
using System.Linq;

namespace CatenaryCAD.Models.Handlers
{
    [Serializable]
    [CustomEntity("{36E13AC1-DF87-4158-8C7E-221A17AEB6E7}", "BASEMENT", "Фундамент опоры контактной сети")]
    internal sealed class FoundationHandler : Handler
    {
        [NonSerialized]
        private static readonly Type[] Foundations;
        static FoundationHandler() => Foundations = Main.GetCatenaryObjects(typeof(IFoundation));

        public FoundationHandler()
        {
            Property<Type> basement_type = new Property<Type>("01_foundation_type", "Тип фундамента", "Фундамент", ConfigFlags.RefreshAfterChange);

            basement_type.DictionaryValues = Foundations
                .Where((type) => type.GetCustomAttributes(typeof(ModelNonBrowsableAttribute), false).FirstOrDefault() == null)
                .Select((type) => new
                {
                    type,
                    name = (type.GetCustomAttributes(typeof(ModelNameAttribute), false)
                               .FirstOrDefault() as ModelNameAttribute ?? new ModelNameAttribute(type.Name)).Name,
                }).ToDictionary(p => p.name, p => p.type);

            basement_type.Updated += (type) =>
            {
                Point3D position = Model?.Position ?? new Point3D(0, 0, 0);
                Vector3D direction = Model?.Direction ?? new Vector3D(1, 0, 0);

                if (!TryModify()) return;
                Model = (IFoundation)Activator.CreateInstance(type);

                Model.Position = position;
                Model.Direction = direction;

                Model.TryModify += () => TryModify();
                Model.Update += () => DbEntity.Update();
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
