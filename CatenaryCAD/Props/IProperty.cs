using System;
using System.Collections;
using System.Linq;

namespace CatenaryCAD.Properties
{
    [Serializable, Flags]
    public enum PropertyAttributes
    {
        None = 0,
        ReadOnly = 1,
        RefreshAfterChange = 2,
        NotBrowsable = 4,
    }

    /// <summary>
    /// Интерфейс, описывающий контракты для классов представляющих объекты параметров модели.
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Имя параметра, отображаемое в свойствах объекта.
        /// </summary>
        /// <value>
        /// Наименование параметра.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Категория параметра, отображаемая в свойствах объекта.
        /// </summary>
        /// <value>
        /// Наименование группы параметра.
        /// </value>
        public string Category { get; }

        /// <value>
        /// Флаги конфигурации параметра.
        /// </value>
        public PropertyAttributes Attributes { get; }

        /// <value>
        /// Значение параметра.
        /// </value>
        public object Value { get; set; }

        /// <value>
        /// Коллекция стандартных значений для параметра.
        /// </value>
        public ICollection StandartValues { get; }
        public Type GetValueType();
    }

    internal static partial class Extensions
    {
        internal static AdapterProperty[] ToAdapterProperty(this IOrderedEnumerable<IProperty> props)
            => props.Select(prop => prop.ToAdapterProperty()).ToArray();

        internal static AdapterProperty ToAdapterProperty(this IProperty prop) => new AdapterProperty(prop);

    }
}
