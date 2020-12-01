#if !NETCORE
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CatenaryCAD.Helpers.DeepCloner
{
	// The contents of this file is taken from https://github.com/force-net/DeepCloner/blob/develop/DeepCloner/Helpers/TypeCreationHelper.cs
	// To be replaced by the framework implementation when released for the appropriate builds

	// Author licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.
	internal static class TypeCreationHelper
	{
		private static ModuleBuilder _moduleBuilder;

		internal static ModuleBuilder GetModuleBuilder()
		{
			// todo: think about multithread
			if (_moduleBuilder == null)
			{
				AssemblyName aName = new AssemblyName("DeepClonerCode");
				var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
				var mb = ab.DefineDynamicModule(aName.Name);
				_moduleBuilder = mb;
			}

			return _moduleBuilder;
		}
	}
}
#endif