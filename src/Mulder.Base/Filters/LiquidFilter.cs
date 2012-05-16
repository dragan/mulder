using System;
using System.Collections.Generic;

using DotLiquid;

namespace Mulder.Base.Filters
{
	public class LiquidFilter : IFilter
	{
		public LiquidFilter()
		{
		}
		
		public string Execute(string source, IDictionary<string, object> arguments)
		{
			Hash data = Hash.FromDictionary(arguments);
			Template template = Template.Parse(source);
			string output = template.Render(data);
			
			return output;
		}
	}
}
