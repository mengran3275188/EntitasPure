using DesperateDevs.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Entitas.CodeGeneration.Plugins
{
	public class UpdateCSProjPostProcessor : ICodeGenFilePostProcessor, ICodeGeneratorInterface, IConfigurable
	{
		private readonly ProjectPathConfig _projectPathConfig = new ProjectPathConfig();

		private readonly TargetDirectoryConfig _targetDirectoryConfig = new TargetDirectoryConfig();

		public string name
		{
			get
			{
				return "Update .csproj";
			}
		}

		public int priority
		{
			get
			{
				return 96;
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
				return false;
			}
		}

		public Dictionary<string, string> defaultProperties
		{
			get
			{
				return this._projectPathConfig.defaultProperties.Merge(this._targetDirectoryConfig.defaultProperties);
			}
		}

		public void Configure(Properties properties)
		{
			this._projectPathConfig.Configure(properties);
			this._targetDirectoryConfig.Configure(properties);
		}

		public CodeGenFile[] PostProcess(CodeGenFile[] files)
		{
			string project = File.ReadAllText(this._projectPathConfig.projectPath);
			project = this.removeExistingGeneratedEntries(project);
			project = this.addGeneratedEntries(project, files);
			File.WriteAllText(this._projectPathConfig.projectPath, project);
			return files;
		}

		private string removeExistingGeneratedEntries(string project)
		{
			string str = this._targetDirectoryConfig.targetDirectory.Replace("/", "\\").Replace("\\", "\\\\");
			string pattern = "\\s*<Compile Include=\"" + str + ".* \\/>";
			project = Regex.Replace(project, pattern, string.Empty);
			project = Regex.Replace(project, "\\s*<ItemGroup>\\s*<\\/ItemGroup>", string.Empty);
			return project;
		}

		private string addGeneratedEntries(string project, CodeGenFile[] files)
		{
			string entryTemplate = "    <Compile Include=\"" + this._targetDirectoryConfig.targetDirectory.Replace("/", "\\") + "\\{0}\" />";
			string arg = string.Join("\r\n", (from file in files
			select string.Format(entryTemplate, file.fileName.Replace("/", "\\"))).ToArray());
			string replacement = string.Format("</ItemGroup>\n  <ItemGroup>\n{0}\n  </ItemGroup>", arg);
			return new Regex("<\\/ItemGroup>").Replace(project, replacement, 1);
		}
	}
}
