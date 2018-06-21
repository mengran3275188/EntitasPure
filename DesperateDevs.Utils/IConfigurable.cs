using System.Collections.Generic;

namespace DesperateDevs.Utils
{
	public interface IConfigurable
	{
		Dictionary<string, string> defaultProperties
		{
			get;
		}

		void Configure(Properties properties);
	}
}
