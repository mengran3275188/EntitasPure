using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public class IgnoreNamespacesConfig : AbstractConfigurableConfig
	{
		private const string IGNORE_NAMESPACES_KEY = "Entitas.CodeGeneration.Plugins.IgnoreNamespaces";

		public override Dictionary<string, string> defaultProperties
		{
			get
			{
				return new Dictionary<string, string>
				{
					{
						"Entitas.CodeGeneration.Plugins.IgnoreNamespaces",
						"false"
					}
				};
			}
		}

		public bool ignoreNamespaces
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.Plugins.IgnoreNamespaces"] == "true";
			}
		}
	}
}
