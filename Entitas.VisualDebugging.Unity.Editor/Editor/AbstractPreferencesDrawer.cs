using Entitas.CodeGeneration;

namespace DesperateDevs.Unity.Editor
{
	public abstract class AbstractPreferencesDrawer : IPreferencesDrawer
	{
		protected bool _drawContent = true;

		public abstract int priority
		{
			get;
		}

		public abstract string title
		{
			get;
		}

		public abstract void Initialize(Preferences preferences);

		public abstract void DrawHeader(Preferences preferences);

		public virtual void DrawContent(Preferences preferences)
		{
			_drawContent = EditorLayout.DrawSectionHeaderToggle(title, _drawContent);
			if (_drawContent)
			{
				EditorLayout.BeginSectionContent();
				drawContent(preferences);
				EditorLayout.EndSectionContent();
			}
		}

		protected abstract void drawContent(Preferences preferences);
	}
}
