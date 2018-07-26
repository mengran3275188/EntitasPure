using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.CodeGenerator
{
	public class CodeGeneratorConfig : AbstractConfigurableConfig
	{
		private const string SEARCH_PATHS_KEY = "Entitas.CodeGeneration.CodeGenerator.SearchPaths";

		private const string PLUGINS_PATHS_KEY = "Entitas.CodeGeneration.CodeGenerator.Plugins";

		private const string DATA_PROVIDERS_KEY = "Entitas.CodeGeneration.CodeGenerator.DataProviders";

		private const string CODE_GENERATORS_KEY = "Entitas.CodeGeneration.CodeGenerator.CodeGenerators";

		private const string POST_PROCESSORS_KEY = "Entitas.CodeGeneration.CodeGenerator.PostProcessors";

		public override Dictionary<string, string> defaultProperties
		{
			get
			{
				return new Dictionary<string, string>
				{
					{
						"Entitas.CodeGeneration.CodeGenerator.SearchPaths",
						"Assets/Libraries/Entitas, Assets/Libraries/Entitas/Editor, /Applications/Unity/Unity.app/Contents/Managed, /Applications/Unity/Unity.app/Contents/Mono/lib/mono/unity, /Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/GUISystem"
					},
					{
						"Entitas.CodeGeneration.CodeGenerator.Plugins",
						"Entitas.CodeGeneration.Plugins, Entitas.VisualDebugging.CodeGeneration.Plugins"
					},
					{
						"Entitas.CodeGeneration.CodeGenerator.DataProviders",
						string.Empty
					},
					{
						"Entitas.CodeGeneration.CodeGenerator.CodeGenerators",
						string.Empty
					},
					{
						"Entitas.CodeGeneration.CodeGenerator.PostProcessors",
						string.Empty
					}
				};
			}
		}

		public string[] searchPaths
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.CodeGenerator.SearchPaths"].ArrayFromCSV();
			}
			set
			{
				base.properties["Entitas.CodeGeneration.CodeGenerator.SearchPaths"] = value.ToCSV();
			}
		}

		public string[] plugins
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.CodeGenerator.Plugins"].ArrayFromCSV();
			}
			set
			{
				base.properties["Entitas.CodeGeneration.CodeGenerator.Plugins"] = value.ToCSV();
			}
		}

		public string[] dataProviders
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.CodeGenerator.DataProviders"].ArrayFromCSV();
			}
			set
			{
				base.properties["Entitas.CodeGeneration.CodeGenerator.DataProviders"] = value.ToCSV();
			}
		}

		public string[] codeGenerators
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.CodeGenerator.CodeGenerators"].ArrayFromCSV();
			}
			set
			{
				base.properties["Entitas.CodeGeneration.CodeGenerator.CodeGenerators"] = value.ToCSV();
			}
		}

		public string[] postProcessors
		{
			get
			{
				return base.properties["Entitas.CodeGeneration.CodeGenerator.PostProcessors"].ArrayFromCSV();
			}
			set
			{
				base.properties["Entitas.CodeGeneration.CodeGenerator.PostProcessors"] = value.ToCSV();
			}
		}
	}
}
