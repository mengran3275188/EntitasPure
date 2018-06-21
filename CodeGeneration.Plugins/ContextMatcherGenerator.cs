using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextMatcherGenerator : ICodeGenerator, ICodeGeneratorInterface
	{
		private const string CONTEXT_TEMPLATE = "public sealed partial class ${ContextName}Matcher {\n\n    public static Entitas.IAllOfMatcher<${ContextName}Entity> AllOf(params int[] indices) {\n        return Entitas.Matcher<${ContextName}Entity>.AllOf(indices);\n    }\n\n    public static Entitas.IAllOfMatcher<${ContextName}Entity> AllOf(params Entitas.IMatcher<${ContextName}Entity>[] matchers) {\n          return Entitas.Matcher<${ContextName}Entity>.AllOf(matchers);\n    }\n\n    public static Entitas.IAnyOfMatcher<${ContextName}Entity> AnyOf(params int[] indices) {\n          return Entitas.Matcher<${ContextName}Entity>.AnyOf(indices);\n    }\n\n    public static Entitas.IAnyOfMatcher<${ContextName}Entity> AnyOf(params Entitas.IMatcher<${ContextName}Entity>[] matchers) {\n          return Entitas.Matcher<${ContextName}Entity>.AnyOf(matchers);\n    }\n}\n";

		public string name
		{
			get
			{
				return "Context (Matcher API)";
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
			return new CodeGenFile(contextName + Path.DirectorySeparatorChar.ToString() + contextName + "Matcher.cs", "public sealed partial class ${ContextName}Matcher {\n\n    public static Entitas.IAllOfMatcher<${ContextName}Entity> AllOf(params int[] indices) {\n        return Entitas.Matcher<${ContextName}Entity>.AllOf(indices);\n    }\n\n    public static Entitas.IAllOfMatcher<${ContextName}Entity> AllOf(params Entitas.IMatcher<${ContextName}Entity>[] matchers) {\n          return Entitas.Matcher<${ContextName}Entity>.AllOf(matchers);\n    }\n\n    public static Entitas.IAnyOfMatcher<${ContextName}Entity> AnyOf(params int[] indices) {\n          return Entitas.Matcher<${ContextName}Entity>.AnyOf(indices);\n    }\n\n    public static Entitas.IAnyOfMatcher<${ContextName}Entity> AnyOf(params Entitas.IMatcher<${ContextName}Entity>[] matchers) {\n          return Entitas.Matcher<${ContextName}Entity>.AnyOf(matchers);\n    }\n}\n".Replace("${ContextName}", contextName), base.GetType().FullName);
		}
	}
}
