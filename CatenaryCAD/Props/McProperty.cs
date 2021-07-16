using Multicad;
using System;
using System.Collections;

namespace Catenary.Properties
{
    internal class McProperty : McDynamicProperty
    {
        private IProperty property;

        public McProperty(IProperty prop) => property = prop;
        public override Type PropertyType => property.GetValueType();

        public override string Name => property.Name;
        public override string DisplayName => property.Name;
        public override string Category => property.Category;

        public override bool IsBrowsable => property.Attributes.HasValue ? !property.Attributes.Value.HasFlag(Attributes.NotBrowsable) : true;

        public override bool IsReadOnly => property.Attributes.HasValue ? property.Attributes.Value.HasFlag(Attributes.ReadOnly) : false;
        public override ExAttributesProperties ExtendedAtributes
        {
            get
            {
                if (property.Attributes.HasValue ? property.Attributes.Value.HasFlag(Attributes.RefreshAfterChange) : false) 
                    return ExAttributesProperties.RefreshListAfterChange;
                else return ExAttributesProperties.None;
            }
        }

        public override object GetValue() => property.Value;

        public override ICollection GetStandardValues() => property.DropDownValues;
        public override StandardValueTypeEnum GetStandardValueType()
        {
            if (property.DropDownValues != null)
                return StandardValueTypeEnum.Exclusive;
            else
                return StandardValueTypeEnum.None;
        }

        public override bool SetValue(object val)
        {
            property.Value = val;
            return true;
        }
    }
}
