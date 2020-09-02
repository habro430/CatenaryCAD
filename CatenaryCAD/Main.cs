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
        /// Кэшированные объекты, производные от IObject подгруженных из плагинов *.dll
        /// </summary>
        public static Type[] CatenaryObjectTypes { private set; get; }

        /// <summary>
        /// Возвращает из числа кэшированных объектов в <see cref="CatenaryObjectTypes"/> 
        /// все не абстрактные обьекты, производные от <paramref name="type"/> и имеющих атрибут 
        /// <see cref="CatenaryObjectAttribute"/>.
        /// </summary>
        public static Type[] GetCatenaryObjectTypesFor(Type type)
        {
            return Main.CatenaryObjectTypes
                    .Where(abstr => !abstr.IsAbstract)
                    .Where(interf => interf.GetInterface(type.FullName) != null)
                    .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false)).ToArray();
        }

        public void Initialize()
        {
            var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll");
            var types = new List<Type>();

            foreach (var file in files)
            {
                Assembly asm = Assembly.Load(file);
                types.AddRange(asm.GetTypes()
                        .Where(interf => interf.GetInterface(typeof(IObject).FullName) != null)
                        .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false)));
            }
            CatenaryObjectTypes = types.ToArray();
        }

        public void Terminate()
        {

        }



    }
}
