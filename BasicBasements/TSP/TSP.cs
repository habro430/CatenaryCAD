using Catenary.Attributes;
using Catenary.Components;
using Catenary.Geometry.Meshes;
using Catenary.Models.Events;
using Catenary.Properties;
using System;
using System.Collections.Generic;

namespace BasicFoundations
{
    [Serializable]
    [ModelName("ТСП")]
    [ModelDescription("Фундамент с анкерным креплением стоек жестких поперечин")]
    public class TSP : AbstractFoundation
    {
        private static readonly Dictionary<string, int> defaultlenghts = new Dictionary<string, int> { ["4.5 м"] = 4500, ["5.0 м"] = 5000 };

        public TSP()
        {
            PropertiesDictionary.TryAdd("foundation_lenght", new Property<int>("Длинна фундамента", "Фундамент", defaultlenghts, UpdateLenght, Attributes.RefreshAfterChange));
        }
        private void UpdateLenght(int lenght)
        {
            if (!(bool?)EventInvoke(this, new TryModify())?.Value ?? false) return;

            switch (lenght)
            {
                case 4500:
                    Component tsp45 = new Component(new IMesh[] { GetOrCreateFromCache("tsp-4.5") });
                    ComponentsDictionary.AddOrUpdate("anchor", tsp45, (name, component) => tsp45);
                    break;
                case 5000:
                    Component tsp50 = new Component(new IMesh[] { GetOrCreateFromCache("tsp-5.0") });
                    ComponentsDictionary.AddOrUpdate("anchor", tsp50, (name, component) => tsp50);
                    break;
            }

        }
    }
}
