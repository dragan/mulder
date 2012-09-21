using System;

using MarkdownSharp;

using Mulder.Base.Compilation;

namespace Mulder.Base.Filters
{
	public class MarkdownFilter : IFilter
	{
		public MarkdownFilter()
		{
		}
		
		public string Execute(string source, FilterContext filterContext)
		{
			var markdown = new Markdown();
			string output = markdown.Transform(source);
			
			return output;
		}
	}
}
