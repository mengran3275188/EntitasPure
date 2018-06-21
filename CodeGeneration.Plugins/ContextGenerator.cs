using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextGenerator : ICodeGenerator, ICodeGeneratorInterface
	{
		private const string CONTEXT_TEMPLATE = "public sealed partial class ${ContextName}Context : Entitas.Context<${ContextName}Entity> {\n\n    public ${ContextName}Context()\n        : base(\n            ${Lookup}.TotalComponents,\n            0,\n            new Entitas.ContextInfo(\n                \"${ContextName}\",\n                ${Lookup}.componentNames,\n                ${Lookup}.componentTypes\n            ),\n            (entity) =>\n\n#if (ENTITAS_FAST_AND_UNSAFE)\n                new Entitas.UnsafeAERC()\n#else\n                new Entitas.SafeAERC(entity)\n#endif\n\n        ) {\n    }\n}\n";

		public string name
		{
			get
			{
				return "Context";
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

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return (from d in data.OfType<ContextData>()
			select this.generateContextClass(d)).ToArray();
		}

		private CodeGenFile generateContextClass(ContextData data)
		{
			string contextName = data.GetContextName();
			return new CodeGenFile(contextName + Path.DirectorySeparatorChar.ToString() + contextName + "Context.cs", "public sealed partial class ${ContextName}Context : Entitas.Context<${ContextName}Entity> {\n\n    public ${ContextName}Context()\n        : base(\n            ${Lookup}.TotalComponents,\n            0,\n            new Entitas.ContextInfo(\n                \"${ContextName}\",\n                ${Lookup}.componentNames,\n                ${Lookup}.componentTypes\n            ),\n            (entity) =>\n\n#if (ENTITAS_FAST_AND_UNSAFE)\n                new Entitas.UnsafeAERC()\n#else\n                new Entitas.SafeAERC(entity)\n#endif\n\n        ) {\n    }\n}\n".Replace("${ContextName}", contextName).Replace("${Lookup}", contextName + "ComponentsLookup"), base.GetType().FullName);
		}
	}
}
