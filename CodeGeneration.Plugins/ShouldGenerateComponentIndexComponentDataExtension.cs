using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ShouldGenerateComponentIndexComponentDataExtension
	{
		public const string COMPONENT_GENERATE_INDEX = "component_generateIndex";

		public static bool ShouldGenerateIndex(this ComponentData data)
		{
			return (bool)((Dictionary<string, object>)data)["component_generateIndex"];
		}

		public static void ShouldGenerateIndex(this ComponentData data, bool generate)
		{
			((Dictionary<string, object>)data)["component_generateIndex"] = generate;
		}
	}
}
