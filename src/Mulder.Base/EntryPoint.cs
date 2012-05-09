using System;
using System.Collections.Generic;

using Mulder.Base.Logging;

namespace Mulder.Base
{
	public class EntryPoint
	{
		readonly ILog log;
		
		public EntryPoint(ILog log)
		{
			this.log = log;
		}
		
		public ExitCode Run(string[] arguments)
		{
			log.InfoMessage("The Truth Is Out There");
			
			return ExitCode.Success;
		}
	}
}
