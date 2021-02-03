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

        /// <summary>
        /// Коллеция стандартных значений для параметра
        /// </summary>
        public Dictionary<string, T> DictionaryValues { set; get; }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T value = default(T);
        /// <summary>
        /// Значение параметра
        /// </summary>
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Updated?.Invoke(value);
            }
        }

        /// <summary>
        /// Имя параметра, отображаемое в свойствах объекта
        /// </summary>
        public string Name { get; private set;  }

        /// <summary>
        /// Категория параметра, отображаемая в свойствах объекта
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Флаги конфигурации параметра
        /// </summary>
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

            DictionaryValues = dictionaryvalues;
            Value = DictionaryValues.Values.FirstOrDefault();
        }

        public Property(string name, string category, Dictionary<string, T> dictionaryvalues, T value,
            Action<T> update = null, PropertyAttributes attr = PropertyAttributes.None)
        {
            Name = name;
            Category = category;

            Attributes = attr;
            Updated = update;

            DictionaryValues = dictionaryvalues;
            Value = value;
        }

        public Type GetValueType() => typeof(T);
        public ICollection GetValuesCollection()
        {
            if (DictionaryValues != null)
                return DictionaryValues.Keys;
            else
                return null;
        }
        public object GetValue()
        {
            if (DictionaryValues != null)
            {
                foreach (var dict in DictionaryValues)
                {
                    if (dict.Value.Equals(Value))
                        return dict.Key;
                }
            }
            else
            {
                return Value;
            }

            return null;
        }

        public bool SetValue(object val)
        {

            if (Attributes.HasFlag(PropertyAttributes.ReadOnly))
            {
                if (value == null)
                {

                    if (DictionaryValues != null)
                        Value = DictionaryValues[val.ToString()];
                    else
                    {
                        try
                        {
                            Value = (T)Convert.ChangeType(val, typeof(T));
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (DictionaryValues != null)
                    Value = DictionaryValues[val.ToString()];
                else
                {
                    try
                    {
                        Value = (T)Convert.ChangeType(val, typeof(T));
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
