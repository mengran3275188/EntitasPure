using Entitas.CodeGeneration;
using DesperateDevs.Utils;
using System.Collections.Generic;

namespace DesperateDevs.Unity.Editor
{
	public class PreferencesConfig : AbstractConfigurableConfig
	{
		private const string PREFERENCES_KEY = "Preferences.";

		private readonly string _key;

		public override Dictionary<string, string> defaultProperties => new Dictionary<string, string>
		{
			{
				_key,
				string.Empty
			}
		};

		public string[] preferenceDrawers
		{
			get
			{
				return _preferences[_key].ArrayFromCSV();
			}
			set
			{
				_preferences[_key] = value.ToCSV();
			}
		}

		public PreferencesConfig(string name)
		{
			_key = "Preferences." + name;
		}
	}
}
