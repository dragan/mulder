using System;

using Autofac;

using Mulder.Base;
using Mulder.Base.Logging;

namespace Mulder.Cli
{
	class Program
	{
		static int Main(string[] args)
		{
			IContainer container = Ioc.CreateContainer(Console.Out, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			try {
				var entryPoint = container.Resolve<EntryPoint>();
				ExitCode exitCode = entryPoint.Run(args);
				return (int)exitCode;
			} catch (Exception e) {
				container.Resolve<ILog>().ErrorMessage(e);
				return (int)ExitCode.Error;
			}
		}
	}
}
