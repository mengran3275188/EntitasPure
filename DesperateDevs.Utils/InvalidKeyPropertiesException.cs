using System;

namespace DesperateDevs.Utils
{
	public class InvalidKeyPropertiesException : Exception
	{
		public readonly string key;

		public InvalidKeyPropertiesException(string key)
			: base("Invalid key: " + key)
		{
			this.key = key;
		}
	}
}
