using DesperateDevs.Logging;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public class DryRun : AbstractCommand
	{
		public override string trigger
		{
			get
			{
				return "dry";
			}
		}

		public override void Run(string[] args)
		{
			if (base.assertProperties())
			{
				CodeGenerator codeGenerator = CodeGeneratorUtil.CodeGeneratorFromProperties();
				codeGenerator.OnProgress += delegate(string title, string info, float progress)
				{
					int num = (int)(progress * 100f);
					fabl.Debug(string.Format("{0}: {1} ({2}%)", title, info, num));
				};
				codeGenerator.DryRun();
			}
		}
	}
}
