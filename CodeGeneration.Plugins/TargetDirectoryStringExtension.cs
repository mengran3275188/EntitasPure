using System;

namespace Entitas.CodeGeneration.Plugins
{
	public static class TargetDirectoryStringExtension
	{
		public static string ToSafeDirectory(this string directory)
		{
			if (!string.IsNullOrEmpty(directory) && !(directory == "."))
			{
				if (directory.EndsWith("/", StringComparison.Ordinal))
				{
					directory = directory.Substring(0, directory.Length - 1);
				}
				if (!directory.EndsWith("/EntitasGenerated", StringComparison.Ordinal))
				{
					directory += "/EntitasGenerated";
				}
				return directory;
			}
			return "EntitasGenerated";
		}
	}
}
