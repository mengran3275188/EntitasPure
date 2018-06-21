using DesperateDevs.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextDataProvider : ICodeGeneratorDataProvider, ICodeGeneratorInterface, IConfigurable
	{
		private readonly ContextNamesConfig _contextNamesConfig = new ContextNamesConfig();

		public string name
		{
			get
			{
				return "Context";
			}
		}

		public int priority
		{
			get
			{
				return 0;
			}
		}

		public bool isEnabledByDefault
		{
			get
			{
				return true;
			}
		}

		public bool runInDryMode
		{
			get
			{
				return true;
			}
		}

		public Dictionary<string, string> defaultProperties
		{
			get
			{
				return this._contextNamesConfig.defaultProperties;
			}
		}

		public void Configure(Properties properties)
		{
			this._contextNamesConfig.Configure(properties);
		}

		public CodeGeneratorData[] GetData()
		{
			return this._contextNamesConfig.contextNames.Select(delegate(string contextName)
			{
				ContextData contextData = new ContextData();
				contextData.SetContextName(contextName);
				return contextData;
			}).ToArray();
		}
	}
}
