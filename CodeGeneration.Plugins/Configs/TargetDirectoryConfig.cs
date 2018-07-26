using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public class TargetDirectoryConfig : AbstractConfigurableConfig
	{
		private const string TARGET_DIRECTORY_KEY = "Entitas.CodeGeneration.Plugins.TargetDirectory";

		public override Dictionary<string, string> defaultProperties
		{
			get
			{
				return new Dictionary<string, string>
				{
					{
						"Entitas.CodeGeneration.Plugins.TargetDirectory",
						"Assets/Generated"
					}
				};
			}
		}

		public string targetDirectory
		{
			get
			{
				return _preferences["Entitas.CodeGeneration.Plugins.TargetDirectory"].ToSafeDirectory();
			}
		}
	}
}
