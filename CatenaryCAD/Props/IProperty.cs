using System;
using System.Collections;
using System.Linq;

namespace CatenaryCAD.Properties
{
    [Serializable, Flags]
    public enum ConfigFlags
    {
        None = 0,
        ReadOnly = 1,
        RefreshAfterChange = 2,
        NotBrowsable = 4,
    }

    public interface IProperty
    {
        public string Identifier {  get; }
        public string Name { get; }
        public string Category { get; }
        public ConfigFlags Properties { get; }

        public object GetValue();
        public bool SetValue(object value);

        public Type GetValueType();
        public ICollection GetValuesCollection();
    }

    internal static partial class Extensions
    {
        internal static AdapterProperty[] ToAdapterProperty(this IOrderedEnumerable<IProperty> props)
            => props.Select(prop => prop.ToAdapterProperty()).ToArray();

        internal static AdapterProperty ToAdapterProperty(this IProperty prop) => new AdapterProperty(prop);

    }
}
