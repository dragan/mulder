using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mulder.Base.Commands
{
	public class CreateCommand : ICommand
	{
		static readonly string usage = "create [site]";
		readonly TextWriter writer;
		readonly IDictionary<string, ICommand> subCommands;
		
		public string Usage { get { return usage; } }
		
		public CreateCommand(TextWriter writer, IDictionary<string, ICommand> subCommands)
		{
			this.writer = writer;
			this.subCommands = subCommands;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			if (arguments.Length == 0) {
				this.writer.WriteLine("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			string subCommandName = arguments[0];
			if (!subCommands.ContainsKey(subCommandName)) {
				this.writer.WriteLine("usage: {0}", Usage);
				return ExitCode.Error;
			}
			
			return subCommands[subCommandName].Execute(arguments.Skip(1).ToArray());
		}
	}
}
