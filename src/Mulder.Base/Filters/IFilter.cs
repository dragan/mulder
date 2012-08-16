using System;
using System.Collections.Generic;

namespace Mulder.Base.Filters
{
	public interface IFilter
	{
		string Execute(string source, IDictionary<string, object> arguments);
	}
}
