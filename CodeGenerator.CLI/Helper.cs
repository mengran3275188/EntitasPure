using DesperateDevs.Utils;
using DesperateDevs.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public static class Helper
	{
		public static string[] GetUnusedKeys(string[] requiredKeys, Properties properties)
		{
			return (from key in properties.keys
			where !requiredKeys.Contains(key)
			select key).ToArray();
		}

		public static string[] GetMissingKeys(string[] requiredKeys, Properties properties)
		{
			return (from key in requiredKeys
			where !properties.HasKey(key)
			select key).ToArray();
		}

		public static bool GetUserDecision(char accept = 'y', char cancel = 'n')
		{
			char keyChar;
			do
			{
				keyChar = Console.ReadKey(true).KeyChar;
			}
			while (keyChar != accept && keyChar != cancel);
			return keyChar == accept;
		}

		public static void AddKey(string question, string key, string value, Properties properties)
		{
			fabl.Info(question + ": '" + key + "' ? (y / n)");
			if (Helper.GetUserDecision('y', 'n'))
			{
				properties[key] = value;
				Preferences.SaveProperties(properties);
				fabl.Info("Added: " + key);
			}
		}

		public static void RemoveKey(string question, string key, Properties properties)
		{
			fabl.Warn(question + ": '" + key + "' ? (y / n)");
			if (Helper.GetUserDecision('y', 'n'))
			{
				properties.RemoveProperty(key);
				Preferences.SaveProperties(properties);
				fabl.Warn("Removed: " + key);
			}
		}

		public static void RemoveValue(string question, string value, string[] values, Action<string[]> updateAction, Properties properties)
		{
			fabl.Warn(question + ": '" + value + "' ? (y / n)");
			if (Helper.GetUserDecision('y', 'n'))
			{
				List<string> list = values.ToList();
				list.Remove(value);
				updateAction(list.ToArray());
				Preferences.SaveProperties(properties);
				fabl.Warn("Removed: " + value);
			}
		}

		public static void AddValue(string question, string value, string[] values, Action<string[]> updateAction, Properties properties)
		{
			fabl.Info(question + ": '" + value + "' ? (y / n)");
			if (Helper.GetUserDecision('y', 'n'))
			{
				List<string> list = values.ToList();
				list.Add(value);
				updateAction(CodeGeneratorUtil.GetOrderedNames(list.ToArray()));
				Preferences.SaveProperties(properties);
				fabl.Info("Added: " + value);
			}
		}
	}
}
