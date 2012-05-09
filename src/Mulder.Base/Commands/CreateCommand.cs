using System;
using System.Collections.Generic;
using System.Linq;

using Mulder.Base.Logging;

namespace Mulder.Base.Commands
{
	public class CreateCommand : ICommand
	{
		static readonly string usage = "create [site]";
		readonly ILog log;
		readonly IDictionary<string, ICommand> subCommands;
		
		public string Usage { get { return usage; } }
		
		public CreateCommand(ILog log, IDictionary<string, ICommand> subCommands)
		{
			this.log = log;
			this.subCommands = subCommands;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			if (arguments.Length == 0) {
				log.ErrorMessage("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			string subCommandName = arguments[0];
			if (!subCommands.ContainsKey(subCommandName)) {
				log.ErrorMessage("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			return subCommands[subCommandName].Execute(arguments.Skip(1).ToArray());
		}
	}
}
