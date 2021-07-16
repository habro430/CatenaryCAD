using Catenary.Components;
using Catenary.Geometry.Meshes;
using Catenary.Attributes;
using System;
using Catenary.Properties;
using Catenary.Models.Events;
using System.Collections.Generic;

namespace BasicAnchors
{
    [Serializable]
    [ModelName("ТАС")]
    [ModelDescription("Трехлучевой анкер")]
    public class TAS : AbstractAnchor
    {
        private static readonly Dictionary<string, int> defaultlenghts = new Dictionary<string, int> { ["4.0 м"] = 4000, ["4.5 м"] = 4500, ["5.0 м"] = 5000 };

        public TAS()
        {
            PropertiesDictionary.TryAdd("anchor_lenght", new Property<int>("Длинна анкера", "Анкер", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
        }
        private void UpdateLenght(int lenght)
        {
            if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;

            switch (lenght)
            {
                case 4000:
                    Component tas40 = new Component(new IMesh[] { GetOrCreateFromCache("tas-4.0") });
                    ComponentsDictionary.AddOrUpdate("anchor", tas40, (name, component) => tas40);
                    break;
                case 4500:
                    Component tas45 = new Component(new IMesh[] { GetOrCreateFromCache("tas-4.5") });
                    ComponentsDictionary.AddOrUpdate("anchor", tas45, (name, component) => tas45);
                    break;
                case 5000:
                    Component tas50 = new Component(new IMesh[] { GetOrCreateFromCache("tas-5.0") });
                    ComponentsDictionary.AddOrUpdate("anchor", tas50, (name, component) => tas50);
                    break;
            }

        }
    }
}
