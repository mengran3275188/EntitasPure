using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ShouldGenerateComponentComponentDataExtension
	{
		public const string COMPONENT_GENERATE_COMPONENT = "component_generateComponent";

		public const string COMPONENT_OBJECT_TYPE = "component_objectType";

		public static bool ShouldGenerateComponent(this ComponentData data)
		{
			return (bool)((Dictionary<string, object>)data)["component_generateComponent"];
		}

		public static void ShouldGenerateComponent(this ComponentData data, bool generate)
		{
			((Dictionary<string, object>)data)["component_generateComponent"] = generate;
		}

		public static string GetObjectType(this ComponentData data)
		{
			return (string)((Dictionary<string, object>)data)["component_objectType"];
		}

		public static void SetObjectType(this ComponentData data, string type)
		{
			((Dictionary<string, object>)data)["component_objectType"] = type;
		}
	}
}
