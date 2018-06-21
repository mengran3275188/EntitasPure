using System;

namespace Entitas.CodeGeneration.Plugins
{
	public class ConsoleWriteLinePostProcessor : ICodeGenFilePostProcessor, ICodeGeneratorInterface
	{
		public string name
		{
			get
			{
				return "Console.WriteLine generated files";
			}
		}

		public int priority
		{
			get
			{
				return 200;
			}
		}

		public bool isEnabledByDefault
		{
			get
			{
				return false;
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
			foreach (CodeGenFile codeGenFile in files)
			{
				Console.WriteLine(codeGenFile.fileName + " - " + codeGenFile.generatorName);
			}
			return files;
		}
	}
}
