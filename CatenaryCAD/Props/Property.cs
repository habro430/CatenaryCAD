using System;
using System.Collections;
using System.Collections.Generic;

namespace CatenaryCAD.Properties
{
    [Serializable]
    public sealed class Property<T> : AbstractProperty
    {
        public event Action<T> Updated;

        public override string ID { get; protected set; }
        public override string Name { get; protected set; }
        public override string Category { get; protected set; }
        public override PropertyFlags Properties { get; protected set; }

        public Dictionary<string, T> DictionaryValues { set; get; }

        private T _value = default;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Updated?.Invoke(value);
            }
        }

        public Property(string id, string name, string category, PropertyFlags props = PropertyFlags.None)
        {
            ID = id;
            Name = name;
            Category = category;

            Properties = props;
        }

        public override Type GetValueType() => typeof(T);
        public override ICollection GetValuesCollection()
        {
            if (DictionaryValues != null)
                return DictionaryValues.Keys;
            else
                return null;
        }
        public override object GetValue()
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

        public override bool SetValue(object val)
        {

            if (Properties.HasFlag(PropertyFlags.ReadOnly))
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
