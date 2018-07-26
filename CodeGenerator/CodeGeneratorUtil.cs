using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Entitas.CodeGeneration.CodeGenerator
{
	public static class CodeGeneratorUtil
	{
        public static CodeGenerator CodeGeneratorFromPreferences(Preferences preferences)
        {
            ICodeGenerationPlugin[] instances = CodeGeneratorUtil.LoadFromPlugins(preferences);
            CodeGeneratorConfig codeGeneratorConfig = preferences.CreateAndConfigure<CodeGeneratorConfig>();
            IPreProcessor[] enabledInstancesOf = CodeGeneratorUtil.GetEnabledInstancesOf<IPreProcessor>(instances, codeGeneratorConfig.preProcessors);
            IDataProvider[] enabledInstancesOf2 = CodeGeneratorUtil.GetEnabledInstancesOf<IDataProvider>(instances, codeGeneratorConfig.dataProviders);
            ICodeGenerator[] enabledInstancesOf3 = CodeGeneratorUtil.GetEnabledInstancesOf<ICodeGenerator>(instances, codeGeneratorConfig.codeGenerators);
            IPostProcessor[] enabledInstancesOf4 = CodeGeneratorUtil.GetEnabledInstancesOf<IPostProcessor>(instances, codeGeneratorConfig.postProcessors);
            CodeGeneratorUtil.configure(enabledInstancesOf, preferences);
            CodeGeneratorUtil.configure(enabledInstancesOf2, preferences);
            CodeGeneratorUtil.configure(enabledInstancesOf3, preferences);
            CodeGeneratorUtil.configure(enabledInstancesOf4, preferences);
            bool trackHooks = true;
            if (preferences.HasKey("Jenny.TrackHooks"))
            {
                trackHooks = (preferences["Jenny.TrackHooks"] == "true");
            }
            return new CodeGenerator(enabledInstancesOf, enabledInstancesOf2, enabledInstancesOf3, enabledInstancesOf4, trackHooks);
        }

        private static void configure(ICodeGenerationPlugin[] plugins, Preferences preferences)
        {
            foreach (IConfigurable item in plugins.OfType<IConfigurable>())
            {
                item.Configure(preferences);
            }
        }

        public static ICodeGenerationPlugin[] LoadFromPlugins(Preferences preferences)
        {
            CodeGeneratorConfig codeGeneratorConfig = preferences.CreateAndConfigure<CodeGeneratorConfig>();
            AssemblyResolver assemblyResolver = new AssemblyResolver(false, codeGeneratorConfig.searchPaths);
            string[] plugins = codeGeneratorConfig.plugins;
            foreach (string path in plugins)
            {
                assemblyResolver.Load(path);
            }
            return assemblyResolver.GetTypes().GetInstancesOf<ICodeGenerationPlugin>();
        }

        public static T[] GetOrderedInstancesOf<T>(ICodeGenerationPlugin[] instances) where T : ICodeGenerationPlugin
        {
            return (from instance in instances.OfType<T>()
                    orderby instance.priority, instance.GetType().ToCompilableString()
                    select instance).ToArray();
        }

        public static string[] GetOrderedTypeNamesOf<T>(ICodeGenerationPlugin[] instances) where T : ICodeGenerationPlugin
        {
            return (from instance in (IEnumerable<T>)CodeGeneratorUtil.GetOrderedInstancesOf<T>(instances)
                    select instance.GetType().ToCompilableString()).ToArray();
        }

        public static T[] GetEnabledInstancesOf<T>(ICodeGenerationPlugin[] instances, string[] typeNames) where T : ICodeGenerationPlugin
        {
            return (from instance in (IEnumerable<T>)CodeGeneratorUtil.GetOrderedInstancesOf<T>(instances)
                    where Enumerable.Contains<string>((IEnumerable<string>)typeNames, instance.GetType().ToCompilableString())
                    select instance).ToArray();
        }

        public static string[] GetAvailableNamesOf<T>(ICodeGenerationPlugin[] instances, string[] typeNames) where T : ICodeGenerationPlugin
        {
            return (from typeName in CodeGeneratorUtil.GetOrderedTypeNamesOf<T>(instances)
                    where !Enumerable.Contains<string>((IEnumerable<string>)typeNames, typeName)
                    select typeName).ToArray();
        }

        public static string[] GetUnavailableNamesOf<T>(ICodeGenerationPlugin[] instances, string[] typeNames) where T : ICodeGenerationPlugin
        {
            string[] orderedTypeNames = CodeGeneratorUtil.GetOrderedTypeNamesOf<T>(instances);
            return (from typeName in typeNames
                    where !Enumerable.Contains<string>((IEnumerable<string>)orderedTypeNames, typeName)
                    select typeName).ToArray();
        }

        public static Dictionary<string, string> GetDefaultProperties(ICodeGenerationPlugin[] instances, CodeGeneratorConfig config)
        {
            return new Dictionary<string, string>().Merge((from instance in CodeGeneratorUtil.GetEnabledInstancesOf<IPreProcessor>(instances, config.preProcessors).OfType<IConfigurable>().Concat(CodeGeneratorUtil.GetEnabledInstancesOf<IDataProvider>(instances, config.dataProviders).OfType<IConfigurable>())
                .Concat(CodeGeneratorUtil.GetEnabledInstancesOf<ICodeGenerator>(instances, config.codeGenerators).OfType<IConfigurable>())
                .Concat(CodeGeneratorUtil.GetEnabledInstancesOf<IPostProcessor>(instances, config.postProcessors).OfType<IConfigurable>())
                                                           select instance.defaultProperties).ToArray());
        }

        public static void AutoImport(CodeGeneratorConfig config, params string[] searchPaths)
        {
            string[] source = (from assembly in (from type in AssemblyResolver.GetAssembliesContainingType<ICodeGenerationPlugin>(true, searchPaths).GetAllTypes().GetNonAbstractTypes<ICodeGenerationPlugin>()
                                                 select type.Assembly).Distinct()
                               select CodeGeneratorUtil.makeRelativePath(Directory.GetCurrentDirectory(), assembly.CodeBase)).ToArray();
            HashSet<string> currentFullPaths = new HashSet<string>(config.searchPaths.Select(Path.GetFullPath));
            IEnumerable<string> second = from path in source.Select(Path.GetDirectoryName)
                                         where !currentFullPaths.Contains(path)
                                         select path;
            config.searchPaths = config.searchPaths.Concat(second).Distinct().ToArray();
            config.plugins = source.Select(Path.GetFileNameWithoutExtension).Distinct().ToArray();
        }

        private static string makeRelativePath(string dir, string otherDir)
        {
            dir = CodeGeneratorUtil.createUri(dir);
            otherDir = CodeGeneratorUtil.createUri(otherDir);
            if (otherDir.StartsWith(dir))
            {
                otherDir = otherDir.Replace(dir, string.Empty);
                if (otherDir.StartsWith("/"))
                {
                    otherDir = otherDir.Substring(1);
                }
            }
            return otherDir;
        }

        private static string createUri(string path)
        {
            Uri uri = new Uri(path);
            return Uri.UnescapeDataString(uri.AbsolutePath + uri.Fragment);
        }
    }
}
