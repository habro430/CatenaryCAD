using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Properties
{
    /// <summary>
    /// Класс, реализующий типизируемый параметр.
    /// </summary>
    /// <typeparam name="T">Тип значения параметра.</typeparam>
    [Serializable, DebuggerDisplay("Name = {Name}, Value = {Value}")]
    public sealed class Property<T> : IProperty
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, T> dropdovnvalues;

        /// <inheritdoc cref="IProperty.DropDownValues"/>
        public Dictionary<string, T> DropDownValues
        {
            get => dropdovnvalues;
            set
            {
                dropdovnvalues = value;
                Value = dropdovnvalues.Values.FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ICollection IProperty.DropDownValues => DropDownValues != null ? DropDownValues.Keys : null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T value = default(T);

        /// <inheritdoc cref="IProperty.Value"/>
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Updated?.Invoke(value);
            }
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IProperty.Value
        {
            get
            {
                if (DropDownValues != null)
                {
                    foreach (var dict in DropDownValues)
                        if (dict.Value.Equals(Value))
                            return dict.Key;
                }
                else
                {
                    return Value;
                }

                return null;
            }
            set
            {
                if(Attributes.HasValue ? Attributes.Value.HasFlag(Properties.Attributes.ReadOnly) : false)
                {
                    if (value == null)
                    {

                        if (DropDownValues != null)
                            Value = DropDownValues[value.ToString()];
                        else
                            Value = (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                else
                {
                    if (DropDownValues != null)
                        Value = DropDownValues[value.ToString()];
                    else
                        Value = (T)Convert.ChangeType(value, typeof(T));
                }
            }
        }

        /// <inheritdoc/>
        public string Name { get; private set;  }

        /// <inheritdoc/>
        public string Category { get; private set; }

        /// <inheritdoc/>
        public Attributes? Attributes { get; private set; }

        /// <summary>
        /// Событие, вызываемое после изменения значения <see cref="Value"></see>.
        /// </summary>
        /// <value>
        /// Метод, который принимает один параметр <typeparamref name="T"/>.
        /// </value> 
        public event Action<T> Updated;

        /// <inheritdoc/>
        public Type GetValueType() => typeof(T);

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Property{T}"/>, имеющий указанные имена 
        /// параметра <paramref name="name"/> и категории <paramref name="category"/>, указанный 
        /// метод <paramref name="update"/> для вызова после обновления значения, а так же указанные 
        /// аттрибуты конфигурации <paramref name="attr"/>.
        /// </summary>
        /// <param name="name">Имя параметра, отображаемое в списке параметров.</param>
        /// <param name="category">Имя категории параметра, отображаемое в списке параметров.</param>
        /// <param name="update">Метод, который принимает один параметр <typeparamref name="T"/> и выполняемый 
        /// при изменении значения параметра.</param>
        /// <param name="attr">Атрибуты конфигурации параметра.</param>
        public Property(string name, string category, 
            Action<T> update = null, Attributes? attr = null)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            Value = default;
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Property{T}"/>, имеющий указанные имена
        /// параметра <paramref name="name"/> и категории <paramref name="category"/>, указанное 
        /// значение по умолчанию <paramref name="defaultvalue"/>, указанный метод <paramref name="update"/>
        /// для вызова после обновления значения, а так же указанные аттрибуты конфигурации <paramref name="attr"/>.
        /// </summary>
        /// <param name="name">Имя параметра, отображаемое в списке параметров.</param>
        /// <param name="category">Имя категории параметра, отображаемое в списке параметров.</param>
        /// <param name="update">Метод, который принимает один параметр <typeparamref name="T"/> и выполняемый 
        /// при изменении значения параметра.</param>
        /// <param name="defaultvalue">Значение параметра по умолчанию.</param>
        /// <param name="attr">Атрибуты конфигурации параметра.</param>
        public Property(string name, string category, T defaultvalue,
            Action<T> update = null, Attributes? attr = null)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            Value = defaultvalue;
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Property{T}"/>, имеющий указанные имена 
        /// параметра <paramref name="name"/> и категории <paramref name="category"/>, 
        /// указанные значения выбираемые из списка <paramref name="dropdownvalues"/>, 
        /// указанный метод <paramref name="update"/> для вызова после обновления значения, 
        /// а так же указанные аттрибуты конфигурации <paramref name="attr"/>.
        /// </summary>
        /// <remarks>При инициализации класса данным конструктором, значением по умолчанию устанавливаеться
        /// первое значение из списка <paramref name="dropdownvalues"/>.</remarks>
        /// <param name="name">Имя параметра, отображаемое в списке параметров.</param>
        /// <param name="category">Имя категории параметра, отображаемое в списке параметров.</param>
        /// <param name="dropdownvalues">Стандартные значения парметра, выбираемые из списка.</param>
        /// <param name="update">Метод, который принимает один параметр <typeparamref name="T"/> и выполняемый 
        /// при изменении значения параметра.</param>
        /// <param name="attr">Атрибуты конфигурации параметра.</param>
        public Property(string name, string category, Dictionary<string, T> dropdownvalues, 
            Action<T> update = null, Attributes? attr = null)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            DropDownValues = dropdownvalues;
            //Value = DropDownValues.Values.FirstOrDefault();
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Property{T}"/>, имеющий указанные имена 
        /// параметра <paramref name="name"/> и категории <paramref name="category"/>, 
        /// указанные значения выбираемые из списка <paramref name="dropdownvalues"/>, 
        /// указанное значение по умолчанию <paramref name="defaultvalue"/>, указанный 
        /// метод <paramref name="update"/> для вызова после обновления значения, 
        /// а так же указанные аттрибуты конфигурации <paramref name="attr"/>.       
        /// </summary>
        /// <param name="name">Имя параметра, отображаемое в списке параметров.</param>
        /// <param name="category">Имя категории параметра, отображаемое в списке параметров.</param>
        /// <param name="dropdownvalues">Стандартные значения парметра, выбираемые из списка.</param>
        /// <param name="defaultvalue">Значение параметра по умолчанию.</param>
        /// <param name="update">Метод, который принимает один параметр <typeparamref name="T"/> и выполняемый 
        /// при изменении значения параметра.</param>
        /// <param name="attr">Атрибуты конфигурации параметра.</param>
        public Property(string name, string category, Dictionary<string, T> dropdownvalues, T defaultvalue,
            Action<T> update = null, Attributes? attr = null)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            DropDownValues = dropdownvalues;
            //Value = defaultvalue;
        }

    }
}
