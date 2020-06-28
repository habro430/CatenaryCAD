using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq;

namespace CatenaryCAD.Properties
{
    [Serializable, Flags]
    public enum PropertyFlags
    {
        None = 0,
        ReadOnly = 1,
        RefreshAfterChange = 2,
        NotBrowsable = 4,
    }

    public interface IProperty
    {
        public abstract string ID {  get; }
        public abstract string Name { get; }
        public abstract string Category { get; }
        public abstract PropertyFlags Properties { get; }

        public abstract object GetValue();
        public abstract bool SetValue(object value);

        public abstract Type GetValueType();
        public abstract ICollection GetValuesCollection();
    }
    internal static partial class Extensions
    {
        internal static AdapterProperty[] ToAdapterProperty(this IProperty[] props) 
            => props.Select(prop => prop.ToAdapterProperty()).ToArray();

        internal static AdapterProperty ToAdapterProperty(this IProperty prop) => new AdapterProperty(prop);

    }
}
