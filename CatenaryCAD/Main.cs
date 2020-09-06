using CatenaryCAD.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CatenaryCAD
{
    internal sealed class Main : Multicad.Runtime.IExtensionApplication
    {
        /// <summary>
        /// Кэшированные производные от IObject объекты
        /// </summary>
        public static Type[] CatenaryObjects { private set; get; }

        /// <summary>
        /// Загружает производные от IObject объекты из плагинов *.dll раположенных в <paramref name="directory"/>
        /// </summary>
        public static Type[] GetCatenaryObject(string directory)
        {
            var files = Directory.GetFiles(directory, "*.dll");

            return files.Where((file) => file != Assembly.GetExecutingAssembly().Location)
                        .Select((file) => Assembly.Load(file).GetTypes()
                        .Where(interf => interf.GetInterface(typeof(IObject).FullName) != null))
                        .SelectMany((arr)=> arr).ToArray();
        }

        /// <summary>
        /// Возвращает из числа кэшированных объектов в <see cref="CatenaryObjects"/> 
        /// все не абстрактные обьекты, производные от <paramref name="type"/>.
        /// </summary>
        public static Type[] GetCatenaryObjectFor(Type type)
        {
            return Main.CatenaryObjects
                    .Where(abstr => !abstr.IsAbstract)
                    .Where(interf => interf.GetInterface(type.FullName) != null).ToArray();
        }

        public void Initialize()
        {
            CatenaryObjects = GetCatenaryObject(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) ;
        }

        public void Terminate()
        {

        }



    }
}
