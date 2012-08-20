using System;
using System.CodeDom.Compiler;

namespace Mulder.Base.Exceptions
{
	public class LoadingSiteException : Exception
	{
		public LoadingSiteException(string message) : base(message)
		{
		}
	}

	public class ErrorCompilingRulesException : LoadingSiteException
	{
		const string ExceptionMessage = "There was a problem compiling the Rules";
		readonly CompilerErrorCollection errors;

		public CompilerErrorCollection Errors { get { return errors; } }

		public ErrorCompilingRulesException(CompilerErrorCollection errors) : base(ExceptionMessage)
		{
			this.errors = errors;
		}
	}
}
