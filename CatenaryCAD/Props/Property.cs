using System;
using System.Collections;
using System.Collections.Generic;

namespace CatenaryCAD.Properties
{
    [Serializable]
    public sealed class Property<T> : IProperty
    {
        public event Action<T> Updated;

        private string id, name, category;
        private ConfigFlags properties;

        public Dictionary<string, T> DictionaryValues { set; get; }

        private T _value = default(T);
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Updated?.Invoke(value);
            }
        }

        public string ID => id;
        public string Name => name;
        public string Category => category;
        public ConfigFlags Properties => properties;

        public Property(string id, string name, string category, ConfigFlags props = ConfigFlags.None)
        {
            this.id = id;
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
                if (_value == null)
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
