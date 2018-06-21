using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ContextsComponentDataExtension
	{
		public const string COMPONENT_CONTEXTS = "component_contexts";

		public static string[] GetContextNames(this ComponentData data)
		{
			return (string[])((Dictionary<string, object>)data)["component_contexts"];
		}

		public static void SetContextNames(this ComponentData data, string[] contextNames)
		{
			((Dictionary<string, object>)data)["component_contexts"] = contextNames;
		}
	}
}
