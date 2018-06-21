using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public class AssembliesConfig : AbstractConfigurableConfig
	{
		private const string ASSEMBLIES_KEY = "Entitas.CodeGeneration.Plugins.Assemblies";

		public override Dictionary<string, string> defaultProperties
		{
			get
			{
				return new Dictionary<string, string>
				{
					{
						"Entitas.CodeGeneration.Plugins.Assemblies",
						"Library/ScriptAssemblies/Assembly-CSharp.dll"
					}
				};
			}
		}

		public string[] assemblies
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.Plugins.Assemblies"].ArrayFromCSV();
			}
		}
	}
}
