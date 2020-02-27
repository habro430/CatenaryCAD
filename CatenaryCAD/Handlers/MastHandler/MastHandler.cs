using Multicad.Runtime;
using Multicad.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Multicad.Geometry;
using Multicad;
using Multicad.CustomObjectBase;
using System.ComponentModel;
using System.Collections;
using static CatenaryCAD.Handlers.MastHandler;

namespace CatenaryCAD.Handlers
{
    [CustomEntity("{742ECCF0-0CEC-4791-B4BE-4E3568E2C43E}", "ОПОРА", "Несущая конструкция, на которой \nзакрепляют устройства контактной \nсети, состоящая из стойки и \nфундамента или только из стойки.")]
    [Serializable]

    class MastHandler : AbstractHandler, IMcDynamicProperties
    {
        public MastHandler()
        {
            Parameters["material"] = new Material();
            (Parameters["material"] as Material).ValueUpdated += Material_ValueUpdated;

            Parameters["type"] = new ArmoredTypes();
            Parameters["lenght"] = new ArmoredLenght();

            Parameters["loadbearing"] = new LoadBearing();
        }

        private void Material_ValueUpdated()
        {
            if (!TryModify()) return;

            if ((Parameters["material"] as Material).Value == Material.TypeEnumeration.Armored)
            {
                Parameters["type"] = new ArmoredTypes();
                Parameters["lenght"] = new ArmoredLenght();
            }
            else
            {
                Parameters["type"] = new MetallTypes();
                Parameters["lenght"] = new MetallLenght();
            }

        }

        public override void OnDraw(GeometryBuilder dc)
        {
            dc.Clear();
            dc.Color = Multicad.Constants.Colors.ByObject;
            dc.LineType = Multicad.Constants.LineTypes.ByObject;

            dc.DrawCircle(Point3d.Origin, 100 * this.DbEntity.DScale);
        }

