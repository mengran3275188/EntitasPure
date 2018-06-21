using DesperateDevs.Logging;
using System;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	public class ScanDlls : AbstractCommand
	{
		public override string trigger
		{
			get
			{
				return "scan";
			}
		}

		public override void Run(string[] args)
		{
			if (base.assertProperties())
			{
				ScanDlls.printTypes(CodeGeneratorUtil.LoadTypesFromPlugins(base.loadProperties()));
			}
		}

		private static void printTypes(Type[] types)
		{
			foreach (Type item in from type in types
			orderby type.Assembly.GetName().Name, type.FullName
			select type)
			{
				fabl.Info(item.Assembly.GetName().Name + ": " + item);
			}
		}
	}
}
