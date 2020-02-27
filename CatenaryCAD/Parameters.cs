using Multicad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD
{
    [Serializable]
    public class Parameters : IEnumerable
    {
        private Dictionary<string, McDynamicProperty> parameters = new Dictionary<string, McDynamicProperty>();

        public McDynamicProperty this[string name]
        {
            get => parameters[name];
            set => parameters[name] = value;
        }

        public void Add(string name, McDynamicProperty geometry) => parameters.Add(name, geometry);
        public void Remove(string name) => parameters.Remove(name);

        public bool Exist(string name) => parameters.ContainsKey(name);

        public McDynamicProperty[] ToArray() => parameters.Values.ToArray();

        public IEnumerator GetEnumerator() => parameters.GetEnumerator();

    }
}
