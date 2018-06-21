using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.CodeGenerator;
using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entitas.CodeGeneration.Plugins
{
	public class EntityIndexDataProvider : ICodeGeneratorDataProvider, ICodeGeneratorInterface, IConfigurable
	{
		private readonly CodeGeneratorConfig _codeGeneratorConfig = new CodeGeneratorConfig();

		private readonly AssembliesConfig _assembliesConfig = new AssembliesConfig();

		private readonly IgnoreNamespacesConfig _ignoreNamespacesConfig = new IgnoreNamespacesConfig();

		private readonly ContextsComponentDataProvider _contextsComponentDataProvider = new ContextsComponentDataProvider();

		private Type[] _types;

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
				return this._assembliesConfig.defaultProperties.Merge(this._ignoreNamespacesConfig.defaultProperties, this._contextsComponentDataProvider.defaultProperties);
			}
		}

		public EntityIndexDataProvider()
			: this(null)
		{
		}

		public EntityIndexDataProvider(Type[] types)
		{
			this._types = types;
		}

		public void Configure(Properties properties)
		{
			this._codeGeneratorConfig.Configure(properties);
			this._assembliesConfig.Configure(properties);
			this._ignoreNamespacesConfig.Configure(properties);
			this._contextsComponentDataProvider.Configure(properties);
		}

		public CodeGeneratorData[] GetData()
		{
			if (this._types == null)
			{
				this._types = PluginUtil.GetAssembliesResolver(this._assembliesConfig.assemblies, this._codeGeneratorConfig.searchPaths).GetTypes();
			}
			IEnumerable<EntityIndexData> first = from kv in (from type in this._types
			where !type.IsAbstract
			where type.ImplementsInterface<IComponent>()
			select type).ToDictionary((Type type) => type, (Type type) => type.GetPublicMemberInfos())
			where kv.Value.Any((PublicMemberInfo info) => info.attributes.Any((AttributeInfo attr) => attr.attribute is AbstractEntityIndexAttribute))
			select this.createEntityIndexData(kv.Key, kv.Value);
			IEnumerable<EntityIndexData> second = from type in this._types
			where !type.IsAbstract
			where Attribute.IsDefined(type, typeof(CustomEntityIndexAttribute))
			select this.createCustomEntityIndexData(type);
			return first.Concat(second).ToArray();
		}

		private EntityIndexData createEntityIndexData(Type type, List<PublicMemberInfo> infos)
		{
			EntityIndexData entityIndexData = new EntityIndexData();
			PublicMemberInfo publicMemberInfo = infos.Single((PublicMemberInfo i) => i.attributes.Count((AttributeInfo attr) => attr.attribute is AbstractEntityIndexAttribute) == 1);
			AbstractEntityIndexAttribute attribute = (AbstractEntityIndexAttribute)publicMemberInfo.attributes.Single((AttributeInfo attr) => attr.attribute is AbstractEntityIndexAttribute).attribute;
			entityIndexData.SetEntityIndexType(this.getEntityIndexType(attribute));
			entityIndexData.IsCustom(false);
			entityIndexData.SetEntityIndexName(type.ToCompilableString().ToComponentName(this._ignoreNamespacesConfig.ignoreNamespaces));
			entityIndexData.SetKeyType(publicMemberInfo.type.ToCompilableString());
			entityIndexData.SetComponentType(type.ToCompilableString());
			entityIndexData.SetMemberName(publicMemberInfo.name);
			entityIndexData.SetContextNames(this._contextsComponentDataProvider.GetContextNamesOrDefault(type));
			return entityIndexData;
		}

		private EntityIndexData createCustomEntityIndexData(Type type)
		{
			EntityIndexData entityIndexData = new EntityIndexData();
			CustomEntityIndexAttribute customEntityIndexAttribute = (CustomEntityIndexAttribute)type.GetCustomAttributes(typeof(CustomEntityIndexAttribute), false)[0];
			entityIndexData.SetEntityIndexType(type.ToCompilableString());
			entityIndexData.IsCustom(true);
			entityIndexData.SetEntityIndexName(type.ToCompilableString().RemoveDots());
			entityIndexData.SetContextNames(new string[1]
			{
				customEntityIndexAttribute.contextType.ToCompilableString().ShortTypeName().RemoveContextSuffix()
			});
			MethodData[] methods = (from method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
			where Attribute.IsDefined(method, typeof(EntityIndexGetMethodAttribute))
			select new MethodData(method.ReturnType.ToCompilableString(), method.Name, (from p in method.GetParameters()
			select new MemberData(p.ParameterType.ToCompilableString(), p.Name)).ToArray())).ToArray();
			entityIndexData.SetCustomMethods(methods);
			return entityIndexData;
		}

		private string getEntityIndexType(AbstractEntityIndexAttribute attribute)
		{
			switch (attribute.entityIndexType)
			{
			case EntityIndexType.EntityIndex:
				return "Entitas.EntityIndex";
			case EntityIndexType.PrimaryEntityIndex:
				return "Entitas.PrimaryEntityIndex";
			default:
				throw new Exception("Unhandled EntityIndexType: " + attribute.entityIndexType);
			}
		}
	}
}
