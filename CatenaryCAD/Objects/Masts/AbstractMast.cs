using CatenaryCAD.Geometry;
using CatenaryCAD.Properties;
using Multicad.CustomObjectBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace CatenaryCAD.Objects.Masts
{
    public enum GeometryType
    {
        Geometry2D,
        Geometry3D,
    }
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
        public virtual AbstractGeometry[] GetGeometry(GeometryType type)
        {
            switch(type)
            {
                case GeometryType.Geometry2D: return Geometry2D;
                case GeometryType.Geometry3D: return Geometry3D;

                default: return Geometry2D;
            }
        }

        public abstract void GetGeometry2D(GeometryBuilder geometry, Color colour, double scale);
        public abstract void GetGeometry3D(GeometryBuilder geometry, Color colour, double scale);
    }
}
