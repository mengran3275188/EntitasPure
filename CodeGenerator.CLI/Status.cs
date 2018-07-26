using DesperateDevs.Utils;
using DesperateDevs.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
    /*
	public class Status : AbstractCommand
	{
		public override string trigger
		{
			get
			{
				return "status";
			}
		}

		public override void Run(string[] args)
		{
			if (base.assertProperties())
			{
				Properties properties = base.loadProperties();
				CodeGeneratorConfig codeGeneratorConfig = new CodeGeneratorConfig();
				codeGeneratorConfig.Configure(properties);
				fabl.Debug(codeGeneratorConfig.ToString());
				Type[] array = null;
				Dictionary<string, string> dictionary = null;
				try
				{
					array = CodeGeneratorUtil.LoadTypesFromPlugins(properties);
					dictionary = CodeGeneratorUtil.GetConfigurables(CodeGeneratorUtil.GetUsed<ICodeGeneratorDataProvider>(array, codeGeneratorConfig.dataProviders), CodeGeneratorUtil.GetUsed<ICodeGenerator>(array, codeGeneratorConfig.codeGenerators), CodeGeneratorUtil.GetUsed<ICodeGenFilePostProcessor>(array, codeGeneratorConfig.postProcessors));
				}
				catch (Exception ex)
				{
					Status.printKeyStatus(null, codeGeneratorConfig.defaultProperties, properties);
					throw ex;
				}
				Status.printKeyStatus(dictionary, codeGeneratorConfig.defaultProperties, properties);
				Status.printConfigurableKeyStatus(dictionary, properties);
				Status.printPluginStatus(array, codeGeneratorConfig);
			}
		}

		private static void printKeyStatus(Dictionary<string, string> configurables, Dictionary<string, string> defaultProperties, Properties properties)
		{
			string[] requiredKeys = defaultProperties.Keys.ToArray();
			string[] array = defaultProperties.Keys.ToArray();
			if (configurables != null)
			{
				array = array.Concat(configurables.Keys).ToArray();
			}
			string[] unusedKeys = Helper.GetUnusedKeys(array, properties);
			foreach (string str in unusedKeys)
			{
				fabl.Info("Unused key: " + str);
			}
			unusedKeys = Helper.GetMissingKeys(requiredKeys, properties);
			foreach (string str2 in unusedKeys)
			{
				fabl.Warn("Missing key: " + str2);
			}
		}

		private static void printConfigurableKeyStatus(Dictionary<string, string> configurables, Properties properties)
		{
			foreach (KeyValuePair<string, string> missingConfigurable in CodeGeneratorUtil.GetMissingConfigurables(configurables, properties))
			{
				fabl.Warn("Missing key: " + missingConfigurable.Key);
			}
		}

		private static void printPluginStatus(Type[] types, CodeGeneratorConfig config)
		{
			string[] unavailable = CodeGeneratorUtil.GetUnavailable<ICodeGeneratorDataProvider>(types, config.dataProviders);
			string[] unavailable2 = CodeGeneratorUtil.GetUnavailable<ICodeGenerator>(types, config.codeGenerators);
			string[] unavailable3 = CodeGeneratorUtil.GetUnavailable<ICodeGenFilePostProcessor>(types, config.postProcessors);
			string[] available = CodeGeneratorUtil.GetAvailable<IDataProvider>(types, config.dataProviders);
			string[] available2 = CodeGeneratorUtil.GetAvailable<ICodeGenerator>(types, config.codeGenerators);
			string[] available3 = CodeGeneratorUtil.GetAvailable<IPostProcessor>(types, config.postProcessors);
			Status.printUnavailable(unavailable);
			Status.printUnavailable(unavailable2);
			Status.printUnavailable(unavailable3);
			Status.printAvailable(available);
			Status.printAvailable(available2);
			Status.printAvailable(available3);
		}

		private static void printUnavailable(string[] names)
		{
			foreach (string str in names)
			{
				fabl.Warn("Unavailable: " + str);
			}
		}

		private static void printAvailable(string[] names)
		{
			foreach (string str in names)
			{
				fabl.Info("Available: " + str);
			}
		}
	}
    */
}
