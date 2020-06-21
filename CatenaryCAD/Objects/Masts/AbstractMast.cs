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

        protected virtual AbstractGeometry[] Geometry2D { get; set; }
        protected virtual AbstractGeometry[] Geometry3D { get; set; }


        public static Dictionary<string, Type> GetInheritedMastsFor(Type abst_mast_type)
        {
            //получаем все не абстрактные обьекты производные от 'abst_mast_type' и имеющие атрибут MastTypeAttribute
            var masts = Assembly.GetExecutingAssembly().GetTypes()
                .Where(abstr => !abstr.IsAbstract)
                .Where(bastype => bastype.BaseType == abst_mast_type)
                .Where(attr => Attribute.IsDefined(attr, typeof(MastAttribute), false))
                .ToArray();

            //получем словарь из MastTypeAttribute и типа производного от 'abst_mast_type' класса
            return masts.Select((type) => new
            {
                type,
                atrr = type.GetCustomAttributes(typeof(MastAttribute), false)
                            .FirstOrDefault() as MastAttribute
            }).ToDictionary(p => p.atrr.Type, p => p.type);
        }

        public abstract AbstractProperty[] GetProperties();
        public virtual AbstractGeometry[] GetGeometry(ViewType type)
        {
            switch(type)
            {
                case ViewType.Geometry2D: return Geometry2D;
                case ViewType.Geometry3D: return Geometry3D;

                default: return Geometry2D;
            }
        }
    }
}
