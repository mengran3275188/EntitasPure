using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator
{
	public static class CodeGeneratorUtil
	{
		public static CodeGenerator CodeGeneratorFromProperties()
		{
			Properties properties = Preferences.LoadProperties();
			CodeGeneratorConfig codeGeneratorConfig = new CodeGeneratorConfig();
			codeGeneratorConfig.Configure(properties);
			Type[] types = CodeGeneratorUtil.LoadTypesFromPlugins(properties);
			ICodeGeneratorDataProvider[] enabledInstances = CodeGeneratorUtil.GetEnabledInstances<ICodeGeneratorDataProvider>(types, codeGeneratorConfig.dataProviders);
			ICodeGenerator[] enabledInstances2 = CodeGeneratorUtil.GetEnabledInstances<ICodeGenerator>(types, codeGeneratorConfig.codeGenerators);
			ICodeGenFilePostProcessor[] enabledInstances3 = CodeGeneratorUtil.GetEnabledInstances<ICodeGenFilePostProcessor>(types, codeGeneratorConfig.postProcessors);
			CodeGeneratorUtil.configure(enabledInstances, properties);
			CodeGeneratorUtil.configure(enabledInstances2, properties);
			CodeGeneratorUtil.configure(enabledInstances3, properties);
			return new CodeGenerator(enabledInstances, enabledInstances2, enabledInstances3);
		}

		private static void configure(ICodeGeneratorInterface[] plugins, Properties properties)
		{
			foreach (IConfigurable item in plugins.OfType<IConfigurable>())
			{
				item.Configure(properties);
			}
		}

		public static Type[] LoadTypesFromPlugins(Properties properties)
		{
			CodeGeneratorConfig codeGeneratorConfig = new CodeGeneratorConfig();
			codeGeneratorConfig.Configure(properties);
			AssemblyResolver assemblyResolver = new AssemblyResolver(false, codeGeneratorConfig.searchPaths);
			string[] plugins = codeGeneratorConfig.plugins;
			foreach (string path in plugins)
			{
				assemblyResolver.Load(path);
			}
			return assemblyResolver.GetTypes();
		}

		public static string[] GetOrderedNames(string[] types)
		{
			return (from type in types
			orderby type
			select type).ToArray();
		}

		public static T[] GetOrderedInstances<T>(Type[] types) where T : ICodeGeneratorInterface
		{
			return (from type in types
			where InterfaceTypeExtension.ImplementsInterface<T>(type)
			where !type.IsAbstract
			select (T)Activator.CreateInstance(type) into instance
			orderby instance.priority, instance.GetType().ToCompilableString()
			select instance).ToArray();
		}

		public static string[] GetOrderedTypeNames<T>(Type[] types) where T : ICodeGeneratorInterface
		{
			return (from instance in (IEnumerable<T>)CodeGeneratorUtil.GetOrderedInstances<T>(types)
			select instance.GetType().ToCompilableString()).ToArray();
		}

		public static T[] GetEnabledInstances<T>(Type[] types, string[] enabledTypeNames) where T : ICodeGeneratorInterface
		{
			return (from instance in (IEnumerable<T>)CodeGeneratorUtil.GetOrderedInstances<T>(types)
			where Enumerable.Contains<string>((IEnumerable<string>)enabledTypeNames, instance.GetType().ToCompilableString())
			select instance).ToArray();
		}

		public static string[] GetAvailable<T>(Type[] types, string[] enabledTypeNames) where T : ICodeGeneratorInterface
		{
			return (from typeName in CodeGeneratorUtil.GetOrderedTypeNames<T>(types)
			where !Enumerable.Contains<string>((IEnumerable<string>)enabledTypeNames, typeName)
			select typeName).ToArray();
		}

		public static string[] GetUnavailable<T>(Type[] types, string[] enabledTypeNames) where T : ICodeGeneratorInterface
		{
			string[] typeNames = CodeGeneratorUtil.GetOrderedTypeNames<T>(types);
			return (from typeName in enabledTypeNames
			where !Enumerable.Contains<string>((IEnumerable<string>)typeNames, typeName)
			select typeName).ToArray();
		}

		public static T[] GetUsed<T>(Type[] types, string[] enabledTypeNames) where T : ICodeGeneratorInterface
		{
			return (from instance in (IEnumerable<T>)CodeGeneratorUtil.GetOrderedInstances<T>(types)
			where Enumerable.Contains<string>((IEnumerable<string>)enabledTypeNames, instance.GetType().ToCompilableString())
			select instance).ToArray();
		}

		public static Dictionary<string, string> GetConfigurables(ICodeGeneratorDataProvider[] dataProviders, ICodeGenerator[] codeGenerators, ICodeGenFilePostProcessor[] postProcessors)
		{
			return new Dictionary<string, string>().Merge((from instance in dataProviders.OfType<IConfigurable>().Concat(codeGenerators.OfType<IConfigurable>()).Concat(postProcessors.OfType<IConfigurable>())
			select instance.defaultProperties).ToArray());
		}

		public static Dictionary<string, string> GetMissingConfigurables(Dictionary<string, string> configurables, Properties properties)
		{
			return (from kv in configurables
			where !properties.HasKey(kv.Key)
			select kv).ToDictionary((KeyValuePair<string, string> kv) => kv.Key, (KeyValuePair<string, string> kv) => kv.Value);
		}
	}
}
