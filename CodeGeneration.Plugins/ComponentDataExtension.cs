using DesperateDevs.Utils;

namespace Entitas.CodeGeneration.Plugins
{
	public static class ComponentDataExtension
	{
		public static string ToComponentName(this string fullTypeName, bool ignoreNamespaces)
		{
			if (!ignoreNamespaces)
			{
				return fullTypeName.RemoveDots().RemoveComponentSuffix();
			}
			return fullTypeName.ShortTypeName().RemoveComponentSuffix();
		}
	}
}
