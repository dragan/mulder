using System;

namespace Mulder.Base.Logging
{
	public interface ILog
	{
		void DebugMessage(string message);
		void DebugMessage(string message, params object[] args);
		void InfoMessage(string message);
		void InfoMessage(string message, params object[] args);
		void ErrorMessage(string message);
		void ErrorMessage(string message, params object[] args);
		void ErrorMessage(Exception exception);
	}
}
