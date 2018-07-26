using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class MergeFilesPostProcessor : IPostProcessor, ICodeGenerationPlugin
	{
		public string name
		{
			get
			{
				return "Merge files";
			}
		}

		public int priority
		{
			get
			{
				return 90;
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
			Dictionary<string, CodeGenFile> dictionary = new Dictionary<string, CodeGenFile>();
			for (int i = 0; i < files.Length; i++)
			{
				CodeGenFile codeGenFile = files[i];
				if (!dictionary.ContainsKey(codeGenFile.fileName))
				{
					dictionary.Add(codeGenFile.fileName, codeGenFile);
				}
				else
				{
					CodeGenFile codeGenFile2 = dictionary[codeGenFile.fileName];
					codeGenFile2.fileContent = codeGenFile2.fileContent + "\n" + codeGenFile.fileContent;
					CodeGenFile codeGenFile3 = dictionary[codeGenFile.fileName];
					codeGenFile3.generatorName = codeGenFile3.generatorName + ", " + codeGenFile.generatorName;
					files[i] = null;
				}
			}
			return (from file in files
			where file != null
			select file).ToArray();
		}
	}
}
