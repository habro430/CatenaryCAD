using CatenaryCAD.Components;
using System.Collections.Concurrent;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция деталей <see cref="IComponent"/>
        /// </summary>
        protected ConcurrentDictionary<string, IComponent> ComponentsDictionary = new ConcurrentDictionary<string, IComponent>();

        public virtual IComponent[] ComponentParts { get => ComponentsDictionary.Values.ToArray(); }
    }
}