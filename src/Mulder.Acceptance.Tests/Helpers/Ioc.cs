using System;
using System.Collections.Generic;
using System.IO;

using Autofac;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.Helpers
{
	public static class Ioc
	{
		public static IContainer CreateAcceptanceTestsContainer(TextWriter textWriter, LogLevel[] logLevels)
		{	
			var builder = new ContainerBuilder();
			
			// Logging
			builder.RegisterInstance(textWriter);
			builder.RegisterType<Log>()
				.As<ILog>()
				.SingleInstance()
				.WithParameter(new NamedParameter("logLevels", logLevels));
			
			// Default DataSource
			builder.RegisterType<FileSystemUnified>().As<IDataSource>();
			
			// FileSystem
			builder.RegisterType<FileSystem>().As<IFileSystem>();
			
			// Commands
			builder.Register(c => {
				return new Dictionary<string, ICommand> {
					{
						"create",
						new CreateCommand(
							c.Resolve<ILog>(),
							new Dictionary<string, ICommand> {
								{ "site", new CreateSiteCommand(c.Resolve<ILog>(), c.Resolve<IFileSystem>(), c.Resolve<IDataSource>()) }
							})
					},
					{
						"compile",
						new CompileCommand(c.Resolve<ILog>())
					}
				};
			}).As<IDictionary<string, ICommand>>();
			
			// EntryPoint
			builder.RegisterType<EntryPoint>();
			
			return builder.Build();
		}
	}
}
