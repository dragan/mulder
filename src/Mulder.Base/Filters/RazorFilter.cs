using System;
using System.Collections.Generic;

using RazorEngine;

namespace Mulder.Base.Filters
{
	public class RazorFilter : IFilter
	{
		public RazorFilter()
		{
		}

		public string Execute(string source, IDictionary<string, object> arguments)
		{
			return Razor.Parse(source, arguments);
		}
	}
}
