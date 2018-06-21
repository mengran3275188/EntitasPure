using DesperateDevs.Utils;
using DesperateDevs.Logging;
using System.IO;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public abstract class AbstractCommand : ICommand
	{
		public abstract string trigger
		{
			get;
		}

		public abstract void Run(string[] args);

		protected Properties loadProperties()
		{
			return new Properties(File.ReadAllText("Entitas.properties"));
		}

		protected bool assertProperties()
		{
			if (File.Exists("Entitas.properties"))
			{
				return true;
			}
			fabl.Warn("Couldn't find Entitas.properties");
			fabl.Info("Run 'entitas new' to create Entitas.properties with default values");
			return false;
		}
	}
}
