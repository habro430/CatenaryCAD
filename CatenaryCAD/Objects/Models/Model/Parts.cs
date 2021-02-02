using CatenaryCAD.ComponentParts;
using System.Collections.Concurrent;

using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция деталей <see cref="IComponentPart"/>
        /// </summary>
        protected ConcurrentDictionary<string, IComponentPart> ComponentPartsDictionary = new ConcurrentDictionary<string, IComponentPart>();

        public virtual IComponentPart[] ComponentParts { get => ComponentPartsDictionary.Values.ToArray(); }
    }
}