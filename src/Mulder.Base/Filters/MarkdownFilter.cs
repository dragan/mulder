using System;
using System.Collections.Generic;

using MarkdownSharp;

namespace Mulder.Base.Filters
{
	public class MarkdownFilter : IFilter
	{
		public MarkdownFilter()
		{
		}
		
		public string Execute(string source, IDictionary<string, object> arguments)
		{
			var markdown = new Markdown();
			string output = markdown.Transform(source);
			
			return output;
		}
	}
}
