using CatenaryCAD.Properties;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция параметров <see cref="IProperty"/>
        /// </summary>
        protected ConcurrentHashSet<IProperty> PropertiesSet = new ConcurrentHashSet<IProperty>();

        public virtual IProperty[] Properties { get => PropertiesSet.ToArray(); }
    }
}