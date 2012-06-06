using System;
using System.Globalization;
using System.Diagnostics;

using Mulder.Base;
using Mulder.Base.Compilation;
using Mulder.Base.Domain;
using Mulder.Base.Loading;
using Mulder.Base.Logging;

namespace Mulder.Base.Commands
{
	public class CompileCommand : ICommand
	{
		static readonly string usage = "compile";
		
		readonly ILog log;
		readonly ILoader loader;
		readonly ICompiler compiler;
		
		public string Usage { get { return usage; } }
		
		public CompileCommand(ILog log, ILoader loader, ICompiler compiler)
		{
			this.log = log;
			this.loader = loader;
			this.compiler = compiler;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			log.InfoMessage("Loading site data...");
			
			Site site = loader.LoadSiteData();
			
			log.InfoMessage("Compiling site...");
			
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			
			compiler.Compile(site);
			
			stopWatch.Stop();
			
			log.InfoMessage("Site compiled in {0}s.", stopWatch.Elapsed.TotalSeconds.ToString("0.000", CultureInfo.InvariantCulture));
			
			return ExitCode.Success;
		}
	}
}
