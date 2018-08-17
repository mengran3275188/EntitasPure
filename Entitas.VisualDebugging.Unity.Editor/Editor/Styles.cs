using UnityEngine;

namespace DesperateDevs.Unity.Editor
{
	public static class Styles
	{
		private static GUIStyle _sectionHeader;

		private static GUIStyle _sectionContent;

		public static GUIStyle sectionHeader
		{
			get
			{
				if (_sectionHeader == null)
				{
					_sectionHeader = new GUIStyle("OL Title");
				}
				return _sectionHeader;
			}
		}

		public static GUIStyle sectionContent
		{
			get
			{
				if (_sectionContent == null)
				{
					_sectionContent = new GUIStyle("OL Box");
					_sectionContent.stretchHeight = false;
				}
				return _sectionContent;
			}
		}
	}
}
