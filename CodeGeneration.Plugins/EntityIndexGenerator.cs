using DesperateDevs.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class EntityIndexGenerator : ICodeGenerator, ICodeGeneratorInterface, IConfigurable
	{
		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		private const string CLASS_TEMPLATE = "public partial class Contexts {\n\n${indexConstants}\n\n    [Entitas.CodeGeneration.Attributes.PostConstructor]\n    public void InitializeEntityIndices() {\n${addIndices}\n    }\n}\n\npublic static class ContextsExtensions {\n\n${getIndices}\n}";

		private const string INDEX_CONSTANTS_TEMPLATE = "    public const string ${IndexName} = \"${IndexName}\";";

		private const string ADD_INDEX_TEMPLATE = "        ${contextName}.AddEntityIndex(new ${IndexType}<${ContextName}Entity, ${KeyType}>(\n            ${IndexName},\n            ${contextName}.GetGroup(${ContextName}Matcher.${IndexName}),\n            (e, c) => ((${ComponentType})c).${MemberName}));";

		private const string ADD_CUSTOM_INDEX_TEMPLATE = "        ${contextName}.AddEntityIndex(new ${IndexType}(${contextName}));";

		private const string GET_INDEX_TEMPLATE = "    public static System.Collections.Generic.HashSet<${ContextName}Entity> GetEntitiesWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {\n        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntities(${MemberName});\n    }";

		private const string GET_PRIMARY_INDEX_TEMPLATE = "    public static ${ContextName}Entity GetEntityWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {\n        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntity(${MemberName});\n    }";

		private const string CUSTOM_METHOD_TEMPLATE = "    public static ${ReturnType} ${MethodName}(this ${ContextName}Context context, ${methodArgs}) {\n        return ((${IndexType})(context.GetEntityIndex(Contexts.${IndexName}))).${MethodName}(${args});\n    }\n";

		public string name
		{
			get
			{
				return "Entity Index";
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
			EntityIndexData[] array = data.OfType<EntityIndexData>().ToArray();
			if (array.Length != 0)
			{
				return this.generateEntityIndices(array);
			}
			return new CodeGenFile[0];
		}

		private CodeGenFile[] generateEntityIndices(EntityIndexData[] data)
		{
			string newValue = string.Join("\n", (from d in data
			select "    public const string ${IndexName} = \"${IndexName}\";".Replace("${IndexName}", d.GetEntityIndexName())).ToArray());
			string newValue2 = string.Join("\n\n", (from d in data
			select this.generateAddMethods(d)).ToArray());
			string newValue3 = string.Join("\n\n", (from d in data
			select this.generateGetMethods(d)).ToArray());
			string fileContent = "public partial class Contexts {\n\n${indexConstants}\n\n    [Entitas.CodeGeneration.Attributes.PostConstructor]\n    public void InitializeEntityIndices() {\n${addIndices}\n    }\n}\n\npublic static class ContextsExtensions {\n\n${getIndices}\n}".Replace("${indexConstants}", newValue).Replace("${addIndices}", newValue2).Replace("${getIndices}", newValue3);
			return new CodeGenFile[1]
			{
				new CodeGenFile("Contexts.cs", fileContent, base.GetType().FullName)
			};
		}

		private string generateAddMethods(EntityIndexData data)
		{
			return string.Join("\n", data.GetContextNames().Aggregate(new List<string>(), delegate(List<string> addMethods, string contextName)
			{
				addMethods.Add(this.generateAddMethod(data, contextName));
				return addMethods;
			}).ToArray());
		}

		private string generateAddMethod(EntityIndexData data, string contextName)
		{
			if (!data.IsCustom())
			{
				return this.generateMethods(data, contextName);
			}
			return this.generateCustomMethods(data);
		}

		private string generateCustomMethods(EntityIndexData data)
		{
			return "        ${contextName}.AddEntityIndex(new ${IndexType}(${contextName}));".Replace("${contextName}", data.GetContextNames()[0].LowercaseFirst()).Replace("${IndexType}", data.GetEntityIndexType());
		}

		private string generateMethods(EntityIndexData data, string contextName)
		{
			return "        ${contextName}.AddEntityIndex(new ${IndexType}<${ContextName}Entity, ${KeyType}>(\n            ${IndexName},\n            ${contextName}.GetGroup(${ContextName}Matcher.${IndexName}),\n            (e, c) => ((${ComponentType})c).${MemberName}));".Replace("${contextName}", contextName.LowercaseFirst()).Replace("${ContextName}", contextName).Replace("${IndexName}", data.GetEntityIndexName())
				.Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${KeyType}", data.GetKeyType())
				.Replace("${ComponentType}", data.GetComponentType())
				.Replace("${MemberName}", data.GetMemberName())
				.Replace("${componentName}", data.GetComponentType().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces).LowercaseFirst());
		}

		private string generateGetMethods(EntityIndexData data)
		{
			return string.Join("\n\n", data.GetContextNames().Aggregate(new List<string>(), delegate(List<string> getMethods, string contextName)
			{
				getMethods.Add(this.generateGetMethod(data, contextName));
				return getMethods;
			}).ToArray());
		}

		private string generateGetMethod(EntityIndexData data, string contextName)
		{
			string text = "";
			if (data.GetEntityIndexType() == "Entitas.EntityIndex")
			{
				text = "    public static System.Collections.Generic.HashSet<${ContextName}Entity> GetEntitiesWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {\n        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntities(${MemberName});\n    }";
				goto IL_0042;
			}
			if (data.GetEntityIndexType() == "Entitas.PrimaryEntityIndex")
			{
				text = "    public static ${ContextName}Entity GetEntityWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {\n        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntity(${MemberName});\n    }";
				goto IL_0042;
			}
			return this.getCustomMethods(data);
			IL_0042:
			return text.Replace("${ContextName}", contextName).Replace("${IndexName}", data.GetEntityIndexName()).Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${KeyType}", data.GetKeyType())
				.Replace("${MemberName}", data.GetMemberName());
		}

		private string getCustomMethods(EntityIndexData data)
		{
			return string.Join("\n", (from m in data.GetCustomMethods()
			select "    public static ${ReturnType} ${MethodName}(this ${ContextName}Context context, ${methodArgs}) {\n        return ((${IndexType})(context.GetEntityIndex(Contexts.${IndexName}))).${MethodName}(${args});\n    }\n".Replace("${ReturnType}", m.returnType).Replace("${MethodName}", m.methodName).Replace("${ContextName}", data.GetContextNames()[0])
				.Replace("${methodArgs}", string.Join(", ", (from p in m.parameters
				select p.type + " " + p.name).ToArray()))
				.Replace("${IndexType}", data.GetEntityIndexType())
				.Replace("${IndexName}", data.GetEntityIndexName())
				.Replace("${args}", string.Join(", ", (from p in m.parameters
				select p.name).ToArray()))).ToArray());
		}
	}
}
