using Entitas.CodeGeneration.Attributes;
using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextsComponentDataProvider : IComponentDataProvider, IConfigurable
	{
		private readonly ContextNamesConfig _contextNamesConfig = new ContextNamesConfig();

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

		public void Provide(Type type, ComponentData data)
		{
			string[] contextNamesOrDefault = this.GetContextNamesOrDefault(type);
			data.SetContextNames(contextNamesOrDefault);
		}

		public string[] GetContextNames(Type type)
		{
			return (from attr in Attribute.GetCustomAttributes(type).OfType<ContextAttribute>()
			select attr.contextName).ToArray();
		}

		public string[] GetContextNamesOrDefault(Type type)
		{
			string[] array = this.GetContextNames(type);
			if (array.Length == 0)
			{
				array = new string[1]
				{
					this._contextNamesConfig.contextNames[0]
				};
			}
			return array;
		}
	}
}
