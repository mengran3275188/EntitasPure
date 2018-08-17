using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DesperateDevs.Unity.Editor
{
	public class ScriptingDefineSymbols
	{
		private readonly Dictionary<BuildTargetGroup, string> _buildTargetToDefSymbol;

		public Dictionary<BuildTargetGroup, string> buildTargetToDefSymbol => _buildTargetToDefSymbol;

		public ScriptingDefineSymbols()
		{
			_buildTargetToDefSymbol = (from BuildTargetGroup buildTargetGroup in Enum.GetValues(typeof(BuildTargetGroup))
			where buildTargetGroup != BuildTargetGroup.Unknown
			where !isBuildTargetObsolete(buildTargetGroup)
			select buildTargetGroup).Distinct().ToDictionary((BuildTargetGroup buildTargetGroup) => buildTargetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup);
		}

		public void AddDefineSymbol(string defineSymbol)
		{
			foreach (KeyValuePair<BuildTargetGroup, string> item in _buildTargetToDefSymbol)
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup(item.Key, item.Value.Replace(defineSymbol, string.Empty) + "," + defineSymbol);
			}
		}

		public void RemoveDefineSymbol(string defineSymbol)
		{
			foreach (KeyValuePair<BuildTargetGroup, string> item in _buildTargetToDefSymbol)
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup(item.Key, item.Value.Replace(defineSymbol, string.Empty));
			}
		}

		private bool isBuildTargetObsolete(BuildTargetGroup buildTargetGroup)
		{
			return Attribute.IsDefined(buildTargetGroup.GetType().GetField(buildTargetGroup.ToString()), typeof(ObsoleteAttribute));
		}
	}
}
