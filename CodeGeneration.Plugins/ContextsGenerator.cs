using DesperateDevs.Utils;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ContextsGenerator : ICodeGenerator, ICodeGeneratorInterface
	{
		private const string CONTEXTS_TEMPLATE = "public partial class Contexts : Entitas.IContexts {\n\n    public static Contexts sharedInstance {\n        get {\n            if (_sharedInstance == null) {\n                _sharedInstance = new Contexts();\n            }\n\n            return _sharedInstance;\n        }\n        set { _sharedInstance = value; }\n    }\n\n    static Contexts _sharedInstance;\n\n${contextProperties}\n\n    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { ${contextList} }; } }\n\n    public Contexts() {\n${contextAssignments}\n\n        var postConstructors = System.Linq.Enumerable.Where(\n            GetType().GetMethods(),\n            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))\n        );\n\n        foreach (var postConstructor in postConstructors) {\n            postConstructor.Invoke(this, null);\n        }\n    }\n\n    public void Reset() {\n        var contexts = allContexts;\n        for (int i = 0; i < contexts.Length; i++) {\n            contexts[i].Reset();\n        }\n    }\n}\n";

		private const string CONTEXT_PROPERTY_TEMPLATE = "    public ${ContextName}Context ${contextName} { get; set; }";

		private const string CONTEXT_LIST_TEMPLATE = "${contextName}";

		private const string CONTEXT_ASSIGNMENT_TEMPLATE = "        ${contextName} = new ${ContextName}Context();";

		public string name
		{
			get
			{
				return "Contexts";
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
			string[] contextNames = (from d in data.OfType<ContextData>()
			select d.GetContextName() into contextName
			orderby contextName
			select contextName).ToArray();
			return new CodeGenFile[1]
			{
				new CodeGenFile("Contexts.cs", this.generateContextsClass(contextNames), base.GetType().FullName)
			};
		}

		private string generateContextsClass(string[] contextNames)
		{
			string newValue = string.Join("\n", (from contextName in contextNames
			select "    public ${ContextName}Context ${contextName} { get; set; }".Replace("${ContextName}", contextName).Replace("${contextName}", contextName.LowercaseFirst())).ToArray());
			string newValue2 = string.Join(", ", (from contextName in contextNames
			select "${contextName}".Replace("${contextName}", contextName.LowercaseFirst())).ToArray());
			string newValue3 = string.Join("\n", (from contextName in contextNames
			select "        ${contextName} = new ${ContextName}Context();".Replace("${ContextName}", contextName).Replace("${contextName}", contextName.LowercaseFirst())).ToArray());
			return "public partial class Contexts : Entitas.IContexts {\n\n    public static Contexts sharedInstance {\n        get {\n            if (_sharedInstance == null) {\n                _sharedInstance = new Contexts();\n            }\n\n            return _sharedInstance;\n        }\n        set { _sharedInstance = value; }\n    }\n\n    static Contexts _sharedInstance;\n\n${contextProperties}\n\n    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { ${contextList} }; } }\n\n    public Contexts() {\n${contextAssignments}\n\n        var postConstructors = System.Linq.Enumerable.Where(\n            GetType().GetMethods(),\n            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))\n        );\n\n        foreach (var postConstructor in postConstructors) {\n            postConstructor.Invoke(this, null);\n        }\n    }\n\n    public void Reset() {\n        var contexts = allContexts;\n        for (int i = 0; i < contexts.Length; i++) {\n            contexts[i].Reset();\n        }\n    }\n}\n".Replace("${contextProperties}", newValue).Replace("${contextList}", newValue2).Replace("${contextAssignments}", newValue3);
		}
	}
}
