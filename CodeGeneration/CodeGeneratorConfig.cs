using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesperateDevs.Utils;

namespace Entitas.CodeGeneration
{
	public class CodeGeneratorConfig : AbstractConfigurableConfig
	{
		private const string SEARCH_PATHS_KEY = "Entitas.CodeGeneration.CodeGenerator.SearchPaths";

		private const string PLUGINS_PATHS_KEY = "Entitas.CodeGeneration.CodeGenerator.Plugins";

		private const string DATA_PROVIDERS_KEY = "Entitas.CodeGeneration.CodeGenerator.DataProviders";

		private const string CODE_GENERATORS_KEY = "Entitas.CodeGeneration.CodeGenerator.CodeGenerators";

		private const string POST_PROCESSORS_KEY = "Entitas.CodeGeneration.CodeGenerator.PostProcessors";

        private const string PRE_PROCESSORS_KEY = "Entitas.CodeGeneration.CodeGenerator.PreProcessors";

        private const string PORT_KEY = "Jenny.Server.Port";

        private const string HOST_KEY = "Jenny.Client.Host";

        public override Dictionary<string, string> defaultProperties
        {
            get
            {
                return new Dictionary<string, string>
            {
                {
                    SEARCH_PATHS_KEY,
                    string.Empty
                },
                {
                    PLUGINS_PATHS_KEY,
                    string.Empty
                },
                {
                    PRE_PROCESSORS_KEY,
                    string.Empty
                },
                {
                    DATA_PROVIDERS_KEY,
                    string.Empty
                },
                {
                    CODE_GENERATORS_KEY,
                    string.Empty
                },
                {
                    POST_PROCESSORS_KEY,
                    string.Empty
                },
                {
                    "Jenny.Server.Port",
                    "3333"
                },
                {
                    "Jenny.Client.Host",
                    "localhost"
                }
            };
            }
        }

        public string[] searchPaths
        {
            get
            {
                return base._preferences[SEARCH_PATHS_KEY].ArrayFromCSV();
            }
            set
            {
                base._preferences[SEARCH_PATHS_KEY] = value.ToCSV();
            }
        }

        public string[] plugins
        {
            get
            {
                return base._preferences[PLUGINS_PATHS_KEY].ArrayFromCSV();
            }
            set
            {
                base._preferences[PLUGINS_PATHS_KEY] = value.ToCSV();
            }
        }

        public string[] preProcessors
        {
            get
            {
                return base._preferences[PRE_PROCESSORS_KEY].ArrayFromCSV();
            }
            set
            {
                base._preferences[PRE_PROCESSORS_KEY] = value.ToCSV();
            }
        }

        public string[] dataProviders
        {
            get
            {
                return base._preferences[DATA_PROVIDERS_KEY].ArrayFromCSV();
            }
            set
            {
                base._preferences[DATA_PROVIDERS_KEY] = value.ToCSV();
            }
        }

        public string[] codeGenerators
        {
            get
            {
                return base._preferences[CODE_GENERATORS_KEY].ArrayFromCSV();
            }
            set
            {
                base._preferences[CODE_GENERATORS_KEY] = value.ToCSV();
            }
        }

        public string[] postProcessors
        {
            get
            {
                return base._preferences[POST_PROCESSORS_KEY].ArrayFromCSV();
            }
            set
            {
                base._preferences[POST_PROCESSORS_KEY] = value.ToCSV();
            }
        }

        public int port
        {
            get
            {
                return int.Parse(base._preferences["Jenny.Server.Port"]);
            }
            set
            {
                base._preferences["Jenny.Server.Port"] = value.ToString();
            }
        }

        public string host
        {
            get
            {
                return base._preferences["Jenny.Client.Host"];
            }
            set
            {
                base._preferences["Jenny.Client.Host"] = value;
            }
        }
    }

}
