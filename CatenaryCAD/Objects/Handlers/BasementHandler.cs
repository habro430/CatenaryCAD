﻿using CatenaryCAD.Properties;
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
        private static readonly Dictionary<string, Type> Basements;
        static BasementHandler()
        {
            //получем словарь из CatenaryObjectAttribute и типа производного от IBasement класса
            Basements = Main.GetCatenaryObjectTypesFor(typeof(IBasement)).Select((type) => new
            {
                type,
                atrr = type.GetCustomAttributes(typeof(CatenaryObjectAttribute), false)
                            .FirstOrDefault() as CatenaryObjectAttribute
            }).ToDictionary(p => p.atrr.Name, p => p.type);
        }
        public BasementHandler()
        {
            Property<Type> basement_type = new Property<Type>("01_basement_type", "Тип фундамента", "Фундамент", ConfigFlags.RefreshAfterChange);

            basement_type.DictionaryValues = Basements;

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
