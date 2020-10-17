using CatenaryCAD.Parts;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция деталей <see cref="IPart"/>
        /// </summary>
        protected ConcurrentHashSet<IPart> PartsSet = new ConcurrentHashSet<IPart>();

        public virtual IPart[] Parts { get => PartsSet.ToArray(); }
    }
}