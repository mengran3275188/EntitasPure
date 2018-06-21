using DesperateDevs.Utils;
using DesperateDevs.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entitas.CodeGeneration.CodeGenerator.CLI
{
	internal class MainClass
	{
		private static Dictionary<LogLevel, ConsoleColor> _consoleColors = new Dictionary<LogLevel, ConsoleColor>
		{
			{
				LogLevel.Warn,
				ConsoleColor.DarkYellow
			},
			{
				LogLevel.Error,
				ConsoleColor.Red
			},
			{
				LogLevel.Fatal,
				ConsoleColor.DarkRed
			}
		};

		public static void Main(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				Entitas.CodeGeneration.CodeGenerator.CLI.MainClass.printUsage();
			}
			else
			{
				Entitas.CodeGeneration.CodeGenerator.CLI.MainClass.setupLogging(args);
				try
				{
					ICommand command = AppDomain.CurrentDomain.GetInstancesOf<ICommand>().SingleOrDefault((ICommand c) => c.trigger == args[0]);
					if (command != null)
					{
						command.Run(args);
					}
					else
					{
						Entitas.CodeGeneration.CodeGenerator.CLI.MainClass.printUsage();
					}
				}
				catch (Exception ex)
				{
					Entitas.CodeGeneration.CodeGenerator.CLI.MainClass.printException(ex, args);
				}
			}
		}

		private static void printException(Exception ex, string[] args)
		{
			ReflectionTypeLoadException ex2 = ex as ReflectionTypeLoadException;
			if (ex2 != null)
			{
				Exception[] loaderExceptions = ex2.LoaderExceptions;
				for (int i = 0; i < loaderExceptions.Length; i++)
				{
					fabl.Error(loaderExceptions[i].ToString());
				}
			}
			else if (ArgsExtension.isVerbose(args))
			{
				fabl.Error(ex.ToString());
			}
			else
			{
				fabl.Error(ex.Message);
			}
		}

		private static void printUsage()
		{
			//Console.WriteLine("Entitas Code Generator version " + EntitasResources.GetVersion());
			Console.WriteLine("usage: entitas new [-f] - Creates new Entitas.properties config with default values\n       entitas edit     - Opens Entitas.properties config\n       entitas doctor   - Checks the config for potential problems\n       entitas status   - Lists available and unavailable plugins\n       entitas fix      - Adds missing or removes unused keys interactively\n       entitas scan     - Scans and prints available types found in specified assemblies\n       entitas dry      - Simulates generating files without writing to disk\n       entitas gen      - Generates files based on Entitas.properties\n       [-v]             - verbose output\n       [-s]             - silent output (errors only)");
		}

		private static void setupLogging(string[] args)
		{
			if (ArgsExtension.isVerbose(args))
			{
				fabl.globalLogLevel = LogLevel.On;
			}
			else if (ArgsExtension.isSilent(args))
			{
				fabl.globalLogLevel = LogLevel.Error;
			}
			else
			{
				fabl.globalLogLevel = LogLevel.Info;
			}
			fabl.AddAppender(delegate(Logger logger, LogLevel logLevel, string message)
			{
				if (Entitas.CodeGeneration.CodeGenerator.CLI.MainClass._consoleColors.ContainsKey(logLevel))
				{
					Console.ForegroundColor = Entitas.CodeGeneration.CodeGenerator.CLI.MainClass._consoleColors[logLevel];
					Console.WriteLine(message);
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine(message);
				}
			});
		}
	}
}
