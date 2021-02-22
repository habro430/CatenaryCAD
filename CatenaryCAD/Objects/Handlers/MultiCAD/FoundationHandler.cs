using CatenaryCAD.Geometry;
using CatenaryCAD.Models.Attributes;
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
        private static readonly Dictionary<string, Type> Foundations;

        static FoundationHandler()
        {
            Foundations = Main.GetCatenaryObjects(typeof(IFoundation))
                            .Where((t) => Attribute.GetCustomAttribute(t, typeof(ModelNonBrowsableAttribute), false) is null)
                            .ToDictionary(p => Attribute.GetCustomAttribute(p, typeof(ModelNameAttribute), false)?.ToString() ?? p.Name, p => p);
        }

        public FoundationHandler()
        {
            Property<Type> basement_type = new Property<Type>("Тип фундамента", "Фундамент", attr: CatenaryCAD.Properties.Attributes.RefreshAfterChange);

            basement_type.DropDownValues = Foundations;

            basement_type.Updated += (type) => Model = Activator.CreateInstance(type) as Foundation;
            basement_type.Value = Foundations.Values.FirstOrDefault();

            Properties.Add(basement_type);
        }

        public override void OnTransform(Matrix3d tfm)
        {
            if (Model.Parent == null)
            {
                if (ID.IsNull) 
                    return;

                Model.TransformBy(tfm.ToCatenaryCAD());
            }
        }

    }
}
