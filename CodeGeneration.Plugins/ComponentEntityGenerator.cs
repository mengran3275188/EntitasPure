using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentEntityGenerator : ICodeGenerator, ICodeGeneratorInterface, IConfigurable
	{
		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		private const string STANDARD_COMPONENT_TEMPLATE = "public partial class ${ContextName}Entity {\n\n    public ${ComponentType} ${componentName} { get { return (${ComponentType})GetComponent(${Index}); } }\n    public bool has${ComponentName} { get { return HasComponent(${Index}); } }\n\n    public void Add${ComponentName}(${memberArgs}) {\n        var index = ${Index};\n        var component = CreateComponent<${ComponentType}>(index);\n${memberAssignment}\n        AddComponent(index, component);\n    }\n\n    public void Replace${ComponentName}(${memberArgs}) {\n        var index = ${Index};\n        var component = CreateComponent<${ComponentType}>(index);\n${memberAssignment}\n        ReplaceComponent(index, component);\n    }\n\n    public void Remove${ComponentName}() {\n        RemoveComponent(${Index});\n    }\n}\n";

		private const string MEMBER_ARGS_TEMPLATE = "${MemberType} new${MemberName}";

		private const string MEMBER_ASSIGNMENT_TEMPLATE = "        component.${memberName} = new${MemberName};";

		private const string FLAG_COMPONENT_TEMPLATE = "public partial class ${ContextName}Entity {\n\n    static readonly ${ComponentType} ${componentName}Component = new ${ComponentType}();\n\n    public bool ${prefixedName} {\n        get { return HasComponent(${Index}); }\n        set {\n            if (value != ${prefixedName}) {\n                if (value) {\n                    AddComponent(${Index}, ${componentName}Component);\n                } else {\n                    RemoveComponent(${Index});\n                }\n            }\n        }\n    }\n}\n";

		public string name
		{
			get
			{
				return "Component (Entity API)";
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
			where d.ShouldGenerateMethods()
			select d).SelectMany((ComponentData d) => this.generateExtensions(d)).ToArray();
		}

		private CodeGenFile[] generateExtensions(ComponentData data)
		{
			return (from contextName in data.GetContextNames()
			select this.generateExtension(contextName, data)).ToArray();
		}

		private CodeGenFile generateExtension(string contextName, ComponentData data)
		{
			string text = data.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces);
			string newValue = contextName + "ComponentsLookup." + text;
			MemberData[] memberData = data.GetMemberData();
			string fileContent = ((memberData.Length == 0) ? "public partial class ${ContextName}Entity {\n\n    static readonly ${ComponentType} ${componentName}Component = new ${ComponentType}();\n\n    public bool ${prefixedName} {\n        get { return HasComponent(${Index}); }\n        set {\n            if (value != ${prefixedName}) {\n                if (value) {\n                    AddComponent(${Index}, ${componentName}Component);\n                } else {\n                    RemoveComponent(${Index});\n                }\n            }\n        }\n    }\n}\n" : "public partial class ${ContextName}Entity {\n\n    public ${ComponentType} ${componentName} { get { return (${ComponentType})GetComponent(${Index}); } }\n    public bool has${ComponentName} { get { return HasComponent(${Index}); } }\n\n    public void Add${ComponentName}(${memberArgs}) {\n        var index = ${Index};\n        var component = CreateComponent<${ComponentType}>(index);\n${memberAssignment}\n        AddComponent(index, component);\n    }\n\n    public void Replace${ComponentName}(${memberArgs}) {\n        var index = ${Index};\n        var component = CreateComponent<${ComponentType}>(index);\n${memberAssignment}\n        ReplaceComponent(index, component);\n    }\n\n    public void Remove${ComponentName}() {\n        RemoveComponent(${Index});\n    }\n}\n").Replace("${ContextName}", contextName).Replace("${ComponentType}", data.GetFullTypeName()).Replace("${ComponentName}", text)
				.Replace("${componentName}", text.LowercaseFirst())
				.Replace("${prefixedName}", data.GetUniqueComponentPrefix().LowercaseFirst() + text)
				.Replace("${Index}", newValue)
				.Replace("${memberArgs}", this.getMemberArgs(memberData))
				.Replace("${memberAssignment}", this.getMemberAssignment(memberData));
			string[] obj = new string[7]
			{
				contextName,
				null,
				null,
				null,
				null,
				null,
				null
			};
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			obj[1] = directorySeparatorChar.ToString();
			obj[2] = "Components";
			directorySeparatorChar = Path.DirectorySeparatorChar;
			obj[3] = directorySeparatorChar.ToString();
			obj[4] = contextName;
			obj[5] = text.AddComponentSuffix();
			obj[6] = ".cs";
			return new CodeGenFile(string.Concat(obj), fileContent, base.GetType().FullName);
		}

		private string getMemberArgs(MemberData[] memberData)
		{
			string[] value = (from info in memberData
			select "${MemberType} new${MemberName}".Replace("${MemberType}", info.type).Replace("${MemberName}", info.name.UppercaseFirst())).ToArray();
			return string.Join(", ", value);
		}

		private string getMemberAssignment(MemberData[] memberData)
		{
			string[] value = (from info in memberData
			select "        component.${memberName} = new${MemberName};".Replace("${MemberType}", info.type).Replace("${memberName}", info.name).Replace("${MemberName}", info.name.UppercaseFirst())).ToArray();
			return string.Join("\n", value);
		}
	}
}
