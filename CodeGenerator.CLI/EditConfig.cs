using DesperateDevs.Logging;
using System.Diagnostics;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public class EditConfig : AbstractCommand
	{
		public override string trigger
		{
			get
			{
				return "edit";
			}
		}

		public override void Run(string[] args)
		{
			if (base.assertProperties())
			{
				fabl.Debug("Opening Entitas.properties");
				Process.Start("Entitas.properties");
			}
		}
	}
}
