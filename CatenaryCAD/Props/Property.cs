using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CatenaryCAD.Properties
{
    /// <summary>
    /// Представляет параметр модели
    /// </summary>
    /// <typeparam name="T">Тип значения параметра</typeparam>
    [Serializable, DebuggerDisplay("Name = {name}, Value = {value}")]
    public sealed class Property<T> : IProperty
    {
        public event Action<T> Updated;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T value = default(T);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string identifier, name, category;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConfigFlags properties;


        /// <summary>
        /// Коллеция стандартных значений для параметра
        /// </summary>
        public Dictionary<string, T> DictionaryValues { set; get; }

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
        /// Идентификатор параметра
        /// </summary>
        public string Identifier => identifier;

        /// <summary>
        /// Имя параметра, отображаемое в свойствах объекта
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Категория параметра, отображаемая в свойствах объекта
        /// </summary>
        public string Category => category;

        /// <summary>
        /// Флаги конфигурации параметра
        /// </summary>
        public ConfigFlags Properties => properties;

        public Property(string id, string name, string category, ConfigFlags props = ConfigFlags.None)
        {
            this.identifier = id;
            this.name = name;
            this.category = category;

            this.properties = props;
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

            if (Properties.HasFlag(ConfigFlags.ReadOnly))
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
