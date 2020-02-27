using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CatenaryCAD
{
    public static class ConverterterHelper
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        //public static object GetDefaultValueFromEnumValue(Enum value)
        //{
        //    DefaultValueAttribute attribute = value.GetType()
        //        .GetField(value.ToString())
        //        .GetCustomAttributes(typeof(DefaultValueAttribute), false)
        //        .SingleOrDefault() as DefaultValueAttribute;
        //    return attribute == null ? value.ToString() : attribute.Value;
        //}

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();

            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
        public class Converter<T> : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                var values = Enum.GetValues(typeof(T))
                    .Cast<Enum>()
                    .Select(f => GetDescriptionFromEnumValue(f))
                    .ToArray();

                return new StandardValuesCollection(values);
            }
        }
    }
}
