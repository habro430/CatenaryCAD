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
        public static Type[] CatenaryObjects { private set; get; }
        public void Initialize()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            var types = new List<Type>();

            foreach (var file in files)
            {
                Assembly asm =  Assembly.Load(file);
                types.AddRange(asm.GetTypes()
                                    .Where(attr => Attribute.IsDefined(attr, typeof(CatenaryObjectAttribute), false)));
            }

            CatenaryObjects = types.ToArray();

        }

        public void Terminate()
        {

        }
    }
}
