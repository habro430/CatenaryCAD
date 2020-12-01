﻿using System;
using System.Security;

using CatenaryCAD.Helpers.DeepCloner;

namespace CatenaryCAD.Helpers
{
	// The contents of this file is taken from https://github.com/force-net/DeepCloner/blob/develop/DeepCloner/DeepClonerExtensions.cs
	// To be replaced by the framework implementation when released for the appropriate builds

	// Author licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.

	/// <summary>
	/// Расширения для клонирования объектов
	/// </summary>
	public static class DeepClonerExtensions
	{
		/// <summary>
		/// Выполняет глубокую(полную) копию объекта и связанного графа.
		/// </summary>
		public static T DeepClone<T>(this T obj) => DeepClonerGenerator.CloneObject(obj);

		/// <summary>
		/// Выполняет глубокое (полное) копирование объекта и связанного графа в существующий объект.
		/// </summary>
		// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static TTo DeepCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom => 
			(TTo)DeepClonerGenerator.CloneObjectTo(objFrom, objTo, true);

		/// <summary>
		/// Выполняет неглубокую(возвращается только новый объект, без клонирования зависимостей) копию объекта.
		/// </summary>
		public static T ShallowClone<T>(this T obj) => ShallowClonerGenerator.CloneObject(obj);

		/// <summary>
		/// Выполняет неглубокую копию объекта в существующий объект.
		/// </summary>
		// <remarks>Method is valid only for classes, classes should be descendants in reality, not in declaration</remarks>
		public static TTo ShallowCloneTo<TFrom, TTo>(this TFrom objFrom, TTo objTo) where TTo : class, TFrom => 
			(TTo)DeepClonerGenerator.CloneObjectTo(objFrom, objTo, false);


		static DeepClonerExtensions()
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
