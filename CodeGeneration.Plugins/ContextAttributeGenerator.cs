using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextAttributeGenerator : ICodeGenerator, ICodeGeneratorInterface
	{
		private const string ATTRIBUTE_TEMPLATE = "public sealed class ${ContextName}Attribute : Entitas.CodeGeneration.Attributes.ContextAttribute {\n\n    public ${ContextName}Attribute() : base(\"${ContextName}\") {\n    }\n}\n";

		public string name
		{
			get
			{
				return "Context Attribute";
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
			select this.generateAttributeClass(d)).ToArray();
		}

		private CodeGenFile generateAttributeClass(ContextData data)
		{
			string contextName = data.GetContextName();
			return new CodeGenFile(contextName + Path.DirectorySeparatorChar.ToString() + contextName + "Attribute.cs", "public sealed class ${ContextName}Attribute : Entitas.CodeGeneration.Attributes.ContextAttribute {\n\n    public ${ContextName}Attribute() : base(\"${ContextName}\") {\n    }\n}\n".Replace("${ContextName}", contextName), base.GetType().FullName);
		}
	}
}
