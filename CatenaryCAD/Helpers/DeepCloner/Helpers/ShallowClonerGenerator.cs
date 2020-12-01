using System;

namespace CatenaryCAD.Helpers.DeepCloner
{
	// The contents of this file is taken from https://github.com/force-net/DeepCloner/blob/develop/DeepCloner/Helpers/ShallowClonerGenerator.cs
	// To be replaced by the framework implementation when released for the appropriate builds

	// Author licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.
	internal static class ShallowClonerGenerator
	{
		public static T CloneObject<T>(T obj)
		{
			// this is faster than typeof(T).IsValueType
			if (obj is ValueType)
			{
				if (typeof(T) == obj.GetType()) 
					return obj;
				
				// we're here so, we clone value type obj as object type T
				// so, we need to copy it, bcs we have a reference, not real object.
				return (T)ShallowObjectCloner.CloneObject(obj);
			}

			if (ReferenceEquals(obj, null))
				return (T)(object)null;
			
			if (DeepClonerSafeTypes.CanReturnSameObject(obj.GetType()))
				return obj;

			return (T)ShallowObjectCloner.CloneObject(obj);
		}
	}
}
