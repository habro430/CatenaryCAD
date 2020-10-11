﻿using BasicMasts.Properties;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using CatenaryCAD.Models;
using CatenaryCAD.Parts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Caching;
using System.Text;

namespace BasicMasts
{
    [Serializable]
    public abstract class AbstractMast : Mast
    {
        protected IShape[] Geometry2D;
        protected IMesh[] Geometry3D;

        [NonSerialized]
        private static ObjectCache Cache = new MemoryCache(typeof(AbstractMast).Name);

        protected internal static Mesh GetOrCreateFromCache(string key)
        {
            //есть ли 3d модель в кэше
            if (!Cache.Contains(key))
            {
                //если нет то читаем 3d модель из ресурсов, генерируем и кэшируем 
                ResourceManager rm = new ResourceManager(typeof(Resources));

                //декодируем модель из base64
                string model = Encoding.Default.GetString(Convert.FromBase64String(rm.GetString(key)));

                //генерируем модель и пишем в кэш
                Cache.Set(key, Mesh.FromObj(model), new CacheItemPolicy());
            }

            //читаем модель из кэша и возвращаем
            return Cache.Get(key) as Mesh;
        }
        protected internal static Dictionary<string, Type> GetInheritedMastsFor(Type abst_mast_type)
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


        public override IMesh[] GetGeometryForLayout() => Geometry3D;
        public override IShape[] GetGeometryForScheme() => Geometry2D;


    }
}
