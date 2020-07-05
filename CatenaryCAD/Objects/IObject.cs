﻿using CatenaryCAD.Geometry;
using CatenaryCAD.Parts;
using CatenaryCAD.Properties;

using System;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects
{
    public interface IObject
    {
        public event Action Updated;
        public IPart[] GetParts();
        public IProperty[] GetProperties();
        public AbstractGeometry[] GetGeometry(NatureType type);
    }
}
