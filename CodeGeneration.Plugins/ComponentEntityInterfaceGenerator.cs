using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentEntityInterfaceGenerator : ICodeGenerator, ICodeGeneratorInterface, IConfigurable
	{
		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		private const string INTERFACE_TEMPLATE = "public interface ${InterfaceName} {\n\n    ${ComponentType} ${componentName} { get; }\n    bool has${ComponentName} { get; }\n\n    void Add${ComponentName}(${memberArgs});\n    void Replace${ComponentName}(${memberArgs});\n    void Remove${ComponentName}();\n}\n";

		private const string ENTITY_INTERFACE_EXTENSION = "public partial class ${ContextName}Entity : ${InterfaceName} { }\n";

		private const string MEMBER_ARGS_TEMPLATE = "${MemberType} new${MemberName}";

		public string name
		{
			get
			{
				return "Component (Entity Interface)";
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
			if (data.GetContextNames().Length > 1)
			{
				return new CodeGenFile[1]
				{
					this.generateInterface(data)
				}.Concat(from contextName in data.GetContextNames()
				select this.generateEntityInterfaceExtension(contextName, data)).ToArray();
			}
			return new CodeGenFile[0];
		}

		private CodeGenFile generateInterface(ComponentData data)
		{
			string text = data.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces);
			MemberData[] memberData = data.GetMemberData();
			string text2 = "I" + text.RemoveComponentSuffix();
			string fileContent = "public interface ${InterfaceName} {\n\n    ${ComponentType} ${componentName} { get; }\n    bool has${ComponentName} { get; }\n\n    void Add${ComponentName}(${memberArgs});\n    void Replace${ComponentName}(${memberArgs});\n    void Remove${ComponentName}();\n}\n".Replace("${InterfaceName}", text2).Replace("${ComponentType}", data.GetFullTypeName()).Replace("${ComponentName}", text)
				.Replace("${componentName}", text.LowercaseFirst())
				.Replace("${memberArgs}", this.getMemberArgs(memberData));
			string[] obj = new string[6]
			{
				"Components",
				null,
				null,
				null,
				null,
				null
			};
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			obj[1] = directorySeparatorChar.ToString();
			obj[2] = "Interfaces";
			directorySeparatorChar = Path.DirectorySeparatorChar;
			obj[3] = directorySeparatorChar.ToString();
			obj[4] = text2;
			obj[5] = ".cs";
			return new CodeGenFile(string.Concat(obj), fileContent, base.GetType().FullName);
		}

		private CodeGenFile generateEntityInterfaceExtension(string contextName, ComponentData data)
		{
			string componentName = data.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces);
			string text = "I" + componentName.RemoveComponentSuffix();
			string fileContent = "public partial class ${ContextName}Entity : ${InterfaceName} { }\n".Replace("${InterfaceName}", "I" + componentName.RemoveComponentSuffix()).Replace("${ContextName}", contextName);
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
			obj[5] = componentName.AddComponentSuffix();
			obj[6] = ".cs";
			return new CodeGenFile(string.Concat(obj), fileContent, base.GetType().FullName);
		}

		private string getMemberArgs(MemberData[] memberData)
		{
			string[] value = (from info in memberData
			select "${MemberType} new${MemberName}".Replace("${MemberType}", info.type).Replace("${MemberName}", info.name.UppercaseFirst())).ToArray();
			return string.Join(", ", value);
		}
	}
}
