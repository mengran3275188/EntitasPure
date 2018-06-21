using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.CodeGenerator;
using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentDataProvider : ICodeGeneratorDataProvider, ICodeGeneratorInterface, IConfigurable
	{
		private readonly CodeGeneratorConfig _codeGeneratorConfig = new CodeGeneratorConfig();

		private readonly AssembliesConfig _assembliesConfig = new AssembliesConfig();

		private readonly ContextsComponentDataProvider _contextsComponentDataProvider = new ContextsComponentDataProvider();

		private Type[] _types;

		private IComponentDataProvider[] _dataProviders;

		public string name
		{
			get
			{
				return "Component";
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
				Dictionary<string, string>[] dictionaries = (from i in this._dataProviders.OfType<IConfigurable>()
				select i.defaultProperties).ToArray();
				return this._assembliesConfig.defaultProperties.Merge(this._contextsComponentDataProvider.defaultProperties).Merge(dictionaries);
			}
		}

		private static IComponentDataProvider[] getComponentDataProviders()
		{
			return new IComponentDataProvider[8]
			{
				new ComponentTypeComponentDataProvider(),
				new MemberDataComponentDataProvider(),
				new ContextsComponentDataProvider(),
				new IsUniqueComponentDataProvider(),
				new UniquePrefixComponentDataProvider(),
				new ShouldGenerateComponentComponentDataProvider(),
				new ShouldGenerateMethodsComponentDataProvider(),
				new ShouldGenerateComponentIndexComponentDataProvider()
			};
		}

		public ComponentDataProvider()
			: this(null)
		{
		}

		public ComponentDataProvider(Type[] types)
			: this(types, ComponentDataProvider.getComponentDataProviders())
		{
		}

		protected ComponentDataProvider(Type[] types, IComponentDataProvider[] dataProviders)
		{
			this._types = types;
			this._dataProviders = dataProviders;
		}

		public void Configure(Properties properties)
		{
			this._codeGeneratorConfig.Configure(properties);
			this._assembliesConfig.Configure(properties);
			foreach (IConfigurable item in this._dataProviders.OfType<IConfigurable>())
			{
				item.Configure(properties);
			}
			this._contextsComponentDataProvider.Configure(properties);
		}

		public CodeGeneratorData[] GetData()
		{
			if (this._types == null)
			{
				this._types = PluginUtil.GetAssembliesResolver(this._assembliesConfig.assemblies, this._codeGeneratorConfig.searchPaths).GetTypes();
			}
			IEnumerable<ComponentData> source = from type in this._types
			where type.ImplementsInterface<IComponent>()
			where !type.IsAbstract
			select this.createDataForComponent(type);
			IEnumerable<ComponentData> enumerable = (from type in this._types
			where !type.ImplementsInterface<IComponent>()
			where !type.IsGenericType
			where this.hasContexts(type)
			select type).SelectMany((Type type) => this.createDataForNonComponent(type));
			ILookup<string, ComponentData> generatedComponentsLookup = enumerable.ToLookup((ComponentData data) => data.GetFullTypeName());
			return (from data in source
			where !generatedComponentsLookup.Contains(data.GetFullTypeName())
			select data).Concat(enumerable).ToArray();
		}

		private ComponentData createDataForComponent(Type type)
		{
			ComponentData componentData = new ComponentData();
			IComponentDataProvider[] dataProviders = this._dataProviders;
			for (int i = 0; i < dataProviders.Length; i++)
			{
				dataProviders[i].Provide(type, componentData);
			}
			return componentData;
		}

		private ComponentData[] createDataForNonComponent(Type type)
		{
			return this.getComponentNames(type).Select(delegate(string componentName)
			{
				ComponentData componentData = this.createDataForComponent(type);
				componentData.SetFullTypeName(componentName.AddComponentSuffix());
				componentData.SetMemberData(new MemberData[1]
				{
					new MemberData(type.ToCompilableString(), "value")
				});
				return componentData;
			}).ToArray();
		}

		private bool hasContexts(Type type)
		{
			return this._contextsComponentDataProvider.GetContextNames(type).Length != 0;
		}

		private string[] getComponentNames(Type type)
		{
			CustomComponentNameAttribute customComponentNameAttribute = Attribute.GetCustomAttributes(type).OfType<CustomComponentNameAttribute>().SingleOrDefault();
			if (customComponentNameAttribute == null)
			{
				return new string[1]
				{
					type.ToCompilableString().ShortTypeName().AddComponentSuffix()
				};
			}
			return customComponentNameAttribute.componentNames;
		}
	}
}
