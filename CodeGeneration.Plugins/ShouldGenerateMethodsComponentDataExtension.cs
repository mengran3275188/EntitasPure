using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ShouldGenerateMethodsComponentDataExtension
	{
		public const string COMPONENT_GENERATE_METHODS = "component_generateMethods";

		public static bool ShouldGenerateMethods(this ComponentData data)
		{
			return (bool)((Dictionary<string, object>)data)["component_generateMethods"];
		}

		public static void ShouldGenerateMethods(this ComponentData data, bool generate)
		{
			((Dictionary<string, object>)data)["component_generateMethods"] = generate;
		}
	}
}
