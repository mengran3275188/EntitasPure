using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentsLookupGenerator : ICodeGenerator, ICodeGeneratorInterface, IConfigurable
	{
		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		public const string COMPONENTS_LOOKUP = "ComponentsLookup";

		private const string COMPONENTS_LOOKUP_TEMPLATE = "public static class ${Lookup} {\n\n${componentConstants}\n\n${totalComponentsConstant}\n\n    public static readonly string[] componentNames = {\n${componentNames}\n    };\n\n    public static readonly System.Type[] componentTypes = {\n${componentTypes}\n    };\n}\n";

		private const string COMPONENT_CONSTANTS_TEMPLATE = "    public const int ${ComponentName} = ${Index};";

		private const string TOTAL_COMPONENTS_CONSTANT_TEMPLATE = "    public const int TotalComponents = ${totalComponents};";

		private const string COMPONENT_NAMES_TEMPLATE = "        \"${ComponentName}\"";

		private const string COMPONENT_TYPES_TEMPLATE = "        typeof(${ComponentType})";

		public string name
		{
			get
			{
				return "Components Lookup";
			}
		}

		public int priority
		{
			get
			{
				return 0;
			}
		}

		public bool isEnabledByDefault
		{
			get
			{
				return true;
			}
		}

		public bool runInDryMode
		{
			get
			{
				return true;
			}
		}

		public Dictionary<string, string> defaultProperties
		{
			get
			{
				return this._ignoreNamespacesConfig.defaultProperties;
			}
		}

		public void Configure(Properties properties)
		{
			this._ignoreNamespacesConfig.Configure(properties);
		}

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			List<CodeGenFile> list = new List<CodeGenFile>();
			ContextData[] data2 = data.OfType<ContextData>().ToArray();
			list.AddRange(this.generateEmptyLookup(data2));
			ComponentData[] data3 = (from d in data.OfType<ComponentData>()
			where d.ShouldGenerateIndex()
			select d).ToArray();
			CodeGenFile[] array = this.generateLookup(data3);
			HashSet<string> fileNames = new HashSet<string>(from file in array
			select file.fileName);
			List<CodeGenFile> list2 = (from file in list
			where !fileNames.Contains(file.fileName)
			select file).ToList();
			list2.AddRange(array);
			return list2.ToArray();
		}

		private CodeGenFile[] generateEmptyLookup(ContextData[] data)
		{
			ComponentData[] emptyData = new ComponentData[0];
			return (from d in data
			select this.generateComponentsLookupClass(d.GetContextName(), emptyData)).ToArray();
		}

		private CodeGenFile[] generateLookup(ComponentData[] data)
		{
			Dictionary<string, List<ComponentData>> dictionary = data.Aggregate(new Dictionary<string, List<ComponentData>>(), delegate(Dictionary<string, List<ComponentData>> dict, ComponentData d)
			{
				string[] contextNames = d.GetContextNames();
				foreach (string key2 in contextNames)
				{
					if (!dict.ContainsKey(key2))
					{
						dict.Add(key2, new List<ComponentData>());
					}
					dict[key2].Add(d);
				}
				return dict;
			});
			string[] array = dictionary.Keys.ToArray();
			foreach (string key in array)
			{
				dictionary[key] = (from d in dictionary[key]
				orderby d.GetFullTypeName()
				select d).ToList();
			}
			return (from kv in dictionary
			select this.generateComponentsLookupClass(kv.Key, kv.Value.ToArray())).ToArray();
		}

		private CodeGenFile generateComponentsLookupClass(string contextName, ComponentData[] data)
		{
			string newValue = string.Join("\n", data.Select((ComponentData d, int index) => "    public const int ${ComponentName} = ${Index};".Replace("${ComponentName}", d.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces)).Replace("${Index}", index.ToString())).ToArray());
			string newValue2 = "    public const int TotalComponents = ${totalComponents};".Replace("${totalComponents}", data.Length.ToString());
			string newValue3 = string.Join(",\n", (from d in data
			select "        \"${ComponentName}\"".Replace("${ComponentName}", d.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces))).ToArray());
			string newValue4 = string.Join(",\n", (from d in data
			select "        typeof(${ComponentType})".Replace("${ComponentType}", d.GetFullTypeName())).ToArray());
			string fileContent = "public static class ${Lookup} {\n\n${componentConstants}\n\n${totalComponentsConstant}\n\n    public static readonly string[] componentNames = {\n${componentNames}\n    };\n\n    public static readonly System.Type[] componentTypes = {\n${componentTypes}\n    };\n}\n".Replace("${Lookup}", contextName + "ComponentsLookup").Replace("${componentConstants}", newValue).Replace("${totalComponentsConstant}", newValue2)
				.Replace("${componentNames}", newValue3)
				.Replace("${componentTypes}", newValue4);
			return new CodeGenFile(contextName + Path.DirectorySeparatorChar.ToString() + contextName + "ComponentsLookup.cs", fileContent, base.GetType().FullName);
		}
	}
}
