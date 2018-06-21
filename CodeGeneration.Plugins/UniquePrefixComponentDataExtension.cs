using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class UniquePrefixComponentDataExtension
	{
		public const string COMPONENT_UNIQUE_PREFIX = "component_uniquePrefix";

		public static string GetUniqueComponentPrefix(this ComponentData data)
		{
			return (string)((Dictionary<string, object>)data)["component_uniquePrefix"];
		}

		public static void SetUniqueComponentPrefix(this ComponentData data, string prefix)
		{
			((Dictionary<string, object>)data)["component_uniquePrefix"] = prefix;
		}
	}
}
