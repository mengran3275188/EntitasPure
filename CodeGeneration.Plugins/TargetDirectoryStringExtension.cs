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
				if (!directory.EndsWith("/Generated", StringComparison.Ordinal))
				{
					directory += "/Generated";
				}
				return directory;
			}
			return "Generated";
		}
	}
}
