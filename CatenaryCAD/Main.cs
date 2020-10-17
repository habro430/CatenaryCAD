using CatenaryCAD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CatenaryCAD
{
    public sealed class Main : Multicad.Runtime.IExtensionApplication
    {

        /// <summary>
        /// Кэшированные производные от IObject объекты
        /// </summary>
        public static Type[] CatenaryObjects { private set; get; }

        /// <summary>
        /// Загружает производные от IObject объекты из плагинов *.dll раположенных в <paramref name="directory"/>
        /// </summary>
        private static Type[] GetCatenaryObjects(string directory)
        {
            var current_asm_file = Assembly.GetExecutingAssembly().Location;
            var asm_files = Directory.GetFiles(directory, "*.dll").Where((file) => file != current_asm_file).ToArray() ;


            return asm_files.Select((file) => Assembly.Load(file).GetExportedTypes()
                            .Where(abstr => !abstr.IsAbstract)
                            .Where(interf =>  typeof(IModel).IsAssignableFrom(interf)))
                            .SelectMany((arr) => arr).ToArray();
        }

        /// <summary>
        /// Возвращает из числа кэшированных объектов в <see cref="CatenaryObjects"/> 
        /// все не абстрактные обьекты, производные от интефейса <paramref name="iface"/>.
        /// </summary>
        /// <param name="iface">Базовый интерфейс</param>
        public static Type[] GetCatenaryObjects(Type iface)
        {
            if (!iface.IsInterface) throw new ArgumentException("Параметр должен быть интерфейсом.", iface.Name);

            return Main.CatenaryObjects.Where(interf => iface.IsAssignableFrom(interf)).ToArray();
        }

        /// <summary>
        /// Возвращает словарь из числа кэшированных объектов в <see cref="CatenaryObjects"/> 
        /// все не абстрактные обьекты, производные от интефейса <paramref name="iface"/>.
        /// </summary>
        /// <param name="iface">Базовый интерфейс</param>
        //public static Dictionary<string,Type> GetDictionaryCatenaryObjects(Type iface)
        //{

        //}

        /// <summary>
        /// Возвращает из числа кэшированных объектов в <see cref="CatenaryObjects"/> 
        /// все не абстрактные обьекты, производные от интефейса <paramref name="iface"/> и базового типа <paramref name="btype"/>.
        /// </summary>
        /// <param name="iface">Базовый интерфейс</param>
        /// <param name="btype">Базовый тип</param>
        /// <returns></returns>
        public static Type[] GetCatenaryObjects(Type iface, Type btype)
        {
            if (!iface.IsInterface) throw new ArgumentException("Параметр должен быть интерфейсом.", iface.Name);
            if (!btype.IsClass) throw new ArgumentException("Параметр должен быть классом.", btype.Name);

            return Main.CatenaryObjects
                    .Where(type => !type.IsAbstract)
                    .Where(type => type.GetInterface(iface.FullName) != null)
                    .Where(type => type.BaseType == btype).ToArray();
        }

        public void Initialize()
        {
            CatenaryObjects = GetCatenaryObjects(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) ;
        }

        public void Terminate()
        {

        }



    }
}
