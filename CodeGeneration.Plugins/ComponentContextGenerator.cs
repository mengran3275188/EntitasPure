using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentContextGenerator : ICodeGenerator, ICodeGeneratorInterface, IConfigurable
	{
		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		private const string STANDARD_COMPONENT_TEMPLATE = "public partial class ${ContextName}Context {\n\n    public ${ContextName}Entity ${componentName}Entity { get { return GetGroup(${ContextName}Matcher.${ComponentName}).GetSingleEntity(); } }\n    public ${ComponentType} ${componentName} { get { return ${componentName}Entity.${componentName}; } }\n    public bool has${ComponentName} { get { return ${componentName}Entity != null; } }\n\n    public ${ContextName}Entity Set${ComponentName}(${memberArgs}) {\n        if (has${ComponentName}) {\n            throw new Entitas.EntitasException(\"Could not set ${ComponentName}!\\n\" + this + \" already has an entity with ${ComponentType}!\",\n                \"You should check if the context already has a ${componentName}Entity before setting it or use context.Replace${ComponentName}().\");\n        }\n        var entity = CreateEntity();\n        entity.Add${ComponentName}(${methodArgs});\n        return entity;\n    }\n\n    public void Replace${ComponentName}(${memberArgs}) {\n        var entity = ${componentName}Entity;\n        if (entity == null) {\n            entity = Set${ComponentName}(${methodArgs});\n        } else {\n            entity.Replace${ComponentName}(${methodArgs});\n        }\n    }\n\n    public void Remove${ComponentName}() {\n        ${componentName}Entity.Destroy();\n    }\n}\n";

		private const string MEMBER_ARGS_TEMPLATE = "${MemberType} new${MemberName}";

		private const string METHOD_ARGS_TEMPLATE = "new${MemberName}";

		private const string FLAG_COMPONENT_TEMPLATE = "public partial class ${ContextName}Context {\n\n    public ${ContextName}Entity ${componentName}Entity { get { return GetGroup(${ContextName}Matcher.${ComponentName}).GetSingleEntity(); } }\n\n    public bool ${prefixedComponentName} {\n        get { return ${componentName}Entity != null; }\n        set {\n            var entity = ${componentName}Entity;\n            if (value != (entity != null)) {\n                if (value) {\n                    CreateEntity().${prefixedComponentName} = true;\n                } else {\n                    entity.Destroy();\n                }\n            }\n        }\n    }\n}\n";

		public string name
		{
			get
			{
				return "Component (Context API)";
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
			where d.IsUnique()
			select d).SelectMany((ComponentData d) => this.generateExtensions(d)).ToArray();
		}

		private CodeGenFile[] generateExtensions(ComponentData data)
		{
			return (from contextName in data.GetContextNames()
			select this.generateExtension(contextName, data)).ToArray();
		}

		private CodeGenFile generateExtension(string contextName, ComponentData data)
		{
			MemberData[] memberData = data.GetMemberData();
			string text = data.GetFullTypeName().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces);
			string fileContent = ((memberData.Length == 0) ? "public partial class ${ContextName}Context {\n\n    public ${ContextName}Entity ${componentName}Entity { get { return GetGroup(${ContextName}Matcher.${ComponentName}).GetSingleEntity(); } }\n\n    public bool ${prefixedComponentName} {\n        get { return ${componentName}Entity != null; }\n        set {\n            var entity = ${componentName}Entity;\n            if (value != (entity != null)) {\n                if (value) {\n                    CreateEntity().${prefixedComponentName} = true;\n                } else {\n                    entity.Destroy();\n                }\n            }\n        }\n    }\n}\n" : "public partial class ${ContextName}Context {\n\n    public ${ContextName}Entity ${componentName}Entity { get { return GetGroup(${ContextName}Matcher.${ComponentName}).GetSingleEntity(); } }\n    public ${ComponentType} ${componentName} { get { return ${componentName}Entity.${componentName}; } }\n    public bool has${ComponentName} { get { return ${componentName}Entity != null; } }\n\n    public ${ContextName}Entity Set${ComponentName}(${memberArgs}) {\n        if (has${ComponentName}) {\n            throw new Entitas.EntitasException(\"Could not set ${ComponentName}!\\n\" + this + \" already has an entity with ${ComponentType}!\",\n                \"You should check if the context already has a ${componentName}Entity before setting it or use context.Replace${ComponentName}().\");\n        }\n        var entity = CreateEntity();\n        entity.Add${ComponentName}(${methodArgs});\n        return entity;\n    }\n\n    public void Replace${ComponentName}(${memberArgs}) {\n        var entity = ${componentName}Entity;\n        if (entity == null) {\n            entity = Set${ComponentName}(${methodArgs});\n        } else {\n            entity.Replace${ComponentName}(${methodArgs});\n        }\n    }\n\n    public void Remove${ComponentName}() {\n        ${componentName}Entity.Destroy();\n    }\n}\n").Replace("${ContextName}", contextName).Replace("${ComponentType}", data.GetFullTypeName()).Replace("${ComponentName}", text)
				.Replace("${componentName}", text.LowercaseFirst())
				.Replace("${prefixedComponentName}", data.GetUniqueComponentPrefix().LowercaseFirst() + text)
				.Replace("${memberArgs}", this.getMemberArgs(memberData))
				.Replace("${methodArgs}", this.getMethodArgs(memberData));
            //string[] obj = new string[7]
            //{
            //    contextName,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null
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

		private string getMemberArgs(MemberData[] memberData)
		{
			string[] value = (from info in memberData
			select "${MemberType} new${MemberName}".Replace("${MemberType}", info.type).Replace("${MemberName}", info.name.UppercaseFirst())).ToArray();
			return string.Join(", ", value);
		}

		private string getMethodArgs(MemberData[] memberData)
		{
			string[] value = (from info in memberData
			select "new${MemberName}".Replace("${MemberName}", info.name.UppercaseFirst())).ToArray();
			return string.Join(", ", value);
		}
	}
}
