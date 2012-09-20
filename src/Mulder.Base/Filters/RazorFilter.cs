using System;

using RazorEngine;

namespace Mulder.Base.Filters
{
	public class RazorFilter : IFilter
	{
		public RazorFilter()
		{
		}

		public string Execute(string source, dynamic model)
		{
			return Razor.Parse(source, model);
		}
	}
}
