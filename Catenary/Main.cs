﻿using Catenary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Catenary
{
    public sealed class Main
    {

        /// <summary>
        /// Массив из кэшированных и производных от <see cref="IModel"/> объектов.
        /// </summary>
        /// <returns>Объекты <see cref="IModel"/>.</returns>
        public static Type[] CatenaryObjects { private set; get; }

        /// <summary>
        /// Возвращает из числа кэшированных объектов в <see cref="CatenaryObjects"/>, 
        /// все (включая абстрактные) обьекты, производные от интефейса <paramref name="iface"/>.
        /// </summary>
        /// <param name="iface">Базовый интерфейс</param>
        /// <returns>Объекты <see cref="IModel"/>.</returns>
        public static Type[] GetCatenaryObjects(Type iface)
        {
            if (!iface.IsInterface) throw new ArgumentException("Параметр должен быть интерфейсом.", iface.Name);
            return Main.CatenaryObjects.Where(interf => iface.IsAssignableFrom(interf)).ToArray();
        }

        /// <summary>
        /// Возвращает из числа кэшированных объектов в <see cref="CatenaryObjects"/>, 
        /// все (включая абстрактные) обьекты, производные от интефейса <paramref name="iface"/> и базового типа <paramref name="btype"/>.
        /// </summary>
        /// <param name="iface">Базовый интерфейс</param>
        /// <param name="btype">Базовый тип</param>
        /// <returns>Объекты <see cref="IModel"/>.</returns>
        public static Type[] GetCatenaryObjects(Type iface, Type btype)
        {
            if (!iface.IsInterface) throw new ArgumentException("Параметр должен быть интерфейсом.", iface.Name);
            if (!btype.IsClass) throw new ArgumentException("Параметр должен быть классом.", btype.Name);

            return Main.CatenaryObjects
                    .Where(type => type.GetInterface(iface.FullName) != null)
                    .Where(type => type.BaseType == btype).ToArray();
        }

        private static Assembly[] CatenaryAssemblies { set; get; }
        private static Type[] LoadCatenaryObjects(string directory)
        {
            var current_asm_file = Assembly.GetExecutingAssembly().Location;
            var asm_files = Directory.GetFiles(directory, "*.dll").Where((file) => file != current_asm_file).ToArray();

            CatenaryAssemblies = asm_files.Select((file) => Assembly.Load(file)).ToArray();
            return CatenaryAssemblies.Select((asm) => asm.GetExportedTypes()
                                     .Where(interf => typeof(IModel).IsAssignableFrom(interf)))
                                     .SelectMany((arr) => arr).ToArray();
        }

        static Main()
        {
            CatenaryObjects = LoadCatenaryObjects(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }
    }
}
