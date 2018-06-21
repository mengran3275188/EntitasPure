using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ComponentTypeComponentDataExtension
	{
		public const string COMPONENT_FULL_TYPE_NAME = "component_fullTypeName";

		public static string GetFullTypeName(this ComponentData data)
		{
			return (string)((Dictionary<string, object>)data)["component_fullTypeName"];
		}

		public static void SetFullTypeName(this ComponentData data, string fullTypeName)
		{
			((Dictionary<string, object>)data)["component_fullTypeName"] = fullTypeName;
		}
	}
}
