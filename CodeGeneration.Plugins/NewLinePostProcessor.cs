using System;

namespace Entitas.CodeGeneration.Plugins
{
	public class NewLinePostProcessor : ICodeGenFilePostProcessor, ICodeGeneratorInterface
	{
		public string name
		{
			get
			{
				return "Convert newlines";
			}
		}

		public int priority
		{
			get
			{
				return 95;
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

		public CodeGenFile[] PostProcess(CodeGenFile[] files)
		{
			foreach (CodeGenFile obj in files)
			{
				obj.fileContent = obj.fileContent.Replace("\n", Environment.NewLine);
			}
			return files;
		}
	}
}
