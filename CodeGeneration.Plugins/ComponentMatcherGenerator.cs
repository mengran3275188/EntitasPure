using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentMatcherGenerator : ICodeGenerator, ICodeGeneratorInterface, IConfigurable
	{
		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		private const string STANDARD_COMPONENT_TEMPLATE = "public sealed partial class ${ContextName}Matcher {\n\n    static Entitas.IMatcher<${ContextName}Entity> _matcher${ComponentName};\n\n    public static Entitas.IMatcher<${ContextName}Entity> ${ComponentName} {\n        get {\n            if (_matcher${ComponentName} == null) {\n                var matcher = (Entitas.Matcher<${ContextName}Entity>)Entitas.Matcher<${ContextName}Entity>.AllOf(${Index});\n                matcher.componentNames = ${ComponentNames};\n                _matcher${ComponentName} = matcher;\n            }\n\n            return _matcher${ComponentName};\n        }\n    }\n}\n";

		public string name
		{
			get
			{
				return "Component (Matcher API)";
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
			return (from d in data.OfType<ComponentData>()
			where d.ShouldGenerateIndex()
			select d).SelectMany((ComponentData d) => this.generateMatcher(d)).ToArray();
		}

		private CodeGenFile[] generateMatcher(ComponentData data)
		{
			return (from context in data.GetContextNames()
			select this.generateMatcher(context, data)).ToArray();
		}

		private CodeGenFile generateMatcher(string contextName, ComponentData data)
		{
			string text = data.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces);
			string newValue = contextName + "ComponentsLookup." + text;
			string newValue2 = contextName + "ComponentsLookup.componentNames";
			string fileContent = "public sealed partial class ${ContextName}Matcher {\n\n    static Entitas.IMatcher<${ContextName}Entity> _matcher${ComponentName};\n\n    public static Entitas.IMatcher<${ContextName}Entity> ${ComponentName} {\n        get {\n            if (_matcher${ComponentName} == null) {\n                var matcher = (Entitas.Matcher<${ContextName}Entity>)Entitas.Matcher<${ContextName}Entity>.AllOf(${Index});\n                matcher.componentNames = ${ComponentNames};\n                _matcher${ComponentName} = matcher;\n            }\n\n            return _matcher${ComponentName};\n        }\n    }\n}\n".Replace("${ContextName}", contextName).Replace("${ComponentName}", text).Replace("${Index}", newValue)
				.Replace("${ComponentNames}", newValue2);
			//string[] obj = new string[7]
			//{
			//	contextName,
			//	null,
			//	null,
			//	null,
			//	null,
			//	null,
			//	null
			//};
			//char directorySeparatorChar = Path.DirectorySeparatorChar;
			//obj[1] = directorySeparatorChar.ToString();
			//obj[2] = "Components";
			//directorySeparatorChar = Path.DirectorySeparatorChar;
			//obj[3] = directorySeparatorChar.ToString();
			//obj[4] = contextName;
			//obj[5] = text.AddComponentSuffix();
			//obj[6] = ".cs";
            char directorySeparatorChar = Path.DirectorySeparatorChar;
            string[] obj = new string[4]
            {
                contextName,
                directorySeparatorChar.ToString(),
                contextName,
                "Components.cs",
            };
			return new CodeGenFile(string.Concat(obj), fileContent, base.GetType().FullName);
		}
	}
}
