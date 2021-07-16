using System;
using System.Collections;
using System.Linq;

namespace Catenary.Properties
{
    [Serializable, Flags]
    public enum Attributes
    {
        /// <summary>
        /// Атриубут, указывающий на то, что параметр доступент только для чтения.
        /// </summary>
        ReadOnly = 1,
        /// <summary>
        /// Атрибут, указаывающий на то, что после изменения текущего параметра необходимо 
        /// обновить список параметров.
        /// </summary>
        RefreshAfterChange = 2,
        /// <summary>
        /// Атрибут, указаывающий на то, что параметр не видим в списке параметров.
        /// </summary>
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
        public Attributes? Attributes { get; }

        /// <value>
        /// Значение параметра.
        /// </value>
        public object Value { get; set; }

        /// <value>
        /// Коллекция значений параметра, выбираемых из списка.
        /// </value>
        public ICollection DropDownValues { get; }

        /// <summary>
        /// Возвращает тип значения <see cref="Value"></see> .
        /// </summary>
        /// <returns>Тип значения параметра.</returns>
        public Type GetValueType();
    }

    internal static partial class Extensions
    {
        internal static McProperty[] ToAdapterProperty(this IOrderedEnumerable<IProperty> props)
            => props.Select(prop => prop.ToAdapterProperty()).ToArray();

        internal static McProperty ToAdapterProperty(this IProperty prop) => new McProperty(prop);

    }
}
