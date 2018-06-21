using System.IO;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class EntityGenerator : ICodeGenerator, ICodeGeneratorInterface
	{
		private const string ENTITY_TEMPLATE = "public sealed partial class ${ContextName}Entity : Entitas.Entity {\n}\n";

		public string name
		{
			get
			{
				return "Entity";
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
			select this.generateEntityClass(d)).ToArray();
		}

		private CodeGenFile generateEntityClass(ContextData data)
		{
			string contextName = data.GetContextName();
			return new CodeGenFile(contextName + Path.DirectorySeparatorChar.ToString() + contextName + "Entity.cs", "public sealed partial class ${ContextName}Entity : Entitas.Entity {\n}\n".Replace("${ContextName}", contextName), base.GetType().FullName);
		}
	}
}
