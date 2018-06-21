using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class IsUniqueComponentDataExtension
	{
		public const string COMPONENT_IS_UNIQUE = "component_isUnique";

		public static bool IsUnique(this ComponentData data)
		{
			return (bool)((Dictionary<string, object>)data)["component_isUnique"];
		}

		public static void IsUnique(this ComponentData data, bool isUnique)
		{
			((Dictionary<string, object>)data)["component_isUnique"] = isUnique;
		}
	}
}
