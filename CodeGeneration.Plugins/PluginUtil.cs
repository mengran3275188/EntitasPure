using Entitas.CodeGeneration.CodeGenerator;
using DesperateDevs.Utils;
using System;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class PluginUtil
	{
		private static Dictionary<string, AssemblyResolver> _resolvers = new Dictionary<string, AssemblyResolver>();

		public static AssemblyResolver GetAssembliesResolver(string[] assemblies, string[] basePaths)
		{
			string key = assemblies.ToCSV();
			if (!PluginUtil._resolvers.ContainsKey(key))
			{
				AssemblyResolver assemblyResolver = new AssemblyResolver(false, basePaths);
				foreach (string path in assemblies)
				{
					assemblyResolver.Load(path);
				}
				PluginUtil._resolvers.Add(key, assemblyResolver);
			}
			return PluginUtil._resolvers[key];
		}
	}
}