        [CommandMethod("insert_mast", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void insert_mast()
        {
            using (InputJig input = new InputJig())
            {
                HashSet<McObjectId> excluded_objs = new HashSet<McObjectId>();
                while (true)
                {
                    InputResult result = input.GetPoint("Выберите точку для размещения обьекта:");
                    if (result.Result != InputResult.ResultCode.Normal) return;

                    MastHandler mast = new MastHandler();
                    mast.PlaceObject(result.Point, Vector3d.XAxis);
                    mast.DbEntity.Update();

                    if (excluded_objs.Add(mast.ID))
                        input.ExcludeObjects(excluded_objs);

                }
            }

        }

        public ICollection<McDynamicProperty> GetProperties(out bool exclusive)
        {
            exclusive = true;
            return Parameters.ToArray();
        }

        public McDynamicProperty GetProperty(string name)
        {
            switch (name)
            {
                case "Материал":
                    return Parameters["material"];
                case "Тип":
                    return Parameters["type"];
                case "Длинна":
                    return Parameters["lenght"];
                case "Несущая способность":
                    return Parameters["loadbearing"];

            }
            return null;
        }
    }

    [Serializable]
    class Material : McDynamicProperty
    {

        public enum TypeEnumeration : byte
        {
            [Description("Железобетон")]
            Armored,
            [Description("Металл")]
            Metall
        }

        public override ExAttributesProperties ExtendedAtributes => ExAttributesProperties.RefreshListAfterChange;
        public override Type PropertyType => typeof(string);

        public override string Name => "Материал";
        public override string Category => "Стойка";

        private TypeEnumeration _value;
        public TypeEnumeration Value
        {
            get => _value;
            set
            {
                _value = value;

                ValueUpdated?.Invoke();
            }
        }
        public event Action ValueUpdated;

        public override ICollection GetStandardValues()
        {

            var values = Enum.GetValues(typeof(TypeEnumeration))
              .Cast<Enum>()
              .Select(f => ConverterterHelper.GetDescriptionFromEnumValue(f))
              .ToArray();

            return values;
        }

        public override StandardValueTypeEnum GetStandardValueType() => StandardValueTypeEnum.Exclusive;
        public override object GetValue() => ConverterterHelper.GetDescriptionFromEnumValue(Value);

        public override bool SetValue(object val)
        {
            Value = ConverterterHelper.GetEnumValueFromDescription<TypeEnumeration>(val.ToString());
            return true;
        }
    }

    [Serializable]
    class ArmoredTypes : McDynamicProperty
    {
        public enum TypeEnumeration : byte
        {
            [Description("СП")]
            SP,
            [Description("СТ")]
            ST,
            [Description("ССА")]
            SSA,
            [Description("СПА")]
            SPA,
            [Description("СТА")]
            STA,
            [Description("Другое")]
            OtherM
        }

        public override ExAttributesProperties ExtendedAtributes => ExAttributesProperties.RefreshListAfterChange;
        public override Type PropertyType => typeof(string);

        public override string Name => "Тип";
        public override string Category => "Стойка";

        private TypeEnumeration Value;

        public override ICollection GetStandardValues()
        {

            var values = Enum.GetValues(typeof(TypeEnumeration))
              .Cast<Enum>()
              .Select(f => ConverterterHelper.GetDescriptionFromEnumValue(f))
              .ToArray();

            return values;
        }

        public override StandardValueTypeEnum GetStandardValueType() => StandardValueTypeEnum.Exclusive;
        public override object GetValue() => ConverterterHelper.GetDescriptionFromEnumValue(Value);

        public override bool SetValue(object val)
        {
            Value = ConverterterHelper.GetEnumValueFromDescription<TypeEnumeration>(val.ToString());
            return true;
        }
    }
    [Serializable]
    class MetallTypes : McDynamicProperty
    {
        public enum TypeEnumeration : byte
        {
            [Description("МГК")]
            MGK,
            [Description("МШК")]
            MSHK,
            [Description("МГП")]
            MGP,
            [Description("МШП")]
            MSHP,
            [Description("Другое")]
            Other
        }

        public override ExAttributesProperties ExtendedAtributes => ExAttributesProperties.RefreshListAfterChange;
        public override Type PropertyType => typeof(string);

        public override string Name => "Тип";
        public override string Category => "Стойка";

        private TypeEnumeration Value;

        public override ICollection GetStandardValues()
        {

            var values = Enum.GetValues(typeof(TypeEnumeration))
              .Cast<Enum>()
              .Select(f => ConverterterHelper.GetDescriptionFromEnumValue(f))
              .ToArray();

            return values;
        }

        public override StandardValueTypeEnum GetStandardValueType() => StandardValueTypeEnum.Exclusive;
        public override object GetValue() => ConverterterHelper.GetDescriptionFromEnumValue(Value);

        public override bool SetValue(object val)
        {
            Value = ConverterterHelper.GetEnumValueFromDescription<TypeEnumeration>(val.ToString());
            return true;
        }
    }

    [Serializable]
    class LoadBearing : McDynamicProperty
    {
        public enum LoadBearingEnumeration : byte
        {
            [Description("45 кН·м (4,5 тс·м)")]
            k45,
            [Description("59 кН·м (6,0 тс·м)")]
            k59,
            [Description("79 кН·м (8,0 тс·м)")]
            k79,
            [Description("98 кН·м (10,0 тс·м)")]
            k98,
            [Description("117 кН·м (12,0 тс·м)")]
            k117,
            [Description("147 кН·м (15,0 тс·м)")]
            k147,
            [Description("Другое")]
            Other
        }

        public override Type PropertyType => typeof(string);

        public override string Name => "Несущая способность";
        public override string Category => "Стойка";

        private LoadBearingEnumeration Value;

        public override ICollection GetStandardValues()
        {

            var values = Enum.GetValues(typeof(LoadBearingEnumeration))
              .Cast<Enum>()
              .Select(f => ConverterterHelper.GetDescriptionFromEnumValue(f))
              .ToArray();

            return values;
        }

        public override StandardValueTypeEnum GetStandardValueType() => StandardValueTypeEnum.Exclusive;
        public override object GetValue() => ConverterterHelper.GetDescriptionFromEnumValue(Value);

        public override bool SetValue(object val)
        {
            Value = ConverterterHelper.GetEnumValueFromDescription<LoadBearingEnumeration>(val.ToString());
            return true;
        }
    }

    [Serializable]
    class ArmoredLenght : McDynamicProperty
    {
        public enum MastArmoredLenghtEnumeration
        {
            [Description("10.0 м")]
            [DefaultValue(10000d)]
            L10000,
            [Description("10.4 м")]
            [DefaultValue(10400d)]
            L10400,
            [Description("10.8 м")]
            [DefaultValue(10800d)]
            L10800,
            [Description("12.0 м")]
            [DefaultValue(12000d)]
            L12000,
            [Description("12.8 м")]
            [DefaultValue(12800d)]
            L12800,
            [Description("13.6 м")]
            [DefaultValue(13600d)]
            L13600,
            [Description("14.6 м")]
            [DefaultValue(14600d)]
            L14600,
            [Description("Другое")]
            Other
        }
        public override Type PropertyType => typeof(string);

        public override string Name => "Длинна";
        public override string Category => "Стойка";

        private MastArmoredLenghtEnumeration Value;

        public override ICollection GetStandardValues()
        {
            return Enum.GetValues(typeof(MastArmoredLenghtEnumeration))
                .Cast<Enum>()
                .Select(f => ConverterterHelper.GetDescriptionFromEnumValue(f))
                .ToArray();
        }

        public override StandardValueTypeEnum GetStandardValueType() => StandardValueTypeEnum.Exclusive;
        public override object GetValue() => ConverterterHelper.GetDescriptionFromEnumValue(Value);

        public override bool SetValue(object val)
        {
            Value = ConverterterHelper.GetEnumValueFromDescription<MastArmoredLenghtEnumeration>(val.ToString());
            return true;
        }

    }
    [Serializable]
    class MetallLenght : McDynamicProperty
    {
        public enum MastMetallLenghtEnumeration
        {
            [Description("9.6 м")]
            [DefaultValue(9600d)]
            L9600,
            [Description("12.0 м")]
            [DefaultValue(12000d)]
            L12000,
            [Description("13.0 м")]
            [DefaultValue(13000d)]
            L13000,
            [Description("Другое")]
            Other
        }
        public override Type PropertyType => typeof(string);

        public override string Name => "Длинна";
        public override string Category => "Стойка";

        private MastMetallLenghtEnumeration Value;

        public override ICollection GetStandardValues()
        {
            return Enum.GetValues(typeof(MastMetallLenghtEnumeration))
                .Cast<Enum>()
                .Select(f => ConverterterHelper.GetDescriptionFromEnumValue(f))
                .ToArray();
        }

        public override StandardValueTypeEnum GetStandardValueType() => StandardValueTypeEnum.Exclusive;
        public override object GetValue() => ConverterterHelper.GetDescriptionFromEnumValue(Value);

        public override bool SetValue(object val)
        {
            Value = ConverterterHelper.GetEnumValueFromDescription<MastMetallLenghtEnumeration>(val.ToString());
            return true;
        }

    }

}
