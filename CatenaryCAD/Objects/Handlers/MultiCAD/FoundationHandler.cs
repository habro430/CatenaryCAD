using CatenaryCAD.Geometry;
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
        public FoundationHandler() 
        {
            Property<Type> foudation_prop = new Property<Type>("Тип фундамента", "Фундамент", attr: CatenaryCAD.Properties.Attributes.RefreshAfterChange);

            foudation_prop.Updated += (type) =>
            {
                var foundation = Activator.CreateInstance(type) as Foundation;

                foundation.AllowableModelsUpdated += () => 
                {
                    foudation_prop.DropDownValues = foundation.AllowableModels;
                    foudation_prop.Value = foudation_prop.DropDownValues.Values.FirstOrDefault(); 
                };

                Model = foundation;
            };

            foudation_prop.DropDownValues = Foundation.DefaultAllowableFoundations;
            foudation_prop.Value = foudation_prop.DropDownValues.Values.FirstOrDefault();

            PropertiesDictionary.AddOrUpdate("basement_type", foudation_prop, (name, property) => foudation_prop);
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
