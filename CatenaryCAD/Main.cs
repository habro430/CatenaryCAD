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
        public static Type[] CachedCatenaryObjects { private set; get; }
        public void Initialize()
        {
            var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll");
            var types = new List<Type>();

            foreach (var file in files)
            {
                Assembly asm = Assembly.Load(file);
                types.AddRange(asm.GetTypes()
                                    .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false)));
            }
            CachedCatenaryObjects = types.ToArray();
        }

        public void Terminate()
        {

        }
        
    }
}
