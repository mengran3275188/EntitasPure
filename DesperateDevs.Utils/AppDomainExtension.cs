using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DesperateDevs.Utils
{
	public static class AppDomainExtension
	{
		public static Type[] GetAllTypes(this AppDomain appDomain)
		{
			return appDomain.GetAssemblies().GetAllTypes();
		}

		public static Type[] GetAllTypes(this IEnumerable<Assembly> assemblies)
		{
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					list.AddRange(assembly.GetTypes());
				}
				catch (ReflectionTypeLoadException ex)
				{
					list.AddRange(from type in ex.Types
					where type != null
					select type);
				}
			}
			return list.ToArray();
		}

		public static Type[] GetNonAbstractTypes<T>(this AppDomain appDomain)
		{
			return appDomain.GetAllTypes().GetNonAbstractTypes<T>();
		}

		public static Type[] GetNonAbstractTypes<T>(this Type[] types)
		{
			return (from type in types
			where !type.IsAbstract
			where InterfaceTypeExtension.ImplementsInterface<T>(type)
			select type).ToArray();
		}

		public static T[] GetInstancesOf<T>(this AppDomain appDomain)
		{
			return appDomain.GetNonAbstractTypes<T>().GetInstancesOf<T>();
		}

		public static T[] GetInstancesOf<T>(this Type[] types)
		{
			return (from type in (IEnumerable<Type>)types.GetNonAbstractTypes<T>()
			select (T)Activator.CreateInstance(type)).ToArray();
		}
	}
}
