using CatenaryCAD.Properties;
using System.Collections.Concurrent;
using System.Linq;

namespace CatenaryCAD.Models
{
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// Коллекция параметров <see cref="IProperty"/>
        /// </summary>
        protected ConcurrentDictionary<string, IProperty> PropertiesDictionary = new ConcurrentDictionary<string, IProperty>();

        //protected ConcurrentHashSet<IProperty> PropertiesSet = new ConcurrentHashSet<IProperty>();

        public virtual IProperty[] Properties { get => PropertiesDictionary.Values.ToArray(); }
    }
}