using System;

namespace Mulder.Base.Commands
{
	public interface ICommand
	{
		string Usage { get; }
		ExitCode Execute(string[] arguments);
	}
}
