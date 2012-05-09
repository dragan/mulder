using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mulder.Base.Logging
{
	public class Log : ILog
	{
		readonly TextWriter writer;
		readonly IEnumerable<LogLevel> logLevels;
		
		public Log(TextWriter writer, IEnumerable<LogLevel> logLevels)
		{
			this.writer = writer;
			this.logLevels = logLevels;
		}
		
		public void DebugMessage(string message)
		{
			WriteMessage(message, LogLevel.Debug);
		}
		
		public void DebugMessage(string message, params object[] args)
		{
			WriteMessage(message, LogLevel.Debug, args);
		}
		
		public void InfoMessage(string message)
		{
			WriteMessage(message, LogLevel.Info);
		}
		
		public void InfoMessage(string message, params object[] args)
		{
			WriteMessage(message, LogLevel.Info, args);
		}
		
		public void ErrorMessage(string message)
		{
			WriteMessage(message, LogLevel.Error);
		}
		
		public void ErrorMessage(string message, params object[] args)
		{
			WriteMessage(message, LogLevel.Error, args);
		}
		
		public void ErrorMessage(Exception exception)
		{
			WriteMessage(BuildExceptionMessage(exception), LogLevel.Error);
		}
		
		string BuildExceptionMessage(Exception exception)
		{
			Exception logException = exception;
			
			if (exception.InnerException != null)
				logException = exception.InnerException;
			
			var errorMessage = new StringBuilder();
			errorMessage.AppendLine("Error");
			errorMessage.AppendFormat("{0}Message: {1}", Environment.NewLine, logException.Message);
			errorMessage.AppendFormat("{0}Source: {1}", Environment.NewLine, logException.Source);
			errorMessage.AppendFormat("{0}Stack Trace: {1}", Environment.NewLine, logException.StackTrace);
			errorMessage.AppendFormat("{0}Target Site: {1}", Environment.NewLine, logException.TargetSite);
			
			return errorMessage.ToString();
		}
		
		void WriteMessage(string message, LogLevel logLevel)
		{
			WriteMessage(message, logLevel, null);
		}
		
		void WriteMessage(string message, LogLevel logLevel, params object[] arguments)
		{
			if (logLevels.Contains(logLevel) && !logLevels.Contains(LogLevel.Off)) {
				if (arguments != null) {
					writer.WriteLine(message, arguments);
				} else {
					writer.WriteLine(message);
				}
			}
		}
	}
	
	public enum LogLevel
	{
		Off = 0,
		Debug = 1,
		Info = 2,
		Error = 3
	}
}
