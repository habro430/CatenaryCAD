using CatenaryCAD.Parts;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected ConcurrentHashSet<IPart> PartsSet = new ConcurrentHashSet<IPart>();

        public virtual IPart[] Parts { get => PartsSet.ToArray(); }
    }
}