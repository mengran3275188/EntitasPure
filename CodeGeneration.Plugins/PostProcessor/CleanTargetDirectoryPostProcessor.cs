using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Entitas.CodeGeneration.Plugins
{
	public class CleanTargetDirectoryPostProcessor : IPostProcessor, ICodeGenerationPlugin, IConfigurable
	{
		private readonly TargetDirectoryConfig _targetDirectoryConfig = new TargetDirectoryConfig();

		public string name
		{
			get
			{
				return "Clean target directory";
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

		public void Configure(Preferences preferences)
		{
			this._targetDirectoryConfig.Configure(preferences);
		}

		public CodeGenFile[] PostProcess(CodeGenFile[] files)
		{
			this.cleanDir();
			return files;
		}

		private void cleanDir()
		{
			if (Directory.Exists(this._targetDirectoryConfig.targetDirectory))
			{
				FileInfo[] files = new DirectoryInfo(this._targetDirectoryConfig.targetDirectory).GetFiles("*.cs", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo in files)
				{
					try
					{
						File.Delete(fileInfo.FullName);
					}
					catch
					{
						Console.WriteLine("Could not delete file " + fileInfo);
					}
				}
			}
			else
			{
				Directory.CreateDirectory(this._targetDirectoryConfig.targetDirectory);
			}
		}
	}
}
