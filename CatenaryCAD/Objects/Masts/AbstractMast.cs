using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static CatenaryCAD.Extensions;

namespace CatenaryCAD.Objects.Masts
{

    [Serializable]
    public abstract class AbstractMast
    {
        public abstract event Action Updated;

        internal static Dictionary<string, Type> GetMastFromCatenaryObjects()
        {
            //получаем все не абстрактные обьекты производные от AbstractMast и имеющие атрибут CatenaryObjectAttribute
            var masts = Main.CatenaryObjects
                .Where(abstr => !abstr.IsAbstract)
                .Where(bastype => bastype.BaseType == typeof(AbstractMast))
                .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false))
                .ToArray();

            //получем словарь из CatenaryObjectAttribute и типа производного от AbstractMast класса
            return masts.Select((type) => new
            {
                type,
                atrr = type.GetCustomAttributes(typeof(CatenaryObjectAttribute), false)
                            .FirstOrDefault() as CatenaryObjectAttribute
            }).ToDictionary(p => p.atrr.Type, p => p.type);
        }

        public abstract AbstractProperty[] GetProperties();
        public abstract AbstractGeometry[] GetGeometry(ViewType type);
    }
}
