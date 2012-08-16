using System;

namespace Mulder.Base.Commands
{
	public interface ICommand
	{
		string Summary { get; }
		ExitCode Execute(string[] arguments);
		ExitCode ShowHelp(string[] arguments);
	}
}
