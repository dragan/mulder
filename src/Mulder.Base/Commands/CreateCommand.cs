using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mulder.Base.Logging;

namespace Mulder.Base.Commands
{
	public class CreateCommand : ICommand
	{
		const string usage = "usage: mulder create <object> [<args>]";
		const string summary = "create a mulder object";
		const string description = "Create different objects within mulder using this command.";

		readonly ILog log;
		readonly IDictionary<string, ICommand> subCommands;

		public string Summary { get { return summary; } }
		
		public CreateCommand(ILog log, IDictionary<string, ICommand> subCommands)
		{
			this.log = log;
			this.subCommands = subCommands;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			if (arguments.Length == 0) {
				log.ErrorMessage(usage);
				return ExitCode.Error;
			}
			
			string subCommandName = arguments[0];
			if (!subCommands.ContainsKey(subCommandName)) {
				log.ErrorMessage(usage);
				return ExitCode.Error;
			}
			
			return subCommands[subCommandName].Execute(arguments.Skip(1).ToArray());
		}

		public ExitCode ShowHelp(string[] arguments)
		{
			if (arguments.Length > 0) {
				string subCommandName = arguments[0];
				if (!subCommands.ContainsKey(subCommandName)) {
					log.ErrorMessage("mulder: unknown command '{0}'. Run 'mulder help' for more info.", subCommandName);
					return ExitCode.Error;
				}

				return subCommands[subCommandName].ShowHelp(arguments.Skip(1).ToArray());
			}

			var help = new StringBuilder();

			help.AppendLine("");
			help.AppendLine(usage);
			help.AppendLine("");
			help.AppendLine(summary);
			help.AppendLine("");
			help.AppendLine("    " + description);
			help.AppendLine("");
			help.AppendLine("objects:");
			help.AppendLine("");

			foreach (var keyPair in subCommands.OrderBy(c => c.Key)) {
				help.AppendFormat("    {0}    {1}{2}", keyPair.Key, keyPair.Value.Summary, Environment.NewLine);
			}

			log.InfoMessage(help.ToString());

			return ExitCode.Success;
		}
	}
}
