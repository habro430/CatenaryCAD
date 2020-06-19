using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CatenaryCAD.Properties
{
    [Serializable]
    public sealed class PropertyCollection: IEnumerable
    {
        private Dictionary<string, AbstractProperty> properties = new Dictionary<string, AbstractProperty>();

        public AbstractProperty this[string id]
        {
            get => properties[id];
            set => properties[id] = value;
        }
        public void Add(params AbstractProperty[] props)
        {
            foreach (AbstractProperty property in props)
                properties[property.ID] = property;
        }
        public void Remove(params string[] ids)
        {
            foreach (string id in ids)
                properties.Remove(id);
        }

        public bool Exist(string id) => properties.ContainsKey(id);

        public AbstractProperty[] ToArray() => properties.Values.ToArray();
        internal AdapterProperty[] ToAdapterPropertyArray() => properties.Values.Select(prop => prop.ToAdapterProperty()).ToArray();

        public IEnumerator GetEnumerator() => properties.GetEnumerator();
    }
}
