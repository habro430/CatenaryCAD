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
        /// Кэшированные объекты подгруженные из плагинов *.dll и производные от IObject 
        /// </summary>
        public static Type[] CatenaryObjects { private set; get; }

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
            var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll");
            var types = new List<Type>();

            foreach (var file in files)
            {
                Assembly asm = Assembly.Load(file);
                types.AddRange(asm.GetTypes()
                        .Where(interf => interf.GetInterface(typeof(IObject).FullName) != null));
            }
            CatenaryObjects = types.ToArray();
        }

        public void Terminate()
        {

        }



    }
}
