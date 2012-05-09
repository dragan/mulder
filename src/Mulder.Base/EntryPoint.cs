using System;
using System.Collections.Generic;
using System.Linq;

using Mulder.Base.Commands;
using Mulder.Base.Logging;

namespace Mulder.Base
{
	public class EntryPoint
	{
		readonly ILog log;
		readonly IDictionary<string, ICommand> commands;
		
		public EntryPoint(ILog log, IDictionary<string, ICommand> commands)
		{
			this.log = log;
			this.commands = commands;
		}
		
		public ExitCode Run(string[] arguments)
		{
			if (arguments.Length == 0) {
				log.ErrorMessage("usage: [command]");
				return ExitCode.Error;
			}
			
			string commandName = arguments[0];
			if (!commands.ContainsKey(commandName)) {
				log.ErrorMessage("usage: [command]");
				return ExitCode.Error;
			}
			
			return commands[commandName].Execute(arguments.Skip(1).ToArray());
		}
	}
}
