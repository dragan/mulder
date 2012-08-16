using System;
using System.Collections.Generic;
using System.IO;

using Autofac;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Compilation;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Loading;
using Mulder.Base.Logging;

namespace Mulder.Cli
{
	public static class Ioc
	{
		public static IContainer CreateContainer(TextWriter textWriter, LogLevel[] logLevels)
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
			
			// Loading
			builder.RegisterType<Loader>().As<ILoader>().SingleInstance();
			
			// Compilation
			builder.RegisterType<FilterFactory>().As<IFilterFactory>().SingleInstance();
			builder.RegisterType<Compiler>().As<ICompiler>();
			
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
						new CompileCommand(c.Resolve<ILog>(), c.Resolve<ILoader>(), c.Resolve<ICompiler>())
					}
				};
			}).As<IDictionary<string, ICommand>>();
			
			// EntryPoint
			builder.RegisterType<EntryPoint>();
			
			return builder.Build();
		}
	}
}
