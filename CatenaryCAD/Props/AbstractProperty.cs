﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD.Properties
{
    [Serializable, Flags]
    public enum PropertyFlags
    {
        None = 0,
        ReadOnly = 1,
        RefreshAfterChange = 2,
        NotBrowsable = 4,
    }

    [Serializable]
    public abstract class AbstractProperty
    {
        public abstract string ID {  get; protected set; }
        public abstract string Name { get; protected set; }
        public abstract string Category { get; protected set; }
        public abstract PropertyFlags Properties { get; protected set; }

        public abstract object GetValue();
        public abstract bool SetValue(object value);

        public abstract Type GetValueType();
        public abstract ICollection GetValuesCollection();
        internal virtual AdapterProperty ToAdapterProperty() => new AdapterProperty(this);
       
    }
}
