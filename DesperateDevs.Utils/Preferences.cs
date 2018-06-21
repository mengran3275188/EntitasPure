using System.IO;

namespace DesperateDevs.Utils
{
	public static class Preferences
	{
		public const string PATH = "Entitas.properties";

		public static bool HasProperties()
		{
			return File.Exists("Entitas.properties");
		}

		public static Properties LoadProperties()
		{
			return new Properties(File.ReadAllText("Entitas.properties"));
		}

		public static void SaveProperties(Properties properties)
		{
			File.WriteAllText("Entitas.properties", properties.ToString());
		}
	}
}
