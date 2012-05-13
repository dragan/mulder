using System;
using System.Diagnostics;

using Mulder.Base;
using Mulder.Base.Logging;

namespace Mulder.Base.Commands
{
	public class CompileCommand : ICommand
	{
		static readonly string usage = "compile";
		
		readonly ILog log;
		
		public string Usage { get { return usage; } }
		
		public CompileCommand(ILog log)
		{
			this.log = log;
		}
		
		public ExitCode Execute(string[] arguments)
		{
			log.InfoMessage("Loading site data...");
			
			// TODO: Hook in loading of site data, like cofig, layouts, items, and rules
			
			log.InfoMessage("Compiling site...");
			
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			
			// TODO: Hook in site compilation with a site compiler
			
			stopWatch.Stop();
			
			log.InfoMessage("Site compiled in {0}s.", stopWatch.Elapsed.Seconds);
			
			return ExitCode.Success;
		}
	}
}
