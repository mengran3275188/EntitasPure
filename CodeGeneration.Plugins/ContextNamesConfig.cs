using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextNamesConfig : AbstractConfigurableConfig
	{
		private const string CONTEXTS_KEY = "Entitas.CodeGeneration.Plugins.Contexts";

		public override Dictionary<string, string> defaultProperties
		{
			get
			{
				return new Dictionary<string, string>
				{
					{
						"Entitas.CodeGeneration.Plugins.Contexts",
						"Game, GameState, Input"
					}
				};
			}
		}

		public string[] contextNames
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.Plugins.Contexts"].ArrayFromCSV();
			}
		}
	}
}
