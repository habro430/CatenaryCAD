using Catenary.Attributes;
using Catenary.Components;
using Catenary.Geometry;
using Catenary.Geometry.Meshes;
using Catenary.Models;
using Catenary.Models.Events;
using Catenary.Properties;
using System;
using System.Collections.Generic;


namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСС")]
    [ModelDescription("Стаканные фундаменты консольных опор контактной сети")]
    public class TSS : AbstractFoundation
    {
        private static readonly Dictionary<string, int> defaultlenghts = new Dictionary<string, int> { ["4.0 м"] = 4000, ["4.5 м"] = 4500, ["5.0 м"] = 5000 };

        public TSS()
        {
            PropertiesDictionary.TryAdd("foundation_lenght", new Property<int>("Длинна фундамента", "Фундамент", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
        }
        private void UpdateLenght(int lenght)
        {
            if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;

            switch (lenght)
            {
                case 4000:
                    Component tss40 = new Component(new IMesh[] { GetOrCreateFromCache("tss-4.0") });
                    ComponentsDictionary.AddOrUpdate("anchor", tss40, (name, component) => tss40);
                    break;
                case 4500:
                    Component tss45 = new Component(new IMesh[] { GetOrCreateFromCache("tss-4.5") });
                    ComponentsDictionary.AddOrUpdate("anchor", tss45, (name, component) => tss45);
                    break;
                case 5000:
                    Component tss50 = new Component(new IMesh[] { GetOrCreateFromCache("tss-5.0") });
                    ComponentsDictionary.AddOrUpdate("anchor", tss50, (name, component) => tss50);
                    break;
            }

        }
        public override Point3D? GetDockingPoint(IModel from, Ray3D ray) => new Point3D(0, 0, -800);
    }
}
