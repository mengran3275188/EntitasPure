using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ContextDataExtension
	{
		public const string CONTEXT_NAME = "context_name";

		public static string GetContextName(this ContextData data)
		{
			return (string)((Dictionary<string, object>)data)["context_name"];
		}

		public static void SetContextName(this ContextData data, string contextName)
		{
			((Dictionary<string, object>)data)["context_name"] = contextName;
		}
	}
}
