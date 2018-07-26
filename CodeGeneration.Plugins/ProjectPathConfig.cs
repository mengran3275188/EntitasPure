using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public class ProjectPathConfig : AbstractConfigurableConfig
	{
		private const string PROJECT_PATH_KEY = "Entitas.CodeGeneration.Plugins.ProjectPath";

		public override Dictionary<string, string> defaultProperties
		{
			get
			{
				return new Dictionary<string, string>
				{
					{
						"Entitas.CodeGeneration.Plugins.ProjectPath",
						"Assembly-CSharp.csproj"
					}
				};
			}
		}

		public string projectPath
		{
			get
			{
				return _preferences["Entitas.CodeGeneration.Plugins.ProjectPath"];
			}
		}
	}
}
