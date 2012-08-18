using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Mono.Options;

using Mulder.Base.Commands;
using Mulder.Base.Logging;

namespace Mulder.Base
{
	public class EntryPoint
	{
		const string helpKey = "help";

		readonly ILog log;
		readonly IDictionary<string, ICommand> commands;
		readonly OptionSet options;

		bool showHelp;
		bool showVersionInfo;
		
		public EntryPoint(ILog log, IDictionary<string, ICommand> commands)
		{
			this.log = log;
			this.commands = commands;

			options = new OptionSet() {
				{ "h|?|help", "show the help message and quit", f => showHelp = true },
				{ "v|version", "show version information and quit", f => showVersionInfo = true }
			};
		}
		
		public ExitCode Run(string[] arguments)
		{
			List<string> parsedArguments = options.Parse(arguments);

			if (showHelp) {
				return ShowHelp();
			}

			if (showVersionInfo) {
				return ShowVersionInfo();
			}

			if (parsedArguments.Count == 0) {
				log.ErrorMessage("mulder: You must provide a command. Run 'mulder help' for more info.");
				return ExitCode.Error;
			}
			
			string commandName = parsedArguments[0].ToLower();
			string[] commandArguments = parsedArguments.Skip(1).ToArray();

			if (commandName == helpKey) {
				if (commandArguments.Length > 0) {
					return ShowCommandHelp(commandArguments[0], commandArguments.Skip(1).ToArray());
				}
				else {
					return ShowHelp();
				}
			}

			if (!commands.ContainsKey(commandName)) {
				log.ErrorMessage("mulder: unknown command '{0}'. Run 'mulder help' for more info.", commandName);
				return ExitCode.Error;
			}
			
			return commands[commandName].Execute(commandArguments);
		}

		ExitCode ShowHelp()
		{
			var help = new StringBuilder();

			help.AppendLine("");
			help.AppendLine("mulder, a simple static site generator written in C#");
			help.AppendLine("");
			help.AppendLine("usage: mulder [options] <command> [<args>]");
			help.AppendLine("");
			help.AppendLine("commands:");
			help.AppendLine("");

			foreach (var keyPair in commands.OrderBy(c => c.Key)) {
				help.AppendFormat("    {0}    {1}{2}", keyPair.Key, keyPair.Value.Summary, Environment.NewLine);
			}

			help.AppendLine("");
			help.AppendLine("    See 'mulder help <command>' for more information on a specific command.");
			help.AppendLine("");

			help.AppendLine("options:");

			help.AppendLine("");

			foreach (Option option in options) {
				help.Append("    ");

				for (int i = 0; i < option.Names.Length; i++) {

					if (i != 0)
						help.Append(" ");

					help.Append(option.Names[i].Length == 1 ? "-" : "--");
					help.Append(option.Names[i]);
				}

				help.AppendFormat("    {0}{1}", option.Description, Environment.NewLine);
			}

			log.InfoMessage(help.ToString());

			return ExitCode.Success;
		}

		ExitCode ShowCommandHelp(string commandName, string[] commandArguments)
		{
			if (!commands.ContainsKey(commandName)) {
				log.ErrorMessage("mulder: unknown command '{0}'. Run 'mulder help' for more info.", commandName);
				return ExitCode.Error;
			}

			return commands[commandName].ShowHelp(commandArguments);
		}

		ExitCode ShowVersionInfo()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var infoVersions = (AssemblyInformationalVersionAttribute[]) assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);

			log.InfoMessage(string.Format("mulder {0} © 2012 Dale Ragan.", infoVersions[0].InformationalVersion));

			return ExitCode.Success;
		}
	}
}
