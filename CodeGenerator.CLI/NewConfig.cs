using DesperateDevs.Utils;
using DesperateDevs.Logging;
using System.IO;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public class NewConfig : AbstractCommand
	{
		public override string trigger
		{
			get
			{
				return "new";
			}
		}

		public override void Run(string[] args)
		{
			string text = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar.ToString() + "Entitas.properties";
			if (ArgsExtension.isForce(args) || !File.Exists(text))
			{
				CodeGeneratorConfig codeGeneratorConfig = new CodeGeneratorConfig();
				Properties properties = new Properties(codeGeneratorConfig.defaultProperties);
				//codeGeneratorConfig.Configure(properties);
				string text2 = codeGeneratorConfig.ToString();
				File.WriteAllText(text, text2);
				fabl.Info("Created " + text);
				fabl.Debug(text2);
				new EditConfig().Run(args);
			}
			else
			{
				fabl.Warn(text + " already exists!");
				fabl.Info("Use entitas new -f to overwrite the exiting file.");
				fabl.Info("Use entitas edit to open the exiting file.");
			}
		}
	}
}
