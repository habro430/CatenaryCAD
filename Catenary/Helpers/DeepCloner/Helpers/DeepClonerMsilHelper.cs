﻿#if !NETCORE
using System;
using System.Reflection;

namespace Catenary.Helpers.DeepCloner
{
	internal static class DeepClonerMsilHelper
	{
		// The contents of this file is taken from https://github.com/force-net/DeepCloner/blob/develop/DeepCloner/Helpers/DeepClonerMsilHelper.cs
		// To be replaced by the framework implementation when released for the appropriate builds

		// Author licenses this file to you under the MIT license.
		// See the LICENSE file in the project root for more information.
		public static bool IsConstructorDoNothing(Type type, ConstructorInfo constructor)
		{
			if (constructor == null) return false;
			try
			{
				// will not try to determine body for this types
				if (type.IsGenericType || type.IsContextful || type.IsCOMObject || type.Assembly.IsDynamic) return false;

				var methodBody = constructor.GetMethodBody();

				// this situation can be for com
				if (methodBody == null) return false;

				var ilAsByteArray = methodBody.GetILAsByteArray();
				if (ilAsByteArray.Length == 7
					&& ilAsByteArray[0] == 0x02 // Ldarg_0
					&& ilAsByteArray[1] == 0x28 // newobj
					&& ilAsByteArray[6] == 0x2a // ret
					&& type.Module.ResolveMethod(BitConverter.ToInt32(ilAsByteArray, 2)) == typeof(object).GetConstructor(Type.EmptyTypes)) // call object
				{
					return true;
				}
				else if (ilAsByteArray.Length == 1 && ilAsByteArray[0] == 0x2a) // ret
				{
					return true;
				}

				return false;
			}
			catch (Exception)
			{
				// no permissions or something similar
				return false;
			}
		}
	}
}
#endif