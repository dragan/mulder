using System;
using System.Collections.Generic;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Cli
{
	class Program
	{
		static int Main(string[] args)
		{
			ExitCode exitCode;
			ILog log = new Log(Console.Out, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			try {
				var entryPoint = CreateEntryPoint(log);
				exitCode = entryPoint.Run(args);
			} catch (Exception e) {
				log.ErrorMessage(e);
				exitCode = ExitCode.Error;
			}
			
			return (int)exitCode;
		}
		
		static EntryPoint CreateEntryPoint(ILog log)
		{
			var createCommands = new Dictionary<string, ICommand>();
			var fileSystem = new FileSystem();
			createCommands.Add("site", new CreateSiteCommand(log, fileSystem, new FileSystemUnified(log, fileSystem)));
			
			var commands = new Dictionary<string, ICommand>();
			commands.Add("create", new CreateCommand(log, createCommands));
			
			return new EntryPoint(log, commands);
		}
	}
}
