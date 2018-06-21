using System.Collections.Generic;

namespace DesperateDevs.Utils
{
	public abstract class AbstractConfigurableConfig : IConfigurable
	{
		private Properties _properties;

		public abstract Dictionary<string, string> defaultProperties
		{
			get;
		}

		public Properties properties
		{
			get
			{
				return this._properties;
			}
		}

		public virtual void Configure(Properties properties)
		{
			this._properties = properties;
		}

		public override string ToString()
		{
			return this.properties.ToString();
		}
	}
}
