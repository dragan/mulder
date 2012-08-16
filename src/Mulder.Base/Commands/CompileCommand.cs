using System;
using System.Globalization;
using System.Diagnostics;
using System.Text;

using Mulder.Base;
using Mulder.Base.Compilation;
using Mulder.Base.Domain;
using Mulder.Base.Loading;
using Mulder.Base.Logging;

namespace Mulder.Base.Commands
{
	public class CompileCommand : ICommand
	{
		const string usage = "usage: mulder compile";
		const string summary = "compile items of this site";
		const string description = "Compile all items of the current site.";

		readonly ILog log;
		readonly ILoader loader;
		readonly ICompiler compiler;

		public string Summary { get { return summary; } }
		
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

		public ExitCode ShowHelp(string[] arguments)
		{
			var help = new StringBuilder();

			help.AppendLine("");
			help.AppendLine(usage);
			help.AppendLine("");
			help.AppendLine(summary);
			help.AppendLine("");
			help.AppendLine("    " + description);

			log.InfoMessage(help.ToString());

			return ExitCode.Success;
		}
	}
}
