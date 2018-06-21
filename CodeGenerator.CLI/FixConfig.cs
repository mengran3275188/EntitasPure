using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public class FixConfig : AbstractCommand
	{
		public override string trigger
		{
			get
			{
				return "fix";
			}
		}

		public override void Run(string[] args)
		{
			if (base.assertProperties())
			{
				Properties properties = base.loadProperties();
				CodeGeneratorConfig codeGeneratorConfig = new CodeGeneratorConfig();
				codeGeneratorConfig.Configure(properties);
				Type[] array = null;
				Dictionary<string, string> dictionary = null;
				try
				{
					array = CodeGeneratorUtil.LoadTypesFromPlugins(properties);
					dictionary = CodeGeneratorUtil.GetConfigurables(CodeGeneratorUtil.GetUsed<ICodeGeneratorDataProvider>(array, codeGeneratorConfig.dataProviders), CodeGeneratorUtil.GetUsed<ICodeGenerator>(array, codeGeneratorConfig.codeGenerators), CodeGeneratorUtil.GetUsed<ICodeGenFilePostProcessor>(array, codeGeneratorConfig.postProcessors));
				}
				catch (Exception ex)
				{
					FixConfig.fixKeys(null, codeGeneratorConfig.defaultProperties, properties);
					throw ex;
				}
				FixConfig.fixKeys(dictionary, codeGeneratorConfig.defaultProperties, properties);
				FixConfig.fixConfigurableKeys(dictionary, properties);
				FixConfig.fixPlugins(array, codeGeneratorConfig, properties);
			}
		}

		private static void fixKeys(Dictionary<string, string> configurables, Dictionary<string, string> defaultProperties, Properties properties)
		{
			string[] array = defaultProperties.Keys.ToArray();
			string[] array2 = array.ToList().ToArray();
			if (configurables != null)
			{
				array2 = array2.Concat(configurables.Keys).ToArray();
			}
			string[] missingKeys = Helper.GetMissingKeys(array, properties);
			foreach (string key in missingKeys)
			{
				Helper.AddKey("Add missing key", key, defaultProperties[key], properties);
			}
			missingKeys = Helper.GetUnusedKeys(array2, properties);
			foreach (string key2 in missingKeys)
			{
				Helper.RemoveKey("Remove unused key", key2, properties);
			}
		}

		private static void fixConfigurableKeys(Dictionary<string, string> configurables, Properties properties)
		{
			foreach (KeyValuePair<string, string> missingConfigurable in CodeGeneratorUtil.GetMissingConfigurables(configurables, properties))
			{
				Helper.AddKey("Add missing key", missingConfigurable.Key, missingConfigurable.Value, properties);
			}
		}

		private static void fixPlugins(Type[] types, CodeGeneratorConfig config, Properties properties)
		{
			string[] unavailable = CodeGeneratorUtil.GetUnavailable<ICodeGeneratorDataProvider>(types, config.dataProviders);
			string[] unavailable2 = CodeGeneratorUtil.GetUnavailable<ICodeGenerator>(types, config.codeGenerators);
			string[] unavailable3 = CodeGeneratorUtil.GetUnavailable<ICodeGenFilePostProcessor>(types, config.postProcessors);
			string[] available = CodeGeneratorUtil.GetAvailable<ICodeGeneratorDataProvider>(types, config.dataProviders);
			string[] available2 = CodeGeneratorUtil.GetAvailable<ICodeGenerator>(types, config.codeGenerators);
			string[] available3 = CodeGeneratorUtil.GetAvailable<ICodeGenFilePostProcessor>(types, config.postProcessors);
			string[] array = unavailable;
			foreach (string value in array)
			{
				Helper.RemoveValue("Remove unavailable data provider", value, config.dataProviders, delegate(string[] values)
				{
					config.dataProviders = values;
				}, properties);
			}
			array = unavailable2;
			foreach (string value2 in array)
			{
				Helper.RemoveValue("Remove unavailable code generator", value2, config.codeGenerators, delegate(string[] values)
				{
					config.codeGenerators = values;
				}, properties);
			}
			array = unavailable3;
			foreach (string value3 in array)
			{
				Helper.RemoveValue("Remove unavailable post processor", value3, config.postProcessors, delegate(string[] values)
				{
					config.postProcessors = values;
				}, properties);
			}
			array = available;
			foreach (string value4 in array)
			{
				Helper.AddValue("Add available data provider", value4, config.dataProviders, delegate(string[] values)
				{
					config.dataProviders = values;
				}, properties);
			}
			array = available2;
			foreach (string value5 in array)
			{
				Helper.AddValue("Add available code generator", value5, config.codeGenerators, delegate(string[] values)
				{
					config.codeGenerators = values;
				}, properties);
			}
			array = available3;
			foreach (string value6 in array)
			{
				Helper.AddValue("Add available post processor", value6, config.postProcessors, delegate(string[] values)
				{
					config.postProcessors = values;
				}, properties);
			}
		}
	}
}
