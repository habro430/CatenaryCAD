using System;
using System.Security;

using Catenary.Helpers.DeepCloner;

namespace Catenary.Helpers
{
	// The contents of this file is taken from https://github.com/force-net/DeepCloner/blob/develop/DeepCloner/DeepClonerExtensions.cs
	// To be replaced by the framework implementation when released for the appropriate builds

	// Author licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.

	/// <summary>
	/// Расширения для клонирования объектов
	/// </summary>
	public static class Cloner
	{
		/// <summary>
		/// Выполняет глубокую(полную) копию объекта и связанного графа.
		/// </summary>
		public static T DeepClone<T>(this T obj) => DeepClonerGenerator.CloneObject(obj);

		/// <summary>
		/// Выполняет глубокое (полное) копирование объекта и связанного графа в существующий объект.
		/// </summary>
		// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static T DeepCloneTo<F, T>(this F from, T to) where T : class, F => 
			(T)DeepClonerGenerator.CloneObjectTo(from, to, true);

		/// <summary>
		/// Выполняет неглубокую(возвращается только новый объект, без клонирования зависимостей) копию объекта.
		/// </summary>
		public static T ShallowClone<T>(this T obj) => ShallowClonerGenerator.CloneObject(obj);

		/// <summary>
		/// Выполняет неглубокую копию объекта в существующий объект.
		/// </summary>
		// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static T ShallowCloneTo<F, T>(this F from, T to) where T : class, F => 
			(T)DeepClonerGenerator.CloneObjectTo(from, to, false);


		static Cloner()
		{
			if (!PermissionCheck())
			{
				throw new SecurityException("DeepCloner should have enough permissions to run. Grant FullTrust or Reflection permission.");
			}
		}

		private static bool PermissionCheck()
		{
			// best way to check required permission: execute something and receive exception
			// .net security policy is weird for normal usage
			try
			{
				new object().ShallowClone();
			}
			catch (VerificationException)
			{
				return false;
			}
			catch (MemberAccessException)
			{
				return false;
			}
			
			return true;
		}
	}
}
