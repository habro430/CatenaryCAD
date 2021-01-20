using CatenaryCAD.Helpers;
using CatenaryCAD.ComponentParts;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция деталей <see cref="IComponentPart"/>
        /// </summary>
        protected ConcurrentHashSet<IComponentPart> PartsSet = new ConcurrentHashSet<IComponentPart>();

        public virtual IComponentPart[] Parts { get => PartsSet.ToArray(); }
    }
}