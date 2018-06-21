using DesperateDevs.Utils;
using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentGenerator : ICodeGenerator, ICodeGeneratorInterface
	{
		private const string COMPONENT_TEMPLATE = "${Contexts}${Unique}\npublic sealed partial class ${FullComponentName} : Entitas.IComponent {\n    public ${Type} value;\n}\n";

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

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return (from d in data.OfType<ComponentData>()
			where d.ShouldGenerateComponent()
			select this.generateComponentClass(d)).ToArray();
		}

		private CodeGenFile generateComponentClass(ComponentData data)
		{
			string text = data.GetFullTypeName().RemoveDots();
			string text2 = string.Join(", ", data.GetContextNames());
			string newValue = data.IsUnique() ? "[Entitas.CodeGeneration.Attributes.UniqueAttribute]" : string.Empty;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = "[" + text2 + "]";
			}
			return new CodeGenFile("Components" + Path.DirectorySeparatorChar.ToString() + text + ".cs", "${Contexts}${Unique}\npublic sealed partial class ${FullComponentName} : Entitas.IComponent {\n    public ${Type} value;\n}\n".Replace("${FullComponentName}", text).Replace("${Type}", data.GetObjectType()).Replace("${Contexts}", text2)
				.Replace("${Unique}", newValue), base.GetType().FullName);
		}
	}
}
