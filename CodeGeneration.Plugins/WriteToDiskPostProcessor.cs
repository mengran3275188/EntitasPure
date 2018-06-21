using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;

namespace Entitas.CodeGeneration.Plugins
{
	public class WriteToDiskPostProcessor : ICodeGenFilePostProcessor, ICodeGeneratorInterface, IConfigurable
	{
		private readonly TargetDirectoryConfig _targetDirectoryConfig = new TargetDirectoryConfig();

		public string name
		{
			get
			{
				return "Write to disk";
			}
		}

		public int priority
		{
			get
			{
				return 100;
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
				return false;
			}
		}

		public Dictionary<string, string> defaultProperties
		{
			get
			{
				return this._targetDirectoryConfig.defaultProperties;
			}
		}

		public void Configure(Properties properties)
		{
			this._targetDirectoryConfig.Configure(properties);
		}

		public CodeGenFile[] PostProcess(CodeGenFile[] files)
		{
			foreach (CodeGenFile codeGenFile in files)
			{
				string path = this._targetDirectoryConfig.targetDirectory + Path.DirectorySeparatorChar.ToString() + codeGenFile.fileName;
				string directoryName = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllText(path, codeGenFile.fileContent);
			}
			return files;
		}
	}
}
