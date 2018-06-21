using DesperateDevs.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DesperateDevs.Utils
{
	public class AssemblyResolver
	{
		private readonly Logger _logger = fabl.GetLogger(typeof(AssemblyResolver).Name);

		private readonly bool _reflectionOnly;

		private readonly string[] _basePaths;

		private readonly HashSet<Assembly> _assemblies;

		private readonly AppDomain _appDomain;

		public Assembly[] assemblies
		{
			get
			{
				return this._assemblies.ToArray();
			}
		}

		public AssemblyResolver(bool reflectionOnly, params string[] basePaths)
		{
			this._reflectionOnly = reflectionOnly;
			this._basePaths = basePaths;
			this._assemblies = new HashSet<Assembly>();
			this._appDomain = AppDomain.CurrentDomain;
			if (reflectionOnly)
			{
				this._appDomain.ReflectionOnlyAssemblyResolve += this.onReflectionOnlyAssemblyResolve;
			}
			else
			{
				this._appDomain.AssemblyResolve += this.onAssemblyResolve;
			}
		}

		public void Load(string path)
		{
			if (this._reflectionOnly)
			{
				this._logger.Debug(this._appDomain + " reflect: " + path);
				this.resolveAndLoad(path, Assembly.ReflectionOnlyLoadFrom, false);
			}
			else
			{
				this._logger.Debug(this._appDomain + " load: " + path);
				this.resolveAndLoad(path, Assembly.LoadFrom, false);
			}
		}

		public void Close()
		{
			if (this._reflectionOnly)
			{
				this._appDomain.ReflectionOnlyAssemblyResolve -= this.onReflectionOnlyAssemblyResolve;
			}
			else
			{
				this._appDomain.AssemblyResolve -= this.onAssemblyResolve;
			}
		}

		private Assembly resolveAndLoad(string name, Func<string, Assembly> loadMethod, bool isDependency)
		{
			Assembly assembly = null;
			try
			{
				if (isDependency)
				{
					this._logger.Debug("  ➜ Loading Dependency: " + name);
				}
				else
				{
					this._logger.Debug("  ➜ Loading: " + name);
				}
				assembly = loadMethod(name);
				this.addAssembly(assembly);
				return assembly;
			}
			catch (Exception)
			{
				string text = this.resolvePath(name);
				if (text != null)
				{
					try
					{
						assembly = loadMethod(text);
						this.addAssembly(assembly);
						return assembly;
					}
					catch (BadImageFormatException)
					{
						return assembly;
					}
				}
				return assembly;
			}
		}

		private Assembly onAssemblyResolve(object sender, ResolveEventArgs args)
		{
			return this.resolveAndLoad(args.Name, Assembly.LoadFrom, true);
		}

		private Assembly onReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
		{
			return this.resolveAndLoad(args.Name, Assembly.ReflectionOnlyLoadFrom, true);
		}

		private void addAssembly(Assembly assembly)
		{
			this._assemblies.Add(assembly);
		}

		private string resolvePath(string name)
		{
			try
			{
				string text = new AssemblyName(name).Name;
				if (!text.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !text.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
				{
					text += ".dll";
				}
				string[] basePaths = this._basePaths;
				for (int i = 0; i < basePaths.Length; i++)
				{
					string text2 = basePaths[i] + Path.DirectorySeparatorChar.ToString() + text;
					if (File.Exists(text2))
					{
						this._logger.Debug("    ➜ Resolved: " + text2);
						return text2;
					}
				}
			}
			catch (FileLoadException)
			{
				this._logger.Debug("    × Could not resolve: " + name);
			}
			return null;
		}

		public Type[] GetTypes()
		{
			return this._assemblies.ToArray().GetAllTypes();
		}

		public static AssemblyResolver LoadAssemblies(bool allDirectories, params string[] basePaths)
		{
			AssemblyResolver assemblyResolver = new AssemblyResolver(false, basePaths);
			string[] assemblyFiles = AssemblyResolver.getAssemblyFiles(allDirectories, basePaths);
			foreach (string path in assemblyFiles)
			{
				assemblyResolver.Load(path);
			}
			return assemblyResolver;
		}

		public static Assembly[] GetAssembliesContainingType<T>(bool allDirectories, params string[] basePaths)
		{
			AssemblyResolver assemblyResolver = new AssemblyResolver(true, basePaths);
			string[] assemblyFiles = AssemblyResolver.getAssemblyFiles(allDirectories, basePaths);
			foreach (string path in assemblyFiles)
			{
				assemblyResolver.Load(path);
			}
			string interfaceName = typeof(T).FullName;
			Assembly[] result = (from type in assemblyResolver.GetTypes()
			where type.GetInterface(interfaceName) != null
			select type.Assembly).Distinct().ToArray();
			assemblyResolver.Close();
			return result;
		}

		public static AssemblyResolver LoadAssembliesContainingType<T>(bool allDirectories, params string[] basePaths)
		{
			Assembly[] assembliesContainingType = AssemblyResolver.GetAssembliesContainingType<T>(allDirectories, basePaths);
			AssemblyResolver assemblyResolver = new AssemblyResolver(false, basePaths);
			Assembly[] array = assembliesContainingType;
			foreach (Assembly assembly in array)
			{
				assemblyResolver.Load(assembly.CodeBase);
			}
			return assemblyResolver;
		}

		private static string[] getAssemblyFiles(bool allDirectories, params string[] basePaths)
		{
			string[] obj = new string[2]
			{
				"*.dll",
				"*.exe"
			};
			List<string> list = new List<string>();
			string[] array = obj;
			foreach (string pattern in array)
			{
				list.AddRange(basePaths.SelectMany((string s) => Directory.GetFiles(s, pattern, (SearchOption)(allDirectories ? 1 : 0))));
			}
			return list.ToArray();
		}
	}
}
