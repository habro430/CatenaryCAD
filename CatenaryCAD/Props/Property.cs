using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CatenaryCAD.Properties
{
    /// <summary>
    /// Представляет параметр модели
    /// </summary>
    /// <typeparam name="T">Тип значения параметра</typeparam>
    [Serializable, DebuggerDisplay("Name = {Name}, Value = {Value}")]
    public sealed class Property<T> : IProperty
    {
        public event Action<T> Updated;

        /// <value>
        /// Коллекция стандартных значений для параметра.
        /// </value>
        public Dictionary<string, T> StandartValues { set; get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ICollection IProperty.StandartValues => StandartValues != null ? StandartValues.Keys : null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T value = default(T);

        /// <value>
        /// Значение параметра.
        /// </value>
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Updated?.Invoke(value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IProperty.Value
        {
            get
            {
                if (StandartValues != null)
                {
                    foreach (var dict in StandartValues)
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
                if (Attributes.HasFlag(PropertyAttributes.ReadOnly))
                {
                    if (value == null)
                    {

                        if (StandartValues != null)
                            Value = StandartValues[value.ToString()];
                        else
                            Value = (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                else
                {
                    if (StandartValues != null)
                        Value = StandartValues[value.ToString()];
                    else
                        Value = (T)Convert.ChangeType(value, typeof(T));
                }
            }
        }

        /// <summary>
        /// Имя параметра, отображаемое в свойствах объекта.
        /// </summary>
        /// <value>
        /// Наименование параметра.
        /// </value>
        public string Name { get; private set;  }

        /// <summary>
        /// Категория параметра, отображаемая в свойствах объекта.
        /// </summary>
        /// <value>
        /// Наименование группы параметра.
        /// </value>
        public string Category { get; private set; }

        /// <value>
        /// Флаги конфигурации параметра.
        /// </value>
        public PropertyAttributes Attributes { get; private set; }


        public Property(string name, string category, 
            Action<T> update = null, PropertyAttributes attr = PropertyAttributes.None)
        {
            Name = name;
            Category = category;

            Attributes = attr;

            Value = default;
        }

        public Property(string name, string category, T value,
            Action<T> update = null, PropertyAttributes attr = PropertyAttributes.None)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            Value = value;
        }

        public Property(string name, string category, Dictionary<string, T> dictionaryvalues, 
            Action<T> update = null, PropertyAttributes attr = PropertyAttributes.None)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            StandartValues = dictionaryvalues;
            Value = StandartValues.Values.FirstOrDefault();
        }

        public Property(string name, string category, Dictionary<string, T> dictionaryvalues, T value,
            Action<T> update = null, PropertyAttributes attr = PropertyAttributes.None)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            StandartValues = dictionaryvalues;
            Value = value;
        }

        public Type GetValueType() => typeof(T);
    }
}
