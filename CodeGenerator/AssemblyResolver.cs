using DesperateDevs.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Entitas.CodeGeneration.CodeGenerator
{
	//public class AssemblyResolver
	//{
	//	private static Logger _logger = fabl.GetLogger(typeof(AssemblyResolver).Name);

	//	private readonly AppDomain _appDomain;

	//	private string[] _basePaths;

	//	private List<Assembly> _assemblies;

	//	public AssemblyResolver(AppDomain appDomain, string[] basePaths)
	//	{
	//		this._appDomain = appDomain;
	//		this._appDomain.AssemblyResolve += this.onAssemblyResolve;
	//		this._basePaths = basePaths;
	//		this._assemblies = new List<Assembly>();
	//	}

	//	public void Load(string path)
	//	{
	//		AssemblyResolver._logger.Debug("AppDomain load: " + path);
	//		Assembly item = this._appDomain.Load(path);
	//		this._assemblies.Add(item);
	//	}

	//	private Assembly onAssemblyResolve(object sender, ResolveEventArgs args)
	//	{
	//		Assembly result = null;
	//		try
	//		{
	//			AssemblyResolver._logger.Debug("  - Loading: " + args.Name);
	//			result = Assembly.LoadFrom(args.Name);
	//			return result;
	//		}
	//		catch (Exception)
	//		{
	//			string text = new AssemblyName(args.Name).Name;
	//			if (!text.EndsWith(".dll", StringComparison.Ordinal))
	//			{
	//				text += ".dll";
	//			}
	//			string text2 = this.resolvePath(text);
	//			if (text2 != null)
	//			{
	//				return Assembly.LoadFrom(text2);
	//			}
	//			return result;
	//		}
	//	}

	//	private string resolvePath(string assemblyName)
	//	{
	//		string[] basePaths = this._basePaths;
	//		for (int i = 0; i < basePaths.Length; i++)
	//		{
	//			string text = basePaths[i] + Path.DirectorySeparatorChar.ToString() + assemblyName;
	//			if (File.Exists(text))
	//			{
	//				AssemblyResolver._logger.Debug("    - Resolved: " + text);
	//				return text;
	//			}
	//		}
	//		AssemblyResolver._logger.Warn("    - Could not resolve: " + assemblyName);
	//		return null;
	//	}

	//	public Type[] GetTypes()
	//	{
	//		List<Type> list = new List<Type>();
	//		foreach (Assembly assembly in this._assemblies)
	//		{
	//			try
	//			{
	//				list.AddRange(assembly.GetTypes());
	//			}
	//			catch (ReflectionTypeLoadException ex)
	//			{
	//				list.AddRange(from type in ex.Types
	//				where type != null
	//				select type);
	//			}
	//		}
	//		return list.ToArray();
	//	}
	//}
}
