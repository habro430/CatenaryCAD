using Multicad;
using System;
using System.Collections;

namespace CatenaryCAD.Properties
{
    internal class AdapterProperty : McDynamicProperty
    {
        private IProperty property;

        public AdapterProperty(IProperty prop) => property = prop;
        public override Type PropertyType => property.GetValueType();

        public override string Name => property.ID;
        public override string DisplayName => property.Name;
        public override string Category => property.Category;

        public override bool IsBrowsable => !property.Properties.HasFlag(PropertyFlags.NotBrowsable);
        public override bool IsReadOnly => property.Properties.HasFlag(PropertyFlags.ReadOnly);
        public override ExAttributesProperties ExtendedAtributes
        {
            get
            {
                if (property.Properties.HasFlag(PropertyFlags.RefreshAfterChange)) return ExAttributesProperties.RefreshListAfterChange;
                else return ExAttributesProperties.None;
            }
        }
        
        public override object GetValue() => property.GetValue();

        public override ICollection GetStandardValues() => property.GetValuesCollection();
        public override StandardValueTypeEnum GetStandardValueType()
        {
            if (property.GetValuesCollection() != null)
                return StandardValueTypeEnum.Exclusive;
            else
                return StandardValueTypeEnum.None;
        }

        public override bool SetValue(object val) => property.SetValue(val);
    }
}
