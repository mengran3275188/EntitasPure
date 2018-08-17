using Entitas.CodeGeneration;
using DesperateDevs.Utils;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DesperateDevs.Unity.Editor
{
	public class PreferencesWindow : EditorWindow
	{
		public const string PREFERENCES_KEY = "DesperateDevs.Unity.Editor.PreferencesWindow.Preferences.Path";

		public string preferencesName;

		private Preferences _preferences;

		private IPreferencesDrawer[] _preferencesDrawers;

		private Vector2 _scrollViewPosition;

		private Exception _configException;

		private void initialize()
		{
			try
			{
				string @string = EditorPrefs.GetString("DesperateDevs.Unity.Editor.PreferencesWindow.Preferences.Path", string.Empty);
				if (@string != string.Empty && File.Exists(@string))
				{
					Preferences.sharedInstance = new Preferences(@string, null);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				_preferences = Preferences.sharedInstance;
				EditorPrefs.SetString("DesperateDevs.Unity.Editor.PreferencesWindow.Preferences.Path", Path.GetFileName(Preferences.sharedInstance.propertiesPath));
				PreferencesConfig preferencesConfig = new PreferencesConfig(preferencesName);
				_preferences.properties.AddProperties(preferencesConfig.defaultProperties, false);
				preferencesConfig.Configure(_preferences);
				IPreferencesDrawer[] source = (from drawer in AppDomain.CurrentDomain.GetInstancesOf<IPreferencesDrawer>()
				orderby drawer.priority
				select drawer).ToArray();
				if (preferencesConfig.preferenceDrawers.Length == 0)
				{
					preferencesConfig.preferenceDrawers = (from drawer in source
					select drawer.GetType().FullName).ToArray();
				}
				string[] enabledPreferenceDrawers = preferencesConfig.preferenceDrawers;
				_preferencesDrawers = (from drawer in source
				where enabledPreferenceDrawers.Contains(drawer.GetType().FullName)
				select drawer).ToArray();
				IPreferencesDrawer[] preferencesDrawers = _preferencesDrawers;
				for (int i = 0; i < preferencesDrawers.Length; i++)
				{
					preferencesDrawers[i].Initialize(_preferences);
				}
				_preferences.Save(false);
			}
			catch (Exception configException)
			{
				_preferencesDrawers = new IPreferencesDrawer[0];
				_configException = configException;
			}
		}

		private void OnGUI()
		{
			if (_preferencesDrawers == null)
			{
				initialize();
			}
			drawHeader();
			_scrollViewPosition = EditorGUILayout.BeginScrollView(_scrollViewPosition);
			drawContent();
			EditorGUILayout.EndScrollView();
			if (GUI.changed)
			{
				_preferences.Save(false);
			}
		}

		private void drawHeader()
		{
			IPreferencesDrawer[] preferencesDrawers = _preferencesDrawers;
			foreach (IPreferencesDrawer preferencesDrawer in preferencesDrawers)
			{
				try
				{
					preferencesDrawer.DrawHeader(_preferences);
				}
				catch (Exception exception)
				{
					drawException(exception);
				}
			}
		}

		private void drawContent()
		{
			if (_configException == null)
			{
				for (int i = 0; i < _preferencesDrawers.Length; i++)
				{
					try
					{
						_preferencesDrawers[i].DrawContent(_preferences);
					}
					catch (Exception exception)
					{
						drawException(exception);
					}
					if (i < _preferencesDrawers.Length - 1)
					{
						EditorGUILayout.Space();
					}
				}
			}
			else
			{
				drawException(_configException);
			}
		}

		private static void drawException(Exception exception)
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.wordWrap = true;
			gUIStyle.normal.textColor = Color.red;
			if (Event.current.alt)
			{
				EditorGUILayout.LabelField(exception.ToString(), gUIStyle);
			}
			else
			{
				EditorGUILayout.LabelField(exception.Message, gUIStyle);
			}
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Please make sure the properties files are set up correctly.");
		}
	}
}
